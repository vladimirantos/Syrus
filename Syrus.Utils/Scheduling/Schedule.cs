using System;
using System.Threading.Tasks;

namespace Syrus.Shared.Scheduling
{
    public class Schedule
    {
        public Func<Task> ActionReference { get; private set; }
        public double Interval { get; private set; }
        public DateTime LastExecution { get; private set; }

        public bool CanExecute 
        {
            get {
                DateTime current = DateTime.Now;
                var diff = (current - LastExecution).TotalMilliseconds;
                return diff >= Interval;
            }
        }

        public Schedule(Func<Task> action, double interval) => (ActionReference, Interval, LastExecution) = (action, interval, DateTime.Now);

        public async void ExecuteAsync()
        {
            await ActionReference?.Invoke();
            LastExecution = DateTime.Now;
        }

        public Task ToTask() => Task.Run(ExecuteAsync);
    }
}
