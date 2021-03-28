# crypto-invest-trials-from-history

Simulates passive periodic investing in cryptos and displays results in comparison to total market cap change. The investment ratio to certain cryptocurrency depends simply on its market cap.  

## Run

Simply run `CryptoInvest` binary. By default this takes `input.json` (bundled within release package) with input parameters. For using another file use additional argument: `CryptoInvest my-input.json`.

## Input parameters

Parameter name | Default value in input.json | Description
---|:---:|---
from | 2021-01-01 | Date of the first investment
to | 2021-01-31 | Date of the last investment
sleepInSeconds | 10 | Sleep between fetches of data (preventing blocking access due to high usage)
buyingInterval | 7.00:00:00 | Interval of buying new cryptos
enableRebalancing | true | Flag indicating whether rebalancing should perform
rebalancingInterval | 14.00:00:00 | Interval of rebalancing
investAmount | 100 | Amount of money in USD to apply in single investment
topCoinsCount | 10 | Number of top coins (with highest market cap) to invest
notTopCoinsDistribution | AmongNewTopCoins | How to distribute investments from coins which are not already top coins. Two options are available: AmongNewTopCoins (investments will be distributed among new top coins), AmongAllTopCoins (investments will be distributed among all current top coins)
referenceTotalMarketCap | TopCoins | How to compute reference total market cap. Two options are available: TopCoins (market caps sum of top coins), AllCoins (total market cap of all cryptos)
coinsToIgnore | ["USDT", "TUSD", "PAX", "USDC", "BUSD"] | Array of coin IDs to ignore

## Example output

    Initial market cap: $871400373306
    Final market cap: $984768537695
    Market cap change: 13.01%
    Invested: $500.0000
    Final value: $497.9441
    Investing result: -0.41%
    Wallet:
    Total value: $497.9441
    Bitcoin: 0.010860592 BTC ($359.64)
    Ethereum: 0.066789921 ETH ($87.83)
    XRP: 26.489689201 XRP ($13.04)
    Polkadot: 0.528510089 DOT ($8.52)
    Cardano: 18.151051686 ADA ($6.26)
    Chainlink: 0.235684819 LINK ($5.33)
    Litecoin: 0.038734136 LTC ($5.02)
    Bitcoin Cash: 0.010876073 BCH ($4.35)
    Binance Coin: 0.090147021 BNB ($3.99)
    Stellar: 12.982897898 XLM ($3.97)
    Wrapped Bitcoin: 0.000000000 WBTC ($0.00)
