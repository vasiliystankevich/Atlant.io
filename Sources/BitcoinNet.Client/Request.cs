using System;
using Newtonsoft.Json;

namespace BitcoinNet.Client
{
    public class BitcoinNetRequest
    {
        public BitcoinNetRequest()
        {
            JsonRpc = 1.0;
            Id = $"{Guid.NewGuid():N}";
        }

        public BitcoinNetRequest(string method) : this()
        { Method = method; }

        public BitcoinNetRequest(string method, params object[] @params) : this(method)
        { Params = @params; }

        [JsonProperty("jsonrpc")]
        public double JsonRpc { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public object[] Params { get; set; }
    }
}
