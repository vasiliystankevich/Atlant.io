using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using BitcoinNet.Client;
using Dal;
using log4net;
using Libraries.Core.Backend.Authorization;
using Logs;
using Project.Kernel;

namespace DataGenerator
{
    public interface IDataLoader
    {
        void Seed();
        void CreateBtcTransactions();
        void ExecuteAction(Action action, string nameAction);
        ISaContext SaContext { get; set; }
        IDalContext DalContext { get; set; }
        ILogsContext LogsContext { get; set; }
        IWrapper<ILog> Log { get; set; }
        IIdentityRepository IdentityRepository { get; set; }
    }

    public class DataLoader:IDataLoader
    {
        public DataLoader(ISaContext saContext, IDalContext dalContext, ILogsContext logsContext, IWrapper<ILog> log, IIdentityRepository identityRepository)
        {
            SaContext = saContext;
            DalContext = dalContext;
            LogsContext = logsContext;
            Log = log;
            IdentityRepository = identityRepository;
            Random = new Random(DateTime.UtcNow.Second);
            NameBuilder = new StringBuilder();
        }

        public void RecreateDb()
        {
            var instanceSqlServer = ConfigurationManager.AppSettings["InstanceSqlServer"];
            var folderSqlServer = ConfigurationManager.AppSettings["folderSqlServer"];
            var workSqlDbName = ConfigurationManager.AppSettings["WorkSqlDbName"];
            var logsSqlDbName = ConfigurationManager.AppSettings["LogsSqlDbName"];
            var sqlDbUser = ConfigurationManager.AppSettings["SqlDbUser"];

            ExecuteDeploymentScripts("RecreateDb.sql", instanceSqlServer, folderSqlServer, workSqlDbName, sqlDbUser);
            ExecuteDeploymentScripts("RecreateDb.sql", instanceSqlServer, folderSqlServer, logsSqlDbName, sqlDbUser);
        }

        public void WorkDbMigrations()
        {
            var workSqlDbName = ConfigurationManager.AppSettings["WorkSqlDbName"];
            ExecuteDeploymentScripts("WorkDbMigrations.sql", workSqlDbName);
        }
        public void LogsDbMigrations()
        {
            var logsSqlDbName = ConfigurationManager.AppSettings["LogsSqlDbName"];
            ExecuteDeploymentScripts("LogsMigrations.sql", logsSqlDbName);
        }
        public void ExecuteDeploymentScripts(string templateSqlFileName, params string[] args)
        {
            var pathToFolderSqlScript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlScripts");
            var pathToTemplateFileScript = Path.Combine(pathToFolderSqlScript, templateSqlFileName);
            var sqlTemplateScript = File.ReadAllText(pathToTemplateFileScript);
            var sqlScript = string.Format(sqlTemplateScript, args);
            SaContext.ExecuteServerScript(sqlScript);
        }

        public void ExecuteDeploymentScripts()
        {
            ExecuteAction(RecreateDb, nameof(RecreateDb));
            ExecuteAction(WorkDbMigrations, nameof(WorkDbMigrations));
            ExecuteAction(LogsDbMigrations, nameof(LogsDbMigrations));
        }

        public void Seed()
        {
            ExecuteAction(ExecuteDeploymentScripts, nameof(ExecuteDeploymentScripts));

            ExecuteAction(InitIdentity, nameof(InitIdentity));
            ExecuteAction(CreateBtcTransactions, nameof(CreateBtcTransactions));

            DalContextSeeding();
        }

        private void DalContextSeeding()
        {
            DalContext.ExecuteTransaction(() =>
            {
                DalContext.Seeding.Add(new Dal.SeedModel {IsSeed = true});
                DalContext.SaveChanges();
            });
        }

        private void LogsContextSeeding()
        {
            LogsContext.Seeding.Add(new Logs.SeedModel { IsSeed = true });
            LogsContext.SaveChanges();
        }

        public void ExecuteAction(Action action, string nameAction)
        {
            Log.Instance.Info($"{nameAction} begin...");
            action();
            Log.Instance.Info($"{nameAction} end...");
        }

