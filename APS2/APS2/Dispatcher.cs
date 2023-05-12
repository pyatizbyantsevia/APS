using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
namespace APS2
{
    class Dispatcher
    {
        public void Dispatch(Task callerTask)
        {
            new Thread(delegate ()
            {
                Task taskToStartOrResume = OS.Scheduler.PopHighestPriorityTask();
                if (taskToStartOrResume != null)
                    taskToStartOrResume.StartOrResume();
            }).Start();
            if (callerTask != null)
                callerTask.Yield();
        }
    }
}

