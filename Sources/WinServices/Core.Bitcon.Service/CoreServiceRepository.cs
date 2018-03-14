using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BitcoinNet.Client;
using Dal;
using log4net;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Newtonsoft.Json;
using Project.Kernel;

namespace Core.Bitcon.Service
{
    public interface ICoreServiceRepository:IDisposable
    {
        void DataProcessingApiRequest();
        IExecutor Executor { get; set; }
    }

    public class CoreServiceRepository:BaseRepository, ICoreServiceRepository
    {
        public CoreServiceRepository(IWrapper<ILog> logger, IExecutor excecutor) : base(logger)
        {
            var ipAddressCoreBitcoinService = ConfigurationManager.AppSettings["IpAddressCoreBitcoinService"];
            var maxCountConnectionsForTcpListeners =
                Convert.ToInt32(ConfigurationManager.AppSettings["MaxCountConnectionsForTcpListeners"]);
            var portForSendBtc = Convert.ToInt32(ConfigurationManager.AppSettings["PortForSendBtc"]);
            var portForGetLast = Convert.ToInt32(ConfigurationManager.AppSettings["PortForGetLast"]);
            var portForGetAddressForAccount = Convert.ToInt32(ConfigurationManager.AppSettings["PortForGetAddressForAccount"]);
            MaxGenerationForConfirmation = Convert.ToInt32(ConfigurationManager.AppSettings["MaxGenerationForConfirmation"]);
            MinCoutConfirmations = Convert.ToInt32(ConfigurationManager.AppSettings["MinCoutConfirmations"]);
            UriBitcoinRpcServer = ConfigurationManager.AppSettings["UriBitcoinRpcServer"];
            LoginBitcoinRpcServer = ConfigurationManager.AppSettings["LoginBitcoinRpcServer"];
            PasswordBitcoinRpcServer = ConfigurationManager.AppSettings["PasswordBitcoinRpcServer"];

            var emptySendBtcResponse = BaseResponse.Ok();
            var emptyGetLastResponse = new GetLastResponse(Enumerable.Empty<GetLastResponseDataElement>().ToList());
            CaсheSendBtcResponse = JsonConvert.SerializeObject(emptySendBtcResponse);
            CaсheGetLastResponse = JsonConvert.SerializeObject(emptyGetLastResponse);
            var ipAddressForTcpListener = IPAddress.Parse(ipAddressCoreBitcoinService);
            Executor = excecutor;
            Servers = new Dictionary<string, TcpListener>
            {
                {"SendBtc", new TcpListener(ipAddressForTcpListener, portForSendBtc)},
                {"GetLast", new TcpListener(ipAddressForTcpListener, portForGetLast)},
                {"GetAddressForAccount", new TcpListener(ipAddressForTcpListener, portForGetAddressForAccount)},
            };
            OnDataProcessingActions = new Dictionary<string, Func<string, string>>
            {
                {"SendBtc", SendBtcOnDataProcessingAction},
                {"GetLast", GetLastOnDataProcessingAction},
                {"GetAddressForAccount", GetAddressForAccountOnDataProcessingAction},
            };
            Servers.Keys.ForEach(key =>
            {
                Servers[key].Start(maxCountConnectionsForTcpListeners);
                Servers[key].BeginAcceptTcpClient(ServerBeginAcceptTcpClient, key);
            });          
        }

        public void DataProcessingApiRequest()
        {
            Executor.ExecuteAction(ExecuteSendBtcRequests);
            Executor.ExecuteAction(GetStateBitcoinWallets);
            Executor.ExecuteAction(GenerateContentForGetLast);
        }

        protected void ServerBeginAcceptTcpClient(IAsyncResult state)
        {
            var key = (string) state.AsyncState;
            Servers[key].BeginAcceptTcpClient(ServerBeginAcceptTcpClient, key);
            var client = Servers[key].EndAcceptTcpClient(state);
            using (var clientStream = client.GetStream())
            {
                string json;
                using (var textReader = new StreamReader(clientStream))
                    json = textReader.ReadToEnd();
                var dataProcessingResult = OnDataProcessingActions[key](json);
                using (var textWriter = new StreamWriter(clientStream) { AutoFlush = true })
                    textWriter.Write(dataProcessingResult);
                clientStream.Flush();
                clientStream.Close();
            }
        }

