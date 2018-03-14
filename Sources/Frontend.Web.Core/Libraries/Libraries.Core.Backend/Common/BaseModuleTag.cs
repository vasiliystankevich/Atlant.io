namespace Libraries.Core.Backend.Common
{
    public interface ITag
    {
        string AssemblyName { get; set; }
    }

    public abstract class BaseTag:ITag
    {
        protected BaseTag()
        {
            AssemblyName = GetType().Assembly.GetName().Name;
        }

        public string AssemblyName { get; set; }
    }

    public class LibraryTag : BaseTag { }
    public class ModuleTag : BaseTag { }
    public class ThemeTag : BaseTag { }
}