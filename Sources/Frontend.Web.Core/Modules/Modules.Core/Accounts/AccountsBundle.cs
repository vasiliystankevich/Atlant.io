using Libraries.Core.Backend.Common;

namespace Modules.Core.Accounts
{
    public class AccountsBundle:ModuleBaseBundleConfig
    {
        public override void RegisterBundles()
        {
            Controller("Accounts");
            Action("Accounts","Login");
        }
    }
}