        protected string SendBtcOnDataProcessingAction(string json)
        {
            var request = JsonConvert.DeserializeObject<SendBtcRequest>(json);
            var requestTime = DateTime.UtcNow.PruningMilisecond();
            var container = UnityConfig.GetConfiguredContainer();
            using (var context = container.Resolve<IDalContext>())
            {
                context.SendBtcRequests.Add(new SendBtcModel
                {
                    Address = request.Address,
                    Amount = request.Amount,
                    Time = requestTime,
                    IsExecute = false
                });
                context.SaveChangesAsync().Wait();
            }
            var result = CaсheSendBtcResponse;
            return result;
        }

        protected string GetLastOnDataProcessingAction(string json)
        {
            var result = CaсheGetLastResponse;
            return result;
        }

        protected string GetAddressForAccountOnDataProcessingAction(string json)
        {
            string address;
            GetDataForRpcBitcoinServer(out var uriServer, out var login, out var password);
            var request = JsonConvert.DeserializeObject<GetAddressForAccountRequest>(json);
            var server = new Uri(uriServer);
            using (var client = new BitcoinNetClient(server, login, password))
                address = client.GetNewAddress(request.Account);
            var response = new GetAddressForAccountResponse(address);
            return JsonConvert.SerializeObject(response);
        }

        protected void GetDataForRpcBitcoinServer(out string uriServer, out string login, out string password)
        {
            uriServer = UriBitcoinRpcServer;
            login = LoginBitcoinRpcServer;
            password = PasswordBitcoinRpcServer;
        }

        protected void ExecuteSendBtcRequests()
        {
            var container = UnityConfig.GetConfiguredContainer();
            GetDataForRpcBitcoinServer(out var uriServer, out var login, out var password);
            var server = new Uri(uriServer);
            using (var context = container.Resolve<IDalContext>())
            {
                var requests = context.SendBtcRequests.Where(sbr => !sbr.IsExecute).ToList();
                using (var client = new BitcoinNetClient(server, login, password))
                {
                    requests.ForEach(sbr => client.SendToAddress(sbr.Address, sbr.Amount,
                        "send bitcoin for execute request in Core.Bitcon.Service",
                        "send bitcoin for execute request in Core.Bitcon.Service"));
                }
                requests.ForEach(sbr => sbr.IsExecute = true);
                context.SaveChangesAsync().Wait();
            }
        }

        protected void GetStateBitcoinWallets()
        {
            var container = UnityConfig.GetConfiguredContainer();
            GetDataForRpcBitcoinServer(out var uriServer, out var login, out var password);
            var server = new Uri(uriServer);
            using (var context = container.Resolve<IDalContext>())
            {
                List<ListReceivedByAddressResponseElement> wallets;
                List<GetTransactionResponse> transactions;
                using (var client = new BitcoinNetClient(server, login, password))
                {
                    wallets = client.ListReceivedByAddress(0).ToList();
                    var transactionsIds = wallets.SelectMany(w => w.TxIds);
                    transactions = transactionsIds.Select(t => client.GetTransaction(t)).ToList();
                }
                UpdateWallets(context, wallets);
                UpdateTransactions(context, transactions, "send", GetOutgoingDbTransactions, AddOutgoingTransactionInDb);
                UpdateTransactions(context, transactions, "receive", GetIncomingDbTransactions, AddIncomingTransactionInDb);
            }
        }

        protected void GenerateContentForGetLast()
        {
            List<GetLastResponseDataElement> data;
            var container = UnityConfig.GetConfiguredContainer();
            using (var context = container.Resolve<IDalContext>())
            {
                var jointIds = new Dictionary<string, bool>();
                var allTransactions = context.IncomingTransactions.ToList();
                allTransactions.Where(transaction => !transaction.IsInterrogation)
                    .Select(transaction => transaction.TxId).ForEach(
                        txid =>
                        {
                            if (!jointIds.ContainsKey(txid)) jointIds[txid] = true;
                        });
                allTransactions.Where(t => t.Confirmations < MinCoutConfirmations)
                    .Select(transaction => transaction.TxId).ForEach(
                        txid =>
                        {
                            if (!jointIds.ContainsKey(txid)) jointIds[txid] = true;
                        });
                var jointTransactions = allTransactions.Join(jointIds.Keys, transaction => transaction.TxId, key => key,
                    (transaction, key) => transaction).ToList();
                data = jointTransactions.Select(transaction =>
                    new GetLastResponseDataElement(transaction.TimeTransaction, transaction.Address, transaction.Amount,
                        transaction.Confirmations)).ToList();
                jointTransactions.ForEach(transaction => transaction.IsInterrogation = true);
                context.SaveChangesAsync().Wait();
            }
            var result = new GetLastResponse(data);
            lock (LockObject)
            {
                CaсheGetLastResponse = JsonConvert.SerializeObject(result);
            }
        }

