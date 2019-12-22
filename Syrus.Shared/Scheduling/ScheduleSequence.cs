using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Syrus.Shared.Scheduling
{
    /// <summary>
    /// Umožňuje vytvářet navazující úlohy, kdy v každém cyklu scheduleru se provede právě jedna úloha z chainu.
    /// </summary>
    public class ScheduleSequence : IScheduleSequence
    {
        private List<Schedule> _schedules;
        private int _index = 0;
        public ScheduleSequence() => _schedules = new List<Schedule>();

        public IScheduleSequence AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
            return this;
        }

        public IScheduleSequence AddSchedule(Func<Task> action, double interval) => AddSchedule(new Schedule(action, interval));

        public Schedule Next()
        {
            Schedule current = _schedules[_index++];
            if (_index >= _schedules.Count)
                _index = 0;
            return current;
        }
    }
}
