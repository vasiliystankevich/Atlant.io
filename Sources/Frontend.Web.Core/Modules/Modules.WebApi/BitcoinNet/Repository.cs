using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;
using Libraries.Core.Backend.Common;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Newtonsoft.Json;
using Project.Kernel;

namespace Modules.WebApi.BitcoinNet
{
    public interface IBitcoinNetApiRepository:IDisposable
    {
        BaseResponse SendBtc(SendBtcRequest request);
        GetLastResponse GetLast(BaseRequest request);
        GetAddressForAccountResponse GetAddressForAccount(GetAddressForAccountRequest request);
        ITcpClientPoolRepository TcpClientPool { get; set; }
    }
    public class BitcoinNetApiRepository : BaseRepository, IBitcoinNetApiRepository
    {
        public BitcoinNetApiRepository(IWrapper<ILog> logger, ITcpClientPoolRepository tcpClientPool) : base(logger)
        {
            var ipAddressCoreBitcoinService = ConfigurationManager.AppSettings["IpAddressCoreBitcoinService"];
            var portForSendBtc = Convert.ToInt32(ConfigurationManager.AppSettings["PortForSendBtc"]);
            var portForGetLast = Convert.ToInt32(ConfigurationManager.AppSettings["PortForGetLast"]);
            var portForGetAddressForAccount = Convert.ToInt32(ConfigurationManager.AppSettings["PortForGetAddressForAccount"]);

            WebApiEndPoints = new Dictionary<string, IPEndPoint>
            {
                { nameof(SendBtc), new IPEndPoint(IPAddress.Parse(ipAddressCoreBitcoinService), portForSendBtc)},
                { nameof(GetLast), new IPEndPoint(IPAddress.Parse(ipAddressCoreBitcoinService), portForGetLast)},
                { nameof(GetAddressForAccount), new IPEndPoint(IPAddress.Parse(ipAddressCoreBitcoinService), portForGetAddressForAccount)}
            };
            TcpClientPool = tcpClientPool;
        }

        public BaseResponse SendBtc(SendBtcRequest request)
        {
            return SendMessage<BaseRequest, GetLastResponse>(request, nameof(SendBtc));
        }

        public GetLastResponse GetLast(BaseRequest request)
        {
            return SendMessage<BaseRequest, GetLastResponse>(request, nameof(GetLast));
        }

        public GetAddressForAccountResponse GetAddressForAccount(GetAddressForAccountRequest request)
        {
            return SendMessage<GetAddressForAccountRequest, GetAddressForAccountResponse>(request, nameof(GetAddressForAccount));
        }

        protected TResponse SendMessage<TRequest, TResponse>(TRequest request, string webApiName)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var responseJson = TcpClientPool.SendMessage(requestJson, WebApiEndPoints[webApiName]);
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public void Dispose()
        {
        }

        protected Dictionary<string, IPEndPoint> WebApiEndPoints { get; set; }
        public ITcpClientPoolRepository TcpClientPool { get; set; }
    }
}
