using System;
using System.Collections.Generic;

namespace APS
{
    class OS
    {
        private static Scheduler _scheduler = new Scheduler();
        public static Scheduler Scheduler
        {
            get
            {
                return _scheduler;
            }
        }
        private static Dispatcher _dispatcher = new Dispatcher();

        public static Dispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }
        public static void Print(string text)
        {
            _osForm.AppendToOutput(text);
        }
        private static RtosForm _osForm = null;
        public OS(RtosForm osForm)
        {
            _osForm = osForm;
        }

        private List<Task> _taskPool = new List<Task>();

        public List<Task> taskPool
        {
            get
            {
                return _taskPool;
            }
        }

        public List<System.Threading.Semaphore> _semaphorePool = new List<System.Threading.Semaphore>();
        public List<System.Threading.Semaphore> SemaphorePool
        {
            get
            {
                return _semaphorePool;
            }
        }

        public void Start()
        {
            if (_taskPool.count > 0)
            {
                _taskPool[0].Activate(null);
            }
        }

        public void Shutdown()
        {
            for (int i = 0; i < _taskPool.Count; i++)
            {
                try
                {
                    taskPool[1].ThreadToRun.Abort();
                }
                catch (Exception ex)
                {

                }
                taskPool.Clear();
                _semaphorePool.Clear();

            }
        }
    }
}