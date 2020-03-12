using System;
using System.Threading.Tasks;
using AppGia.Controllers;
using Quartz;
using Quartz.Impl;

namespace AppGia.Jobs
{
    public class MontosConsolidadosProcess
    {
        public static async void MontosContableSchedule(String cronExp)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<MontosContableJob>()
                .WithIdentity("MontosContableJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("MontosContableTrigger")
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
        }
        public static async void MontosFlujoSchedule(String cronExp)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<MontosFlujoJob>()
                .WithIdentity("MontosFlujoJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExp)
                .WithIdentity("MontosFlujoTrigger")
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
        }
    }

    internal class MontosContableJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.Out.WriteLineAsync(".... MontosContableJob start!");
                new PreProformaDataAccessLayer().MontosConsolidados(true,false);
                Console.Out.WriteLineAsync(".... MontosContableJob end!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLineAsync("#### Error en MontosContableJob: " + e.Message + ", " + e.StackTrace);
            }
        }
    }
    internal class MontosFlujoJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.Out.WriteLineAsync(".... MontosFlujoJob start!");
                new PreProformaDataAccessLayer().MontosConsolidados(false,true);
                Console.Out.WriteLineAsync(".... MontosFlujoJob end!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLineAsync("#### Error en MontosFlujoJob: " + e.Message + ", " + e.StackTrace);
            }
        }
    }
}