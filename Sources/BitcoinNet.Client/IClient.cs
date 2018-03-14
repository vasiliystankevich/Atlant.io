// COPYRIGHT 2011 Konstantin Ineshin, Irkutsk, Russia.
// If you like this project please donate BTC 18TdCC4TwGN7PHyuRAm8XV88gcCmAHqGNs
// Modification, COPYLEFT 2018 Vasiliy Stankevich, Kharkov, Ukraine.

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BitcoinNet.Client
{
    public interface IBitcoinNetClient: IDisposable
    {
        void BackupWallet(string destination);
        string GetAccount(string address);
        string GetAccountAddress(string account);
        List<string> GetAddressesByAccount(string account);
        double GetBalance(string account, int minConf);
        string GetBlockByCount(int height);
        int GetBlockCount();
        int GetBlockNumber();
        int GetConnectionCount();
        double GetDifficulty();
        bool GetGenerate();
        double GetHashesPerSec();
        GetInfoResponse GetInfo();
        string GetNewAddress(string account);
        double GetReceivedByAccount(string account, int minConf);
        double GetReceivedByAddress(string address, int minConf);
        GetTransactionResponse GetTransaction(string txid);
        JObject GetWork();
        bool GetWork(string data);
        string Help(string command);
        Dictionary<string, double> ListAccounts(int minConf);
        List<ListReceivedByAccountResponseElement> ListReceivedByAccount(int minConf, bool includeEmpty);
        List<ListReceivedByAddressResponseElement> ListReceivedByAddress(int minConf, bool includeEmpty);
        List<ListTransactionResponseElement> ListTransactions(string account, int count);
        bool Move(string fromAccount, string toAccount, float amount, int minConf, string comment);
        string SendFrom(string fromAccount, string toAddress, float amount, int minConf, string comment, string commentTo);
        string SendToAddress(string address, float amount, string comment, string commentTo);
        void SetAccount(string address, string account);
        void SetGenerate(bool generate, int genProcLimit);
        void Stop();
        ValidateAddressResponse ValidateAddress(string address);
    }
}
