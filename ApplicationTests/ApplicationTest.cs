using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace ApplicationTests
{
    public class ApplicationTest
    {
        const string publishFolderName = "publish";

        [Fact]
        public void TestApplication()
        {
            // remove previous publish folder
            if (Directory.Exists(publishFolderName))
            {
                try
                {
                    Directory.Delete(publishFolderName, recursive: true);
                }
                catch
                {
                    // try again...
                    Directory.Delete(publishFolderName, recursive: true);
                }
            }

            // publish app
            Directory.CreateDirectory(publishFolderName);
            var pathToCsproj = Path.Combine( 
                string.Join(
                    Path.DirectorySeparatorChar,  
                    Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).TakeWhile(f => f != "ApplicationTests")
                ),
                "CryptoInvest",
                "CryptoInvest.csproj"
            );
            Console.WriteLine(pathToCsproj);
            var publishProcess = Process.Start("dotnet", @$"publish {pathToCsproj} -o publish");
            publishProcess.WaitForExit();
            Assert.Equal(0, publishProcess.ExitCode);

            // create input file
            File.WriteAllText(Path.Combine(publishFolderName, "input-test.json"),
@"
{
    ""from"":""2021-01-01"",
    ""to"":""2021-01-31"",
    ""sleepInSeconds"":10,
    ""buyingInterval"":""10.00:00:00"",
    ""enableRebalancing"":true,
    ""rebalancingInterval"":""14.00:00:00"",
    ""investAmount"":100,
    ""topCoinsCount"":3,
    ""referenceTotalMarketCap"":""TopCoins"",
    ""coinsToIgnore"":[""USDT"", ""TUSD"", ""PAX"", ""USDC"", ""BUSD""]
}
"
            );

            // run app
            var appProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(publishFolderName, "CryptoInvest"),
                    Arguments = Path.Combine(publishFolderName, "input-test.json"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            appProcess.Start();

            var lines = new List<string>();
            while (!appProcess.StandardOutput.EndOfStream)
            {
                lines.Add(appProcess.StandardOutput.ReadLine());
            }

            appProcess.WaitForExit();

            // assert output
            Assert.Equal(0, appProcess.ExitCode);
            Assert.Equal(28, lines.Count);
            Assert.Contains(lines, l => l.Contains("From") && l.Contains("2021") && l.Contains("1"));
            Assert.Contains(lines, l => l.Contains("To") && l.Contains("2021") && l.Contains("31"));
            Assert.Contains(lines, l => l.Contains("SleepInSeconds") && l.Contains("10"));
            Assert.Contains(lines, l => l.Contains("BuyingInterval") && l.Contains("10"));
            Assert.Contains(lines, l => l.Contains("EnableRebalancing") && l.Contains("rue"));
            Assert.Contains(lines, l => l.Contains("RebalancingInterval") && l.Contains("14"));
            Assert.Contains(lines, l => l.Contains("InvestAmount") && l.Contains("100"));
            Assert.Contains(lines, l => l.Contains("TopCoinsCount") && l.Contains("3"));
            Assert.Contains(lines, l => l.Contains("ReferenceTotalMarketCap") && l.Contains("TopCoins"));
            Assert.Contains(lines, l => l.Contains("CoinsToIgnore") && l.Contains("USDT, TUSD, PAX, USDC, BUSD"));
            Assert.Contains(lines, l => l.Contains("2021 01 03"));
            Assert.Contains(lines, l => l.Contains("2021 01 10"));
            Assert.Contains(lines, l => l.Contains("2021 01 17"));
            Assert.Contains(lines, l => l.Contains("2021 01 24"));
            Assert.Contains(lines, l => l.Contains("2021 01 31"));
            Assert.Contains(lines, l => l.Contains("Initial market cap: 871400373306"));
            Assert.Contains(lines, l => l.Contains("Final market cap: 984768537695"));
            Assert.Contains(lines, l => l.Contains("Market cap change: 13.01"));
            Assert.Contains(lines, l => l.Contains("Invested: 300"));
            Assert.Contains(lines, l => l.Contains("Final value: 301.5716"));
            Assert.Contains(lines, l => l.Contains("Investing result: 0.52"));
            Assert.Contains(lines, l => l.Contains("Bitcoin: 0.007112207 BTC (235.52 $)"));
            Assert.Contains(lines, l => l.Contains("Ethereum: 0.043738295 ETH (57.52 $)"));
            Assert.Contains(lines, l => l.Contains("XRP: 17.347135998 XRP (8.54 $)"));
            Assert.Contains(lines, l => l.Contains("Litecoin: 0.000000000 LTC (0.00 $)"));
            Assert.Contains(lines, l => l.Contains("Polkadot: 0.000000000 DOT (0.00 $)"));
        }

        //03.01. (BR)
        //TOP 3 MARKET CAP: 609,409,213,147 + 111,309,985,611 + 10,610,100,157 = 731329298915
        //BTC: 609,409,213,147 / 731329298915 * 100 = 83.3289756135
        //ETH: 111,309,985,611 / 731329298915 * 100 = 15.2202278476
        //LTC: 10,610,100,157 / 731329298915 * 100 = 1.45079653895
        //
        //14.01. (BR)
        //BTC: (35,791.28 / 32,782.02) * 83.3289756135 = 90.97824656
        //ETH: (1,230.17 / 975.51) * 15.2202278476 = 19.1935169207
        //LTC: (142.43 / 160.19) * 1.45079653895 = 1.28994912943
        //
        //TOTAL VALUE: 90.97824656 + 19.1935169207 + 1.28994912943 = 111.46171261
        //TOP 3 MARKET CAP: 665,831,621,391 + 140,601,384,430 + 15,306,722,570 = 821739728391
        //BTC: (665,831,621,391 / 821739728391) * 211.46171261 = 171.341228986
        //ETH: (140,601,384,430 / 821739728391) * 211.46171261 = 36.1815408452
        //DOT: (15,306,722,570 / 821739728391) * 211.46171261 = 3.93894277868
        //LTC: 0
        //
        //21.01. (B)
        //BTC: (32,289.38 / 35,791.28) * 171.341228986 = 154.576814587
        //ETH: (1,391.61 / 1,230.17) * 36.1815408452 = 40.9297853594
        //DOT: (18.00 / 16.98) * 3.93894277868 = 4.17555771592
        //LTC: 0
        //TOP 3 MARKET CAP: 600,888,568,010 + 159,184,556,292 + 16,268,654,954 = 776341779256
        //BTC: 154.576814587 + 600,888,568,010 / 776341779256 * 100 = 231.976818563
        //ETH: 40.9297853594 + 159,184,556,292 / 776341779256 * 100 = 61.4342281893
        //DOT: 4.17555771592 + 16,268,654,954 / 776341779256 * 100 = 6.27111091023
        //
        //28.01. (R)
        //BTC: (33114.36 / 32,289.38) * 231.976818563 = 237.903728147
        //ETH: (1,314.99 / 1,391.61) * 61.4342281893 = 58.0517499347
        //DOT: (16.12 / 18.00) * 6.27111091023 = 5.61612821516
        //LTC: 0
        //
        //TOTAL VALUE: 237.903728147 + 58.0517499347 + 5.61612821516 = 301.571606297
        //TOP 3 MARKET CAP: 616,452,744,533 + 150,543,956,657 + 22,353,042,408 = 789349743598
        //BTC: (616,452,744,533 / 789349743598) * 301.571606297 = 235.516190235
        //ETH: (150,543,956,657 / 789349743598) * 301.571606297 = 57.5154210102
        //XRP: (22,353,042,408 / 789349743598) * 301.571606297 = 8.53999505198
        //LTC: 0
        //DOT: 0
        //BTC Units: 235.516190235 / 33,114.36 = 0.00711220721
        //ETH Units: 57.5154210102 / 1,314.99 = 0.04373829535
        //XRP Units: 8.53999505198 / 0.4923 = 17.3471359983
    }
}
