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
    ""to"":""2021-01-20"",
    ""sleepInSeconds"":10,
    ""buyingInterval"":""14.00:00:00"",
    ""enableRebalancing"":true,
    ""rebalancingInterval"":""7.00:00:00"",
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
                    FileName = Path.Combine(publishFolderName, "CryptoInvest.exe"),
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

            Assert.Equal(0, appProcess.ExitCode);
            Assert.Contains(lines, l => l.Contains("From") && l.Contains("2021") && l.Contains("1"));
            Assert.Contains(lines, l => l.Contains("To") && l.Contains("2021") && l.Contains("20") && l.Contains("1"));
            Assert.Contains(lines, l => l.Contains("SleepInSeconds") && l.Contains("10"));
            Assert.Contains(lines, l => l.Contains("BuyingInterval") && l.Contains("14"));
            Assert.Contains(lines, l => l.Contains("EnableRebalancing") && l.Contains("rue"));
            Assert.Contains(lines, l => l.Contains("RebalancingInterval") && l.Contains("7"));
            Assert.Contains(lines, l => l.Contains("InvestAmount") && l.Contains("100"));
            Assert.Contains(lines, l => l.Contains("TopCoinsCount") && l.Contains("3"));
            Assert.Contains(lines, l => l.Contains("ReferenceTotalMarketCap") && l.Contains("TopCoins"));
            Assert.Contains(lines, l => l.Contains("CoinsToIgnore") && l.Contains("USDT, TUSD, PAX, USDC, BUSD"));
            Assert.Contains(lines, l => l.Contains("2021 01 03"));
            Assert.Contains(lines, l => l.Contains("2021 01 10"));
            Assert.Contains(lines, l => l.Contains("2021 01 17"));
            Assert.Contains(lines, l => l.Contains("Investing result: "));
        }
    }
}
