using System.Data.Entity.Migrations;

namespace Logs.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LogsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LogsContext context)
        {
        }
    }
}
