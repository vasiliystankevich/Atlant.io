using System;
using System.Configuration;
using System.Web.Helpers;
using log4net;
using Libraries.Core.Backend.Common;
using Project.Kernel;

namespace Libraries.Core.Backend.External
{
    public interface ISenderEmailRepository
    {
        void SendEmail(string email, string subject, string body);
    }

    public class FakeSenderEmailRepository : BaseRepository, ISenderEmailRepository
    {
        public FakeSenderEmailRepository(IWrapper<SenderEmailRepository> senderEmailRepository, IWrapper<ILog> logger) : base(logger)
        {
            SenderEmailRepository = senderEmailRepository;
        }

        public void SendEmail(string email, string subject, string body)
        {
            Logger.Instance.Info($"Send email from {email}");
            Logger.Instance.Info($"Email subject: {subject}");
            Logger.Instance.Info($"Email body: {body}");
            if (email.Contains("shlep")) //replace on olegshlepchenko@gmail.com
                SenderEmailRepository.Instance.SendEmail("vasiliyStankevich@gmail.com", subject, body);
        }

        private IWrapper<SenderEmailRepository> SenderEmailRepository { get; set; }
    }

    public class SenderEmailRepository : BaseRepository, ISenderEmailRepository
    {
        public SenderEmailRepository(IWrapper<ILog> logger) : base(logger)
        {
        }

        public void SendEmail(string email, string subject, string body)
        {
            WebMail.SmtpServer = ConfigurationManager.AppSettings["EMailServerHost"];
            WebMail.SmtpPort = int.Parse(ConfigurationManager.AppSettings["EMailServerPort"]);
            WebMail.UserName = ConfigurationManager.AppSettings["EMailServerUsername"];
            WebMail.Password = ConfigurationManager.AppSettings["EMailServerPassword"];
            WebMail.From = ConfigurationManager.AppSettings["ServerEmailAddress"];
            WebMail.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            WebMail.Send(email, subject, body);
        }
    }
}
