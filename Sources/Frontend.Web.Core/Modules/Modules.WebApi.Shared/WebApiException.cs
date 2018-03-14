using System;
using Modules.WebApi.Shared.Responses;

namespace Modules.WebApi.Shared
{
    public class WebApiException:Exception
    {
        public WebApiException(BaseResponseStatus status):base(status.Message)
        {
            Status = status;
        }
        public BaseResponseStatus Status { get; set; }
    }
}
