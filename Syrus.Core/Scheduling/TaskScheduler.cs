using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Syrus.Core.Scheduling
{
    public class TaskScheduler
    {
        private List<Schedule> _schedules;
        public TaskScheduler() => _schedules = new List<Schedule>();

        public void AddSchedule(Schedule schedule) => _schedules.Add(schedule);

        public void AddSchedule(Func<Task> action, double interval) => AddSchedule(new Schedule(action, interval));

        public async void Run()
        {
            var tasks = _schedules.Where(schedule => schedule.CanExecute).Select(schedule => schedule.ToTask()).ToList();
            if(tasks.Count() > 0)
                await Task.WhenAll(tasks);
        }

        public void Run(double interval)
        {
            var timer = new System.Timers.Timer(interval);
            timer.Elapsed += (object sender, ElapsedEventArgs e) => Run();
            timer.Enabled = true;
        }
    }
}
