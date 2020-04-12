using System;
using System.Threading;
using System.Threading.Tasks;
using AppGia.Dao;
using AppGia.Helpers;
using AppGia.Models;
using Quartz;
using Quartz.Impl;

namespace AppGia.Jobs
{
    public class ExtraccionProcess
    {
        private static int timeOut = 3000;
        private static IScheduler _extraccionContableScheduler;
        private static IScheduler _extraccionFlujoScheduler;
        private static readonly string ClaveExtraccionContable = "EXTR_CONTABLE";
        private static readonly string ClaveExtraccionFlujo = "EXTR_FLUJO";

        public static string getEstatusContable()
        {
            return getEstatus(_extraccionContableScheduler);
        }
        public static string getEstatusFlujo()
        {
            return getEstatus(_extraccionFlujoScheduler);
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
            if (_extraccionContableScheduler != null && _extraccionContableScheduler.IsStarted)
            {
                finished= _extraccionContableScheduler.Shutdown().Wait(timeOut, default);
            }

            if (finished)
            {
                ExtraccionContableSchedule(cronExp, idUsuario);
            }
        }
        public static void rescheduleFlujo(string cronExp,Int64 idUsuario)
        {
            Boolean finished = false;
            if (_extraccionFlujoScheduler != null && _extraccionFlujoScheduler.IsStarted)
            {
                finished= _extraccionFlujoScheduler.Shutdown().Wait(timeOut, default);
            }

            if (finished)
            {
                ExtraccionFlujoSchedule(cronExp, idUsuario);
            }
        }

        public static  void ExtraccionContableSchedule()
        {
           ProgramacionProceso programacionProceso= new ProgramacionProcesoDataAccessLayer().GetByClave(ClaveExtraccionContable);
           if (programacionProceso != null)
           {
               ExtraccionContableSchedule(programacionProceso.cronExpresion, 0);
           }
        }
        private static async void ExtraccionContableSchedule(String cronExp,Int64 idUsuario)
        {
            new ProgramacionProcesoDataAccessLayer().manageProgramacionProceso(
                new ProgramacionProceso(ClaveExtraccionContable, null, cronExp, idUsuario));
            
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            _extraccionContableScheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<ExtraccionContableJob>()
                .WithIdentity("ExtraccionContableJob")
                .Build();

           ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("ExtraccionContableTrigger")
                .StartNow()
                .Build();
            await _extraccionContableScheduler.ScheduleJob(jobDetail, trigger);
            await _extraccionContableScheduler.Start();

        }

        public static  void ExtraccionFlujoSchedule()
        {
            ProgramacionProceso programacionProceso= new ProgramacionProcesoDataAccessLayer().GetByClave(ClaveExtraccionFlujo);
            if (programacionProceso != null)
            {
                ExtraccionFlujoSchedule(programacionProceso.cronExpresion, 0);
            }
        }
        private static async void ExtraccionFlujoSchedule(String cronExp,Int64 idUsuario)
        {
            new ProgramacionProcesoDataAccessLayer().manageProgramacionProceso(
                new ProgramacionProceso(ClaveExtraccionFlujo, null, cronExp, idUsuario));

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(); 
            _extraccionFlujoScheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<ExtraccionFlujoJob>()
                .WithIdentity("ExtraccionFlujoJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("ExtraccionFlujoTrigger")
                .StartNow()
                .Build();
            await _extraccionFlujoScheduler.ScheduleJob(jobDetail, trigger);
            await _extraccionFlujoScheduler.Start();
        }
    }

    internal class ExtraccionContableJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.Out.WriteLineAsync(".... ExtraccionContableJob start!");
                new ETLHelper().extraeBalanzaAuto();
                Console.Out.WriteLineAsync(".... ExtraccionContableJob end!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLineAsync("#### Error en ExtraccionContableJob: " + e.Message + ", " + e.StackTrace);
            }
        }
    }
    internal class ExtraccionFlujoJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.Out.WriteLineAsync(".... ExtraccionFlujoJob start!");
                new ETLHelper().extraeFlujoAuto();
                Console.Out.WriteLineAsync(".... ExtraccionFlujoJob end!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLineAsync("#### Error en ExtraccionFlujoJob: " + e.Message + ", " + e.StackTrace);
            }
        }
    }
}