using System;
using Libraries.Core.Backend.Common;
using Microsoft.Practices.Unity;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            var container = UnityConfig.GetConfiguredContainer();
            Creator<TypeFabric>.Create().RegisterTypes(container);
            var dataLoader = container.Resolve<IDataLoader>();
            dataLoader.ExecuteAction(dataLoader.Seed, nameof(dataLoader.Seed));
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey(true);
        }
    }
}