        protected List<string> GetLocalAccounts(int countAccounts)
        {
            var result = new List<string>();
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < countAccounts; i++)
            {
                NameBuilder.Clear();
                for (var j = 0; j < 6; j++)
                    NameBuilder.Append(alphabet[Random.Next(26)]);
                result.Add($"{NameBuilder}@ageron.info");
            }
            return result;
        }

        public void CreateBtcTransactions()
        {
            var uriBitcoinRpcServer = ConfigurationManager.AppSettings["UriBitcoinRpcServer"];
            var loginBitcoinRpcServer = ConfigurationManager.AppSettings["LoginBitcoinRpcServer"];
            var passwordBitcoinRpcServer = ConfigurationManager.AppSettings["PasswordBitcoinRpcServer"];
            StopBitcoinNodeService();
            StartBitcoinNodeService();
            GenerateLocalBitcoins();
            var accounts = GetLocalAccounts(80);
            using (var client = new BitcoinNetClient(new Uri("http://localhost:18332"), loginBitcoinRpcServer, passwordBitcoinRpcServer))
            {
                for (var i = 0; i <= 7; i++)
                {
                    Log.Instance.Info($"Dispatch of bitcoins, {i}-th generation...");
                    for (var j = 0; j < 10; j++)
                    {
                        var address = client.GetNewAddress(accounts[i*10+j]);
                        for (var k = 0; k < 2; k++)
                            client.SendToAddress(address, 50.00f, "send bitcoin for init data", "send bitcoin for init data");
                    }
                    if (i>6) continue;
                    Log.Instance.Info($"Confirm bitcoin transactions, {i}-th generation");
                    ExternalGenerationHashBlocks(1);
                }
            }
        }

        private static void StartHideExecuteFile(string pathToFile, string workingDirectory, string arguments)
        {
            var executeFileProcess = new Process
            {
                StartInfo =
                {
                    FileName = pathToFile,
                    WorkingDirectory = workingDirectory,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            executeFileProcess.Start();
            executeFileProcess.WaitForExit();
        }

        private static void ExternalGenerationHashBlocks(int count)
        {
            var pathToFile = ConfigurationManager.AppSettings["BitcoinCliExeFile"];
            var argumentsStringFormat = ConfigurationManager.AppSettings["BitcoinCliExeFileArgumentsStringFormat"];
            var workingDirectory = Path.GetDirectoryName(pathToFile);
            var arguments = string.Format(argumentsStringFormat, count);
            StartHideExecuteFile(pathToFile, workingDirectory, arguments);
        }

        private void StartBitcoinNodeService()
        {
            
            var nameService = ConfigurationManager.AppSettings["BitcoinNodeServiceName"];
            var dataDirService = ConfigurationManager.AppSettings["BitcoinNodeServiceDataDir"];
            Log.Instance.Info("Start bitcoin node service...");
            if (Directory.Exists(dataDirService)) Directory.Delete(dataDirService, true);
            using (var sc = new ServiceController(nameService))
            {
                if (sc.Status != ServiceControllerStatus.Stopped) return;
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        private void StopBitcoinNodeService()
        {
            var nameService = ConfigurationManager.AppSettings["BitcoinNodeServiceName"];
            Log.Instance.Info("Stop bitcoin node service...");
            using (var sc = new ServiceController(nameService))
            {
                if (sc.Status!=ServiceControllerStatus.Running) return;
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }

        private void GenerateLocalBitcoins()
        {
            Log.Instance.Info("Generate local bitcoins...");
            ExternalGenerationHashBlocks(1000);
        }

        protected void InitIdentity()
        {
            IdentityRepository.Initialize();
            IdentityRepository.CreateDefaultRoles();
            IdentityRepository.CreateDefaultAdministrator();
            IdentityRepository.CreateDefaultUser();
        }

        protected StringBuilder NameBuilder { get; set; }
        protected Random Random { get; set; }
        public IDalContext DalContext { get; set; }
        public ILogsContext LogsContext { get; set; }
        public ISaContext SaContext { get; set; }
        public IWrapper<ILog> Log { get; set; }
        public IIdentityRepository IdentityRepository { get; set; }
    }
}
