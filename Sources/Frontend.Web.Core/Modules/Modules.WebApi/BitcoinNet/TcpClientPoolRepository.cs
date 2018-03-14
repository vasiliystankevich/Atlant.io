using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;
using Libraries.Core.Backend.Common;
using Project.Kernel;

namespace Modules.WebApi.BitcoinNet
{
    public interface ITcpClientPoolRepository:IDisposable
    {
        string SendMessage(string json, IPEndPoint ipEndPoint);
    }
    public class TcpClientPoolRepository:BaseRepository, ITcpClientPoolRepository
    {
        public TcpClientPoolRepository(IWrapper<ILog> logger) : base(logger)
        {
            NumberClient = 0;
            CountTcpClient = Convert.ToInt32(ConfigurationManager.AppSettings["CountTcpClients"]);
            Clients = new List<TcpClient>(CountTcpClient);
            for (var index=0; index < CountTcpClient; index++)
                Clients[index] = new TcpClient();
        }
        public string SendMessage(string json, IPEndPoint ipEndPoint)
        {
            var result = string.Empty;
            TcpClient localClient;
            lock (LockObject)
            {
                localClient = GetLocalClient();
            }
            localClient.Connect(ipEndPoint);
            using (var ns = localClient.GetStream())
            {
                using (var textWriter = new StreamWriter(ns) { AutoFlush = true })
                    textWriter.Write(json);
                ns.Flush();
                using (var textReader = new StreamReader(ns))
                    result = textReader.ReadToEnd();
                ns.Close();
            }
            localClient.Close();
            lock (LockObject)
            {
                ReleaseLocalClient();
            }
            return result;
        }

        protected TcpClient GetLocalClient()
        {
            if (NumberClient < CountTcpClient) return Clients[NumberClient++];
            Clients.Add(new TcpClient());
            CountTcpClient = Clients.Count;
            return Clients[NumberClient++];
        }

        protected void ReleaseLocalClient()
        {
            var index = NumberClient - 1;
            if (Clients[index].Connected)
                Clients[index].Close();
            NumberClient--;
        }

        public void Dispose()
        {
            lock (LockObject)
            {
                for (var index = 0; index < NumberClient; index++)
                    if (Clients[index].Connected) Clients[index].Close();
                for (var index = 0; index < CountTcpClient; index++)
                    Clients[index] = null;
                Clients = null;
                NumberClient = 0;
            }
        }

        protected int CountTcpClient { get; set; }
        protected int NumberClient { get; set; }
        protected List<TcpClient> Clients { get; set; }
        private static readonly object LockObject= new object();
    }
}
