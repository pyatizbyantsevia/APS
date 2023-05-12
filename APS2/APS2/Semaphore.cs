using APS2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS2
{
    class Semaphore
    {
        private Queue<Task>[] _waitingTasksQueues = new Queue<Task>[16];

        private int _count = 0;
        private int _maxCount = 0;

        public Semaphore(int initialCount, int maxCount)
        {
            if (initialCount > maxCount)
                throw new ArgumentOutOfRangeException("initialCount must be less maxCount");
            _count = initialCount;
            _maxCount = maxCount;

            for (int i = 0; i < _waitingTasksQueues.Length; i++)
            {
                _waitingTasksQueues[i] = new Queue<Task>();
            }
        }

        public bool Wait(Task callerTask)
        {
            if (_count < _maxCount)
            {
                if (callerTask.CurrentTaskState == Task.TaskState.Running)
                {
                    callerTask.CurrentTaskState = Task.TaskState.Waiting;
                    Task.CurrentlyRunningTask = null;

                    _waitingTasksQueues[callerTask.Priority].Enqueue(callerTask);
                    _count++;

                    OS.Dispatcher.Dispatch(null);
                    callerTask.ThreadToRun.Suspend();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Signal()
        {
            Task taskToSchedule = null;
            for (int priority = 0; priority < _waitingTasksQueues.Length; priority++)
            {
                if (_waitingTasksQueues[priority].Count > 0)
                {
                    taskToSchedule = _waitingTasksQueues[priority].Dequeue();
                }
            }

            if (taskToSchedule != null)
            {
                taskToSchedule.CurrentTaskState = Task.TaskState.Ready;
                OS.Scheduler.ScheduleTask(taskToSchedule);

                if (_count >= 0)
                {
                    _count--;
                    return true;
                }
            }
            return false;
        }
    }
}
