using System;
using System.Threading;
using System.Threading.Tasks;
using AppGia.Helpers;
using Quartz;
using Quartz.Impl;

namespace AppGia.Jobs
{
    public class ExtraccionProcess
    {
        private static int timeOut = 3000;
        private static IScheduler _extraccionContableScheduler;
        private static IScheduler _extraccionFlujoScheduler;

        public static void rescheduleContable(string cronExp)
        {
            Boolean finished = false;
            if (_extraccionContableScheduler != null && _extraccionContableScheduler.IsStarted)
            {
                finished= _extraccionContableScheduler.Shutdown().Wait(timeOut, default);
            }

            if (finished)
            {
                ExtraccionContableSchedule(cronExp);
            }
        }
        public static void rescheduleFlujo(string cronExp)
        {
            Boolean finished = false;
            if (_extraccionFlujoScheduler != null && _extraccionFlujoScheduler.IsStarted)
            {
                finished= _extraccionFlujoScheduler.Shutdown().Wait(timeOut, default);
            }

            if (finished)
            {
                ExtraccionFlujoSchedule(cronExp);
            }
        }
        public static async void ExtraccionContableSchedule(String cronExp)
        {
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
        public static async void ExtraccionFlujoSchedule(String cronExp)
        {
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