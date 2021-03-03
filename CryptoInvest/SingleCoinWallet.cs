using System;

namespace CryptoInvest
{
    public class SingleCoinWallet
    {
        private const decimal valueEps = 0.01M;

        private readonly PriceBoard priceBoard;

        public string CoinId { get; }

        public string CoinName { get; }

        public decimal Units { get; private set; }

        public decimal Value => Units * priceBoard.GetPrice(CoinId);

        public SingleCoinWallet(string coinId, string coinName, PriceBoard priceBoard)
        {
            CoinId = coinId;
            CoinName = coinName;
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
            if (IsToBeSoldApproximatellyAll(units))
            {
                Units = 0;
            }
            else if (units > Units)
            {
                throw new InvalidOperationException("Not enough units in wallet");
            }
            else
            {
                Units -= units;
            }
        }

        public void SellForCash(decimal cash)
        {
            var unitsToBeSold = cash / priceBoard.GetPrice(CoinId);
            if (IsToBeSoldApproximatellyAll(unitsToBeSold))
            {
                Units = 0;
            }
            else if (unitsToBeSold > Units)
            {
                throw new InvalidOperationException("Not enough units in wallet");
            }
            else
            {
                Units -= unitsToBeSold;
            }
        }

        public decimal SellAll()
        {
            var value = Value;
            Units = 0;
            return value;
        }

        private bool IsToBeSoldApproximatellyAll(decimal units) =>
            Math.Abs(units - Units) * priceBoard.GetPrice(CoinId) < valueEps;

        public override string ToString() => $"{CoinName}: {Units:0.000000000} {CoinId} ({Value:0.00} $)";
    }
}
