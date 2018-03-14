using System;

namespace BitcoinNet.Client
{
    public class BaseResponseStatus
    {
        public BaseResponseStatus() { }

        protected BaseResponseStatus(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static BaseResponseStatus Create(int code, string message)
        {
            return new BaseResponseStatus(code, message);
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class JsonRpcException : Exception
    {
        public JsonRpcException(BaseResponseStatus status) : base(status.Message)
        {
            Status = status;
        }
        public BaseResponseStatus Status { get; set; }
    }
}
