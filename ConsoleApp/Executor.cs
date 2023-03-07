namespace ConsoleApp
{
    using Business.Jobscheduler;

    public class Executor
    {
        private IJobSchedulerManager jobScheduler;
        public Executor(IJobSchedulerManager jobScheduler)
        {
            this.jobScheduler = jobScheduler;
        }

        public void Execute()
        {
            this.jobScheduler.ScheduleJobs();
        }
    }
}
