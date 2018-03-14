using System;

namespace Modules.WebApi.Shared.Requests
{
    [Serializable]
    public class GetAddressForAccountRequest : BaseRequest
    {
        public string Account { get; set; }
    }

    [Serializable]
    public class SendBtcRequest:BaseRequest
    {
        public string Address { get; set; }
        public float Amount { get; set; }
    }
}