        protected void UpdateTransactions<TDbTransactionModel>(IDalContext context,
            List<GetTransactionResponse> transactions, string category,
            Func<IDalContext, List<TDbTransactionModel>> funcGetDbTransactions,
            Action<HotWalletModel, TransactionElement> actionAddTransactionInDb)
            where TDbTransactionModel : TransactionModel
        {
            long time;
            var dictionaryRpcTransactions = new Dictionary<string, List<TransactionElement>>();
            transactions.ForEach(transaction =>
            {
                var detailTransaction = transaction.Details.First(d =>
                    string.Compare(d.Category, category, StringComparison.OrdinalIgnoreCase) == 0);
                time = GetTransactionTime(transaction, category);
                var result = new TransactionElement(transaction.TxId, transaction.Confirmations, time,
                    detailTransaction.Account, detailTransaction.Address, Convert.ToSingle(detailTransaction.Amount));
                if (!dictionaryRpcTransactions.ContainsKey(detailTransaction.Address))
                    dictionaryRpcTransactions[result.Address] = new List<TransactionElement>();
                dictionaryRpcTransactions[result.Address].Add(result);
            });
            var updateDbTransactionsIds =
                UpdateDbTransactions(context, dictionaryRpcTransactions, funcGetDbTransactions);
            AddTransactionsInDb(context, dictionaryRpcTransactions, updateDbTransactionsIds, actionAddTransactionInDb);
        }

        protected long GetTransactionTime(GetTransactionResponse transaction, string category)
        {
            if (string.Compare("send", category, StringComparison.OrdinalIgnoreCase) == 0) return transaction.Time;
            return string.Compare("receive", category, StringComparison.OrdinalIgnoreCase) == 0 ? transaction.TimeReceived : 0;
        }

        protected List<IncomingTransactionModel> GetIncomingDbTransactions(IDalContext context)
        {
            return context.HotWallets.SelectMany(wallet => wallet.IncomingTransactions)
                .Where(transaction => transaction.Confirmations <= MaxGenerationForConfirmation).ToList();
        }

        protected List<OutgoingTransactionModel> GetOutgoingDbTransactions(IDalContext context)
        {
            return context.HotWallets.SelectMany(wallet => wallet.OutgoingTransactions)
                .Where(transaction => transaction.Confirmations <= MaxGenerationForConfirmation).ToList();
        }

        protected List<string> UpdateDbTransactions<TDbTransactonModel>(IDalContext context,
            Dictionary<string, List<TransactionElement>> rpcTransactions,
            Func<IDalContext, List<TDbTransactonModel>> getDbTransactions) where TDbTransactonModel : TransactionModel
        {
            var dbTransactions = getDbTransactions(context);
            var transactionsForUpdate = dbTransactions.Where(t => t.Confirmations < MaxGenerationForConfirmation).ToList();
            if (dbTransactions.Count == 0) return Enumerable.Empty<string>().ToList();
            foreach (var dbTransaction in transactionsForUpdate)
            {
                if (!rpcTransactions.ContainsKey(dbTransaction.Address)) continue;
                var isExistTransactionElement = rpcTransactions[dbTransaction.Address].Any(te =>
                    string.Compare(te.TxId, dbTransaction.TxId, StringComparison.OrdinalIgnoreCase) == 0);
                if (!isExistTransactionElement) continue;
                var rpcTransaction = rpcTransactions[dbTransaction.Address].First(te =>
                    string.Compare(te.TxId, dbTransaction.TxId, StringComparison.OrdinalIgnoreCase) == 0);
                dbTransaction.Confirmations = rpcTransaction.Confirmations;
            }
            context.SaveChangesAsync().Wait();
            return dbTransactions.Select(transaction => transaction.TxId).ToList();
        }

