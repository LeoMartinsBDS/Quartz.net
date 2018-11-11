using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.Threading;

namespace Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler();

            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("name", "group")
                .UsingJobData("Name", "Bob")
                .UsingJobData("Count", 1)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(2).WithRepeatCount(5))
                .StartNow()
                .Build();
            //USING SCHEDULEBUILDER INTERVAL  

            ITrigger trigger1 = TriggerBuilder.Create()
               .WithCronSchedule("0 44 12 ? * SUN-FRI")
               .StartNow()
               .Build();
            //USING CRON


            scheduler.ScheduleJob(job, trigger1);
            scheduler.Start();

            Thread.Sleep(TimeSpan.FromMinutes(10));
            scheduler.Shutdown();
        }
    }
    
    public class HelloJob : IJob
    {
        //public string Name { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            var Name = context.JobDetail.JobDataMap.GetString("Name");
            var Count = context.JobDetail.JobDataMap.GetInt("Count");

            context.MergedJobDataMap.Put("Count", ++Count);

            Console.WriteLine("Job Started at " + DateTime.Now);
            Console.WriteLine("Hello " + Name);
            Thread.Sleep(TimeSpan.FromSeconds(4));
            Console.WriteLine("Job Finished at " + DateTime.Now);
        }
    }
}
