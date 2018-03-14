using System;
using System.Configuration;
using System.Timers;
using log4net;
using Libraries.Core.Backend.Common;
using Project.Kernel;

namespace Core.Bitcon.Service
{
    public interface ICoreBitconWinServiceInstance
    {
        void Start();
        void Stop();
        ICoreServiceRepository CoreServiceRepository { get; set; }
        IExecutor Executor { get; set; }
        IWrapper<ILog> Log { get; }
    }

    public class CoreBitconWinServiceInstance : ICoreBitconWinServiceInstance
    {
        public CoreBitconWinServiceInstance(ICoreServiceRepository coreServiceRepository, IExecutor excecutor, IWrapper<ILog> log)
        {
            CoreServiceRepository = coreServiceRepository;
            Executor = excecutor;
            Log = log;
        }

        public void Start()
        {
            var timerInterval = Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]);
            ServiceTimer = new Timer(timerInterval) {AutoReset = false};
            ServiceTimer.Elapsed += ServiceTimer_Elapsed;
            ServiceTimer.Start();
            Log.Instance.Info("Core.Bitcon.Service service is Started");
        }

        private void ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceTimer.Stop();
            Executor.ExecuteAction(CoreServiceRepository.DataProcessingApiRequest);
            ServiceTimer.Start();
        }

        public void Stop()
        {
            ServiceTimer.Stop();
            ServiceTimer.Elapsed-= ServiceTimer_Elapsed;
            Log.Instance.Info("Core.Bitcon.Service service is Stopped");
        }

        public ICoreServiceRepository CoreServiceRepository { get; set; }
        public IExecutor Executor { get; set; }
        public IWrapper<ILog> Log { get; }
        public Timer ServiceTimer { get; set; }
    }
}
