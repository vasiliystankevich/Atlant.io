using System.Data.Entity;
using System.Threading.Tasks;


namespace Logs
{
    public interface ILogsContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbSet<SeedModel> Seeding { get; set; }
        IDbSet<FrontendWebLogsModel> FrontendWebLogs { get; set; }
        IDbSet<CoreBitconServiceLogsModel> CoreBitconServiceLogs { get; set; }
    }
    public class LogsContext : DbContext, ILogsContext
    {
        public LogsContext()
            : base("LogsConnection")
        {
        }

        public LogsContext(string nameOrConnectionString):base(nameOrConnectionString)
        {
        }

        public virtual IDbSet<SeedModel> Seeding { get; set; }
        public virtual IDbSet<FrontendWebLogsModel> FrontendWebLogs { get; set; }
        public virtual IDbSet<CoreBitconServiceLogsModel> CoreBitconServiceLogs { get; set; }
    }
}
