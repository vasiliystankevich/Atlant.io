using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Web.Configuration;
using Modules.WebApi.Shared;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Newtonsoft.Json;

namespace WebApi.Client
{
    public interface IWebApiClient : IDisposable
    {
        Guid Login(LoginAccountRequest request);
        void Logout();
        void SendBtc(string address, float amount);
        List<GetLastResponseDataElement> GetLast();
        string GetAddressForAccount(string account);
    }

    public class WebApiClient: IWebApiClient
    {
        public WebApiClient(Uri baseUri)
        {
            UriActions = new Dictionary<string, Uri>
            {
                {nameof(Login).ToLowerInvariant(), new Uri(baseUri, "api/accounts/login")},
                {nameof(Logout).ToLowerInvariant(), new Uri(baseUri, "api/accounts/logout")},
                {nameof(SendBtc).ToLowerInvariant(), new Uri(baseUri, "api/bitcoinnet/sendbtc") },
                {nameof(GetLast).ToLowerInvariant(), new Uri(baseUri, "api/bitcoinnet/getlast") },
                {nameof(GetAddressForAccount).ToLowerInvariant(), new Uri(baseUri, "api/bitcoinnet/getaddressforaccount") }
            };
            var webApiClientTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings["webApiClientTimeout"]);
            Client = new HttpClient {Timeout = new TimeSpan(0, 0, 0, webApiClientTimeout) };
        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        private string GetNameAction()
        {
            var index = 1;
            var result = new StackTrace(1).GetFrame(index).GetMethod().Name.ToLowerInvariant();
            while (string.Compare(nameof(ExecuteRequest), result, StringComparison.OrdinalIgnoreCase) == 0)
                result = new StackTrace(1).GetFrame(index++).GetMethod().Name.ToLowerInvariant();
            return result;
        }

        public TResponse ExecuteRequest<TRequest, TResponse>(TRequest request)
            where TRequest : BaseRequest
            where TResponse : BaseResponse
        {
            var nameAction = GetNameAction();
            HttpResponseMessage response = null;
            var content = string.Empty;
            try
            {
                using (response = Client.PostAsJsonAsync(UriActions[nameAction], request).Result)
                {
                    content = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException e)
            {
                throw new WebApiException(BaseResponseStatus.Create(Convert.ToInt32(response.StatusCode), content));
            }
            catch (Exception e)
            {
                throw new WebApiException(BaseResponseStatus.Create(500, e.Message));
            }
            var message = JsonConvert.DeserializeObject<BaseResponse>(content);
            if (message.Status.Code != 200) throw new WebApiException(message.Status);
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public Guid Login(LoginAccountRequest request)
        {
            var response = ExecuteRequest<LoginAccountRequest, LoginAccountResponse>(request);
            Client.DefaultRequestHeaders.Add(response.Coockie.Key, new List<string> { response.Coockie.Value});
            return response.UserId;
        }

        public void Logout()
        {
            ExecuteRequest<BaseRequest, BaseResponse>(new BaseRequest());
            Client.DefaultRequestHeaders.Clear();
        }

        public void SendBtc(string address, float amount)
        {
            var request = new SendBtcRequest {Address = address, Amount = amount};
            ExecuteRequest<SendBtcRequest, OkResponse>(request);
        }

        public List<GetLastResponseDataElement> GetLast()
        {
            var response = ExecuteRequest<BaseRequest, GetLastResponse>(new BaseRequest());
            return response.Data;
        }

        public string GetAddressForAccount(string account)
        {
            var request = new GetAddressForAccountRequest {Account = account};
            var response = ExecuteRequest<GetAddressForAccountRequest, GetAddressForAccountResponse>(request);
            return response.Address;
        }

        private HttpClient Client { get; }
        private Dictionary<string, Uri> UriActions { get; }
    }
}
