using System;
using System.Collections.Generic;

namespace Modules.WebApi.Shared.Responses
{
    [Serializable]
    public class GetAddressForAccountResponse : OkResponse
    {
        public GetAddressForAccountResponse()
        {
        }

        public GetAddressForAccountResponse(string address)
        {
            Address = address;
        }

        public string Address { get; set; }
    }

    [Serializable]
    public class GetLastResponseDataElement
    {
        public GetLastResponseDataElement(DateTime time, string address, float amount, int confirmation)
        {
            Time = time;
            Address = address;
            Amount = amount;
            Confirmation = confirmation;
        }

        public DateTime Time { get; set; }
        public string Address { get; set; }
        public float Amount { get; set; }
        public int Confirmation { get; set; }
    }

    [Serializable]
    public class GetLastResponse : OkResponse
    {
        public GetLastResponse()
        {
        }
        public GetLastResponse(List<GetLastResponseDataElement> data)
        {
            Data = data;
        }

        public List<GetLastResponseDataElement> Data { get; set; }
    }
}
