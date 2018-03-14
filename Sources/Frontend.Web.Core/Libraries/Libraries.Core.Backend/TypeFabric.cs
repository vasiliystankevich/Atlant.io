using System;
using log4net;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.External;
using Libraries.Core.Backend.Recaptcha;
using Microsoft.Practices.Unity;
using Project.Kernel;

namespace Libraries.Core.Backend
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IWrapper<Random>, Wrapper<Random>>(new InjectionFactory(factory => new Wrapper<Random>(new Random(DateTime.UtcNow.Millisecond))));
            container.RegisterType<IRecaptchaRepository, RecaptchaRepository>();
            container.RegisterType<IWrapper<SenderEmailRepository>, Wrapper<SenderEmailRepository>>(
                new InjectionFactory(factory =>
                {
                    var instanceConteiner = UnityConfig.GetConfiguredContainer();
                    var logWrapper = instanceConteiner.Resolve<IWrapper<ILog>>();
                    var senderEmailRepository = new SenderEmailRepository(logWrapper);
                    return new Wrapper<SenderEmailRepository>(senderEmailRepository);
                }));
            container.RegisterType<ISenderEmailRepository, FakeSenderEmailRepository>();
        }
    }
}
