using System;

namespace Modules.WebApi.Shared.Requests
{
    [Serializable]
    public class BaseRequest
    {
        public string Version { get; set; } = "1.0.0.0";
    }
}
