using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS2
{
    internal class Scheduler
    {
        private Queue<Task>[] _tasksQueues = new Queue<Task>[16];

        public Scheduler()
        {
            for (int i = 0; i < _tasksQueues.Length; i++)
            {
                _tasksQueues[i] = new Queue<Task>();
            }
        }

        public void ScheduleTask(Task task)
        {
            _tasksQueues[task.Priority].Enqueue(task);
        }

        public Task PopHighestPriorityTask()
        {
            for (int priority = 0; priority < _tasksQueues.Length; priority++)
            {
                if (_tasksQueues[priority].Count > 0)
                {
                    return _tasksQueues[priority].Dequeue();
                }
            }
            return null;
        }
    }
}
