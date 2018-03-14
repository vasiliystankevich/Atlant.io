using System.Collections.Generic;
using Newtonsoft.Json;

namespace Libraries.Core.Backend.Recaptcha
{
    public class RecaptchaResultModel
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}