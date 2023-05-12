using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS
{
    class Task
    {
        private Thread _threadToRun = null;

        public Thread ThreadToRun
        {
            get {
                return _threadToRun; 
            }
            set
            {
                _threadToRun = value;
            }
        }

        private ParameterizedThreadStart _taskToRun = null;

        public ParameterizedThreadStart TaskToRun
        {
            get 
            { 
                return _taskToRun;
            }
            set
            {
                _taskToRun = value;
            }
        }

        private string _name = null;

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private int _priority = 16;

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (value >= 0 && value <= 15)
                    _priority = value;
                else
                    throw new ArgumentOutOfRangeException("Priority must be in range from 0 to 15");
            }
        }

        public class TaskState
        {
            private int _taskState;
            private TaskState (int taskState)
            {
                _taskState = taskState;
            }

            public static bool operator== (TaskState taskState1, TaskState taskState2)
            {
                return taskState1._taskState == taskState2._taskState;
            }

            public static bool operator != (TaskState taskState1, TaskState taskState2)
            {
                return taskState1._taskState != taskState2._taskState;
            }

            public static TestState Inactive = new TaskState(0);
            public static TestState Ready = new TaskState(1);
            public static TestState Running = new TaskState(2);
            public static TestState Waiting = new TaskState(3);
        }

        private TaskState _taskState = TaskState.Inactive;
        public TastState CurrentTaskState
        {
            get { return _taskState; }  
            set { _taskState = value; }
        }

        public Task (ParameterizedThreadStart taskToRun, string name, int priority)
        {
            TaskToRun = taskToRun;
            Name = name;
            Priority = priority;
        }

        public void Activate(Task callerTask)
        {
            if (_taskState == TaskState.Inactive)
            {
                _threadToRun = new Thread(_taskToRun);

                OS.Scheduler.ScheduleTask(this);
            }
        }

        public static Task _currentlyRunningTask = null;
        public static Task CurrentlyRunningTask
        {
            get
            {
                return _currentlyRunningTask;
            }
            set { _currentlyRunningTask = value; }

        }

        public void StartOrResume()
        {
            if (_taskState == TaskState.Inactive)
            {
                _taskState= TaskState.Running;
                _currentlyRunningTask = this;
                _threadToRun.Start();
            }
            if (_taskState == TaskState.Ready)
            {
                _taskState= TaskState.Ready;
                _currentlyRunningTask = this;
                _threadToRun.Resume();
            }

        }

        public void Yield()
        {
            if (_taskState == TaskState.Running)
            {
                _taskState= TaskState.Ready;
                _currentlyRunningTask = null;
                OS.Scheduler.ScheduleTask(this);
                _threadToRun.Suspend();
            }
        }

        public void Terminate()
        {
            if (_taskState != TaskState.Inactive)
            {
                _taskState= TaskState.Inactive;
                _currentlyRunningTask = null;

                OS.Dispatcher.Dispatch(null);

                new Thread(delegate ()
                {
                    try
                    {
                        _threadToRun.Abort();
                    }
                    catch (Exception ex)
                    {

                    }
                }).Start();
            }
        }

    }
}
