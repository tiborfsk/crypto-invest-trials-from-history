using System;

namespace CryptoInvest
{
    public class SingleCoinWallet
    {
        private readonly PriceBoard priceBoard;

        public string CoinId { get; }

        public decimal Units { get; private set; }

        public decimal Value => Units * priceBoard.GetPrice(CoinId);

        public SingleCoinWallet(string coinId, PriceBoard priceBoard)
        {
            CoinId = coinId;
            this.priceBoard = priceBoard;
        }

        public void BuyUnits(decimal units)
        {
            Units += units;
        }

        public void BuyByCash(decimal cash)
        {
            Units += cash / priceBoard.GetPrice(CoinId);
        }

        public void SellUnits(decimal units)
        {
            if (units > Units)
            {
                throw new InvalidOperationException("Not enough units in wallet");
            }
            Units -= units;
        }

        public void SellByCash(decimal cash)
        {
            var unitsToBeSold = cash / priceBoard.GetPrice(CoinId);
            if (unitsToBeSold > Units)
            {
                throw new InvalidOperationException("Not enough units in wallet");
            }
            Units -= unitsToBeSold;
        }

        public decimal SellAll()
        {
            var price = priceBoard.GetPrice(CoinId);
            Units = 0;
            return price;
        }
    }
}