        protected void AddTransactionsInDb(IDalContext context, Dictionary<string, List<TransactionElement>> rpcTransactions,
            List<string> updateDbTransactionsIds, Action<HotWalletModel, TransactionElement> actionAddTransactionInDb)
        {
            var localWallets = new Dictionary<string, HotWalletModel>();
            var localTransactions = new Dictionary<string, TransactionElement>();
            var dbWallets = context.HotWallets.ToList();
            dbWallets.ForEach(dbWallet => localWallets[dbWallet.Address] = dbWallet);
            var addRpcTransactionsIds = rpcTransactions.Keys
                .SelectMany(key => rpcTransactions[key].Select(te => te.TxId)).Except(updateDbTransactionsIds).ToList();
            if (addRpcTransactionsIds.Count==0) return;
            rpcTransactions.Keys.SelectMany(key => rpcTransactions[key])
                .ForEach(te => localTransactions[te.TxId] = te);
            addRpcTransactionsIds.ForEach(id =>
            {
                var localTransaction = localTransactions[id];
                var localWallet = localWallets[localTransaction.Address];
                if (localTransaction.Confirmations <= MaxGenerationForConfirmation)
                    actionAddTransactionInDb(localWallet, localTransaction);
            });
            context.SaveChangesAsync().Wait();
        }

        protected void AddIncomingTransactionInDb(HotWalletModel dbWallet, TransactionElement localTransaction)
        {
            dbWallet.IncomingTransactions.Add(new IncomingTransactionModel
            {
                TxId = localTransaction.TxId,
                Account = localTransaction.Account,
                Address = localTransaction.Address,
                Amount = localTransaction.Amount,
                Confirmations = localTransaction.Confirmations,
                TimeTransaction = localTransaction.Time,
            });
        }

        protected void AddOutgoingTransactionInDb(HotWalletModel dbWallet, TransactionElement localTransaction)
        {
            dbWallet.OutgoingTransactions.Add(new OutgoingTransactionModel
            {
                TxId = localTransaction.TxId,
                Account = localTransaction.Account,
                Address = localTransaction.Address,
                Amount = localTransaction.Amount,
                Confirmations = localTransaction.Confirmations,
                TimeTransaction = localTransaction.Time,
            });
        }

        protected void UpdateWallets(IDalContext context, List<ListReceivedByAddressResponseElement> wallets)
        {
            var walletsAdresses = wallets.Select(w => w.Address).ToList();
            var walletsDictionary = new Dictionary<string, ListReceivedByAddressResponseElement>(wallets.Count);
            wallets.ForEach(wallet => walletsDictionary[wallet.Address] = wallet);
            var dbWallets = context.HotWallets.ToList();
            var dbWalletsAddresses = dbWallets.Select(dbWallet => dbWallet.Address).ToList();
            dbWallets.ForEach(dbWallet => dbWallet.Amount = walletsDictionary[dbWallet.Address].Amount);
            context.SaveChangesAsync().Wait();
            walletsAdresses.Except(dbWalletsAddresses).ForEach(address =>
            {
                var addWallet = walletsDictionary[address];
                context.HotWallets.Add(new HotWalletModel
                {
                    Account = addWallet.Account,
                    Address = addWallet.Address,
                    Amount = addWallet.Amount
                });
            });
            context.SaveChangesAsync().Wait();
        }

        public void Dispose()
        {
            Servers.Keys.ForEach(key =>
            {
                Servers[key].Stop();
                Servers[key] = null;
            });
        }

        public IExecutor Executor { get; set; }
        private Dictionary<string, TcpListener> Servers { get; } 
        private Dictionary<string, Func<string, string>> OnDataProcessingActions { get; }
        private string CaсheGetLastResponse { get; set; }
        private string CaсheSendBtcResponse { get; }
        private string UriBitcoinRpcServer { get; }
        private string LoginBitcoinRpcServer { get; }
        private string PasswordBitcoinRpcServer { get; }
        private int MaxGenerationForConfirmation { get; }
        private int MinCoutConfirmations { get; }

        private static readonly object LockObject = new object();
    }
}
