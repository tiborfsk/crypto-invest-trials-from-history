using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvest
{
    public class Wallet
    {
        private readonly PriceBoard priceBoard = new PriceBoard();

        private readonly Dictionary<string, SingleCoinWallet> singleCoinWallets = new Dictionary<string, SingleCoinWallet>();

        public decimal Value => singleCoinWallets.Sum(w => w.Value.Value);

        public List<SingleCoinWallet> SingleCoinWallets => singleCoinWallets.Values.ToList();

        public Wallet(PriceBoard priceBoard)
        {
            this.priceBoard = priceBoard;
        }

        public bool HasSingleCoinWallet(string coinId) => singleCoinWallets.ContainsKey(coinId);

        public SingleCoinWallet GetSingleCoinWallet(string coinId)
        {
            if (!singleCoinWallets.ContainsKey(coinId))
            {
                throw new InvalidOperationException("Single coin wallet not created.");
            }
            return singleCoinWallets[coinId];
        }

        public void AddSingleCoinWallet(string coinId, string coinName)
        {
            if (singleCoinWallets.ContainsKey(coinId))
            {
                throw new InvalidOperationException("Single coin wallet already created.");
            }
            singleCoinWallets.Add(coinId, new SingleCoinWallet(coinId, coinName, priceBoard));
        }

        public SingleCoinWallet GetOrAddSingleCoinWallet(string coinId, string coinName)
        {
            if (!HasSingleCoinWallet(coinId))
            {
                AddSingleCoinWallet(coinId, coinName);
            }

            return GetSingleCoinWallet(coinId);
        }

        public void RemoveSingleCoinWallet(string coinId)
        {
            if (!singleCoinWallets.ContainsKey(coinId))
            {
                throw new InvalidOperationException("Not existing wallet.");
            }
            singleCoinWallets.Remove(coinId);
        }

        public override string ToString() => $"Total value: {Value:0.0000}, {string.Join(", ", SingleCoinWallets.OrderByDescending(scw => scw.Value))}";
    }
}
