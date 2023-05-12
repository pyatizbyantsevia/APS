using APS2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APS2
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

        public List<Task> TaskPool
        {
            get
            {
                return _taskPool;
            }
        }

        public List<Semaphore> _semaphorePool = new List<Semaphore>();
        public List<Semaphore> SemaphorePool
        {
            get
            {
                return _semaphorePool;
            }
        }

        public void Start()
        {
            if (_taskPool.Count > 0)
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
                    TaskPool[1].ThreadToRun.Abort();
                }
                catch (Exception ex)
                {

                }
                TaskPool.Clear();
                _semaphorePool.Clear();

            }
        }
    }
}