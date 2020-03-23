using System;
using System.Threading.Tasks;
using AppGia.Helpers;
using Quartz;
using Quartz.Impl;

namespace AppGia.Jobs
{
    public class ExtraccionProcess
    {
        public static async void ExtraccionContableSchedule(String cronExp)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<ExtraccionContableJob>()
                .WithIdentity("ExtraccionContableJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("ExtraccionContableTrigger")
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
        }
        public static async void ExtraccionFlujoSchedule(String cronExp)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<ExtraccionFlujoJob>()
                .WithIdentity("ExtraccionFlujoJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("ExtraccionFlujoTrigger")
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
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