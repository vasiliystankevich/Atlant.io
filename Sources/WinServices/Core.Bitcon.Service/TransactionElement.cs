using System;
using BitcoinNet.Client;
using Libraries.Core.Backend.Common;

namespace Core.Bitcon.Service
{
    public class TransactionElement
    {
        public TransactionElement(string txId, int confirmations, long time, string account, string address, float amount)
        {
            TxId = txId;
            Confirmations = confirmations;
            Time = time.FromUnixTime();
            Account = account;
            Address = address;
            Amount = amount;
        }

        public string TxId { get; set; }
        public int Confirmations { get; set; }
        public DateTime Time { get; set; }
        public string Account { get; set; }
        public string Address { get; set; }
        public float Amount { get; set; }
    }
}
