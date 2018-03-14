// COPYLEFT 2018 Vasiliy Stankevich, Kharkov, Ukraine.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitcoinNet.Client
{
    public class BitcoinNetClient : IBitcoinNetClient
    {
        public BitcoinNetClient(Uri server, string login, string password)
        {
            Server = server;
            Login = login;
            Password = password;
        }

        public void BackupWallet(string destination)
        {
            ExecuteRequest();
        }

        public string GetAccount(string address)
        {
            return ExecuteRequest<string>(address);
        }

        public string GetAccountAddress(string account = "")
        {
            return ExecuteRequest<string>(account);
        }

        public List<string> GetAddressesByAccount(string account = "")
        {
            return ExecuteRequest<List<string>>(account);
        }

        public double GetBalance(string account = "", int minConf = 1)
        {
            return ExecuteRequest<double>(account, minConf);
        }

        public string GetBlockByCount(int height)
        {
            return ExecuteRequest<string>(height);
        }

        public int GetBlockCount()
        {
            return ExecuteRequest<int>();
        }

        public int GetBlockNumber()
        {
            return ExecuteRequest<int>();
        }

        public int GetConnectionCount()
        {
            return ExecuteRequest<int>();
        }

        public double GetDifficulty()
        {
            return ExecuteRequest<double>();
        }

        public bool GetGenerate()
        {
            return ExecuteRequest<bool>();
        }

        public double GetHashesPerSec()
        {
            return ExecuteRequest<double>();
        }

        public GetInfoResponse GetInfo()
        {
            return ExecuteRequest<GetInfoResponse>();
        }

        public string GetNewAddress(string account)
        {
            return ExecuteRequest<string>(account);
        }

        public double GetReceivedByAccount(string account, int minConf = 1)
        {
            return ExecuteRequest<double>(account, minConf);
        }

        public double GetReceivedByAddress(string address, int minConf = 1)
        {
            return ExecuteRequest<double>(address, minConf);
        }

        public GetTransactionResponse GetTransaction(string txid)
        {
            return ExecuteRequest<GetTransactionResponse>(txid);
        }

        public JObject GetWork()
        {
            return ExecuteRequest<JObject>();
        }

        public bool GetWork(string data)
        {
            return ExecuteRequest<bool>(data);
        }

        public string Help(string command = "")
        {
            return ExecuteRequest<string>(command);
        }

        public Dictionary<string, double> ListAccounts(int minConf = 1)
        {
            return ExecuteRequest<Dictionary<string, double>>(minConf);
        }

        public List<ListReceivedByAccountResponseElement> ListReceivedByAccount(int minConf = 1, bool includeEmpty = false)
        {
            return ExecuteRequest<List<ListReceivedByAccountResponseElement>>(minConf, includeEmpty);
        }

        public List<ListReceivedByAddressResponseElement> ListReceivedByAddress(int minConf = 1, bool includeEmpty = false)
        {
            return ExecuteRequest<List<ListReceivedByAddressResponseElement>>(minConf, includeEmpty);
        }

        public List<ListTransactionResponseElement> ListTransactions(string account="", int count = 10)
        {
            return ExecuteRequest<List<ListTransactionResponseElement>>(account, count);
        }

        public bool Move(string fromAccount, string toAccount, float amount, int minConf = 1, string comment = "")
        {
            return ExecuteRequest<bool>(fromAccount, toAccount, amount, minConf, comment);
        }

        public string SendFrom(string fromAccount, string toAddress, float amount, int minConf = 1, string comment = "",
            string commentTo = "")
        {
            return ExecuteRequest<string>(fromAccount, toAddress, amount, minConf, comment);
        }

        public string SendToAddress(string address, float amount, string comment, string commentTo)
        {
            return ExecuteRequest<string>(address, amount, comment, commentTo);
        }

        public void SetAccount(string address, string account="")
        {
            ExecuteRequest(address, account);
        }

        public void SetGenerate(bool generate, int genProcLimit = 1)
        {
            ExecuteRequest(generate, genProcLimit);
        }

        public void Stop()
        {
            ExecuteRequest();
        }

        public ValidateAddressResponse ValidateAddress(string address)
        {
            return ExecuteRequest<ValidateAddressResponse>(address);
        }

        protected string ExecuteRequest(params object[] @params)
        {
            string content = string.Empty;
            HttpResponseMessage response = null;
            var method = GetNameCallMethod(nameof(ExecuteRequest));
            var request = 0 != @params.Length ? new BitcoinNetRequest(method, @params) : new BitcoinNetRequest(method);
            var jsonRequest = JsonConvert.SerializeObject(request);
            try
            {
                using (var client = GetHttpClient())
                using (response = client
                    .PostAsync(Server, new StringContent(jsonRequest, Encoding.UTF8, "application/json-rpc")).Result)
                {
                    content = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException)
            {
                throw new JsonRpcException(BaseResponseStatus.Create(Convert.ToInt32(response.StatusCode), content));
            }
            catch (Exception exception)
            {
                throw new JsonRpcException(BaseResponseStatus.Create(500, exception.Message));
            }
            return content;
        }

        protected T ExecuteRequest<T>(params object[] @params)
        {
            var jsonResponse = ExecuteRequest(@params);
            return JsonConvert.DeserializeObject<BitcoinNetResponse<T>>(jsonResponse).Result;
        }

        protected HttpClient GetHttpClient()
        {
            var basicAuthorization = $"{Login}:{Password}";
            var byteArrayBasicAuthorization = Encoding.ASCII.GetBytes(basicAuthorization);
            return new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArrayBasicAuthorization))
                }
            };
        }

        protected string GetNameCallMethod(string excludeMethodName)
        {
            var index = 1;
            var result = new StackTrace(1).GetFrame(index).GetMethod().Name.ToLowerInvariant();
            while (string.Compare(excludeMethodName, result, StringComparison.OrdinalIgnoreCase) == 0)
                result = new StackTrace(1).GetFrame(index++).GetMethod().Name.ToLowerInvariant();
            return result;
        }

        public virtual void Dispose()
        {
        }

        protected Uri Server { get; set; }
        protected string Login { get; set; }
        protected string Password { get; set; }
    }
}
