using System;
using System.Configuration;
using System.Net;
using log4net;
using Libraries.Core.Backend.Common;
using Newtonsoft.Json;
using Project.Kernel;

namespace Libraries.Core.Backend.Recaptcha
{
    public interface IRecaptchaRepository
    {
        bool Validate(string response);
    }

    public class RecaptchaRepository:BaseRepository, IRecaptchaRepository
    {
        public RecaptchaRepository(IWrapper<ILog> logger):base(logger)
        {
            PrivateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
        }

        public bool Validate(string response)
        {
            var verifySiteTemplateString =
                $"https://www.google.com/recaptcha/api/siteverify?secret={PrivateKey}&response={response}";
            using (var client = new WebClient())
            {
                var googleReply = client.DownloadString(verifySiteTemplateString);
                var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResultModel>(googleReply);
                return string.Compare(captchaResponse.Success, "True", StringComparison.OrdinalIgnoreCase) == 0;
            }
        }
        protected string PrivateKey { get; set; }
    }
}