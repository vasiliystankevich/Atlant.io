using System;
using System.Data.Entity;
using System.Data.SqlClient;
using Dal;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DataGenerator
{
    public interface ISaContext : IDbContext
    {
        void ExecuteServerScript(string script);
    }

    public class SaContext: DbContext, ISaContext
    {
        public SaContext() : base("SaContextConnection") { }
        public void ExecuteTransaction(Action action)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ExecuteServerScript(string script)
        {
            var serverConnection = new ServerConnection((SqlConnection)Database.Connection);
            var server = new Server(serverConnection);
            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
