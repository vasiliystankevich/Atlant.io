using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Libraries.Core.Backend.Common
{
    public class UnityHttpControllerSelector: DefaultHttpControllerSelector
    {
        public UnityHttpControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            string controllerName = GetControllerName(request);
            var targetType = AppDomain.CurrentDomain.FindApiController(controllerName);
            return new HttpControllerDescriptor(_configuration, controllerName, targetType);
        }

        private readonly HttpConfiguration _configuration;
    }
}
