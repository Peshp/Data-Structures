using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinWalletManager
{
    public class WalletManager : IWalletManager
    {
        private readonly Dictionary<string, Transaction> pendingTransactions = new Dictionary<string, Transaction>();
        private readonly List<Transaction> executedTransactions = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            pendingTransactions.Add(transaction.Hash, transaction);
        }

        public Transaction BroadcastTransaction(string txHash)
        {
            if (!pendingTransactions.TryGetValue(txHash, out var transaction))
            {
                throw new ArgumentException();
            }

            pendingTransactions.Remove(txHash);
            executedTransactions.Add(transaction);

            return transaction;
        }

        public Transaction CancelTransaction(string txHash)
        {
            if (!pendingTransactions.TryGetValue(txHash, out var transaction))
            {
                throw new ArgumentException();
            }

            pendingTransactions.Remove(txHash);

            return transaction;
        }

        public bool Contains(string txHash)
        {
            return pendingTransactions.ContainsKey(txHash);
        }

        public int GetEarliestNonceByUser(string user)
        {
            var earliestNonce = pendingTransactions.Values
                .Where(t => t.From == user)
                .OrderBy(t => t.Nonce)
                .Select(t => t.Nonce)
                .FirstOrDefault();

            return earliestNonce;
        }

        public IEnumerable<Transaction> GetPendingTransactionsByUser(string user)
        {
            var pendingByUser = pendingTransactions.Values
                .Where(t => t.From == user)
                .OrderBy(t => t.Nonce)
                .ThenBy(t => t.Hash);

            return pendingByUser.ToList(); 
        }

        public IEnumerable<Transaction> GetHistoryTransactionsByUser(string user)
        {
            var historyByUser = executedTransactions
                .Where(t => t.From == user || t.To == user)
                .OrderBy(t => t.Hash);

            return historyByUser.ToList(); 
        }
    }
}
