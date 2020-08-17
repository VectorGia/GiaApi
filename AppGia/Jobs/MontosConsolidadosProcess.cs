using System;
using System.Threading.Tasks;
using AppGia.Dao;
using AppGia.Models;
using NLog;
using Quartz;
using Quartz.Impl;

namespace AppGia.Jobs
{
    public class MontosConsolidadosProcess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static int timeOut = 3000;
        private static IScheduler _schedulerMontosContable;
        private static IScheduler _schedulerMontosFlujo;
        private static readonly string ClaveMontosContable = "MONTOS_CONTABLE";
        private static readonly string ClaveMontosFlujo = "MONTOS_FLUJO";
        
        
        public static string getEstatusContable()
        {
            return getEstatus(_schedulerMontosContable);
        }
        public static string getEstatusFlujo()
        {
            return getEstatus(_schedulerMontosFlujo);
        }

        private static string getEstatus(IScheduler scheduler)
        {
            if (scheduler != null)
            {
                if (scheduler.IsStarted)
                {
                    return "GREEN";
                } 
                if (scheduler.InStandbyMode)
                {
                    return "YELLOW";
                }
            }
            return "RED";
        }
        
        public static void rescheduleContable(string cronExp,Int64 idUsuario)
        {
            Boolean finished = false;
            if (_schedulerMontosContable != null && _schedulerMontosContable.IsStarted)
            {
                finished= _schedulerMontosContable.Shutdown().Wait(timeOut, default);
            }
            logger.Info("rescheduleContable finished= '{0}'",finished);
            if (finished)
            {
                MontosContableSchedule(cronExp,idUsuario);
            }
        }
        public static void rescheduleFlujo(string cronExp,Int64 idUsuario)
        {
            Boolean finished = false;
            if (_schedulerMontosFlujo != null && _schedulerMontosFlujo.IsStarted)
            {
                finished= _schedulerMontosFlujo.Shutdown().Wait(timeOut, default);
            }
            logger.Info("rescheduleFlujo finished= '{0}'",finished);
            if (finished)
            {
                MontosFlujoSchedule(cronExp,idUsuario);
            }
        }

        public static  void MontosContableSchedule()
        {
            ProgramacionProceso programacionProceso= new ProgramacionProcesoDataAccessLayer().GetByClave(ClaveMontosContable);
            if (programacionProceso != null)
            {
                MontosContableSchedule(programacionProceso.cronExpresion, 0);
            }
        }
        private static async void MontosContableSchedule(String cronExp,Int64 idUsuario)
        {
            logger.Info("MontosContableSchedule ('{0}','{1}')",cronExp,idUsuario);
            new ProgramacionProcesoDataAccessLayer().manageProgramacionProceso(
                new ProgramacionProceso(ClaveMontosContable, null, cronExp, idUsuario));

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _schedulerMontosContable = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<MontosContableJob>()
                .WithIdentity("MontosContableJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("MontosContableTrigger")
                .StartNow()
                .Build();
            await _schedulerMontosContable.ScheduleJob(jobDetail, trigger);
            await _schedulerMontosContable.Start();
        }
        public static  void MontosFlujoSchedule()
        {
            ProgramacionProceso programacionProceso= new ProgramacionProcesoDataAccessLayer().GetByClave(ClaveMontosFlujo);
            if (programacionProceso != null)
            {
                MontosFlujoSchedule(programacionProceso.cronExpresion, 0);
            }
        }
        private static async void MontosFlujoSchedule(String cronExp,Int64 idUsuario)
        {
            logger.Info("MontosFlujoSchedule ('{0}','{1}')",cronExp,idUsuario);
            new ProgramacionProcesoDataAccessLayer().manageProgramacionProceso(
                new ProgramacionProceso(ClaveMontosFlujo, null, cronExp, idUsuario));

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _schedulerMontosFlujo = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<MontosFlujoJob>()
                .WithIdentity("MontosFlujoJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("MontosFlujoTrigger")
                .StartNow()
                .Build();
            await _schedulerMontosFlujo.ScheduleJob(jobDetail, trigger);
            await _schedulerMontosFlujo.Start();
        }
    }

    internal class MontosContableJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info(".... MontosContableJob start!");
                new PreProformaDataAccessLayer().MontosConsolidados(true,false);
                logger.Info(".... MontosContableJob end!");
            }
            catch (Exception e)
            {
                logger.Error(e,"#### Error en MontosContableJob " );
            }
        }
    }
    internal class MontosFlujoJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info(".... MontosFlujoJob start!");
                new PreProformaDataAccessLayer().MontosConsolidados(false,true);
                logger.Info(".... MontosFlujoJob end!");
            }
            catch (Exception e)
            {
                logger.Error(e,"#### Error en MontosFlujoJob " );
            }
        }
    }
}