using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Libraries.Core.Backend.Common;
using Project.Kernel;

namespace Modules.Site.Home
{
    public interface IHomeRepository
    {
        HomeModel GetLoadModules();
    }

    public class HomeRepository:BaseRepository, IHomeRepository
    {
        public HomeRepository(IWrapper<ILog> logger) : base(logger)
        {
        }

        protected List<string> GetLoadModules(Type baseType)
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.GetTypes().Any(t => t.BaseType == baseType))
                    .Select(a => a.GetName().Name)
                    .ToList();
        }

        public HomeModel GetLoadModules()
        {
            var result = new List<string>();
            var libraries = GetLoadModules(typeof(LibraryTag));
            var modules = GetLoadModules(typeof(ModuleTag));
            var themes = GetLoadModules(typeof(ThemeTag));
            result.AddRange(libraries);
            result.AddRange(modules);
            result.AddRange(themes);
            return new HomeModel {Modules = result};
        }
    }
}
