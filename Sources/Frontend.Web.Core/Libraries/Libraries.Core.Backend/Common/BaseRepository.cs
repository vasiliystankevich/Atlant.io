using log4net;
using Project.Kernel;

namespace Libraries.Core.Backend.Common
{
    public abstract class BaseRepository
    {
        protected BaseRepository(IWrapper<ILog> logger)
        {
            Logger = logger;
        }
        public IWrapper<ILog> Logger { get; set; }
    }
}