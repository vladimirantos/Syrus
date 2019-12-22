using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Syrus.Shared.Scheduling
{
    public interface IScheduler
    {
        IScheduler AddSchedule(Schedule schedule);
        IScheduler AddSchedule(Func<Task> action, double interval);
        IScheduleSequence UseSequence();
    }

    public interface IScheduleSequence
    {
        IScheduleSequence AddSchedule(Schedule schedule);
        IScheduleSequence AddSchedule(Func<Task> action, double interval);
        Schedule Next();
    }

    public interface IScheduleExecution
    {
        void Run();

        void Run(double interval);
    }

    public class Scheduler : IScheduler, IScheduleExecution
    {
        private List<Schedule> _schedules;
        private List<IScheduleSequence> _sequences;

        public Scheduler()
        {
            _schedules = new List<Schedule>();
            _sequences = new List<IScheduleSequence>();
        }

        public IScheduler AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
            return this;
        }

        public IScheduler AddSchedule(Func<Task> action, double interval) => AddSchedule(new Schedule(action, interval));

        public IScheduleSequence UseSequence()
        {
            IScheduleSequence sequence = new ScheduleSequence();
            _sequences.Add(sequence);
            return sequence;
        }

        public async void Run()
        {
            var tasks = _schedules.Where(schedule => schedule.CanExecute).Select(schedule => schedule.ToTask()).ToList();
            var sequences = _sequences.Select(sequence => sequence.Next().ToTask()).ToList();
            if (tasks.Count() > 0)
                await Task.WhenAll(tasks);
        }

        public void Run(double interval)
        {
            var timer = new Timer(interval);
            timer.Elapsed += (object sender, ElapsedEventArgs e) => Run();
            timer.Enabled = true;
        }
    }
}

