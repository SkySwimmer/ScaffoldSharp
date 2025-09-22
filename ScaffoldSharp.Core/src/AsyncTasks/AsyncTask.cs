using System;
using System.Threading;

namespace ScaffoldSharp.Core.AsyncTasks
{
    /// <summary>
    /// Async Task Container
    /// </summary>
    public class AsyncTask
    {
        private Action _action;
        private bool _started;
        private bool _completed;
        private object _lock = new object();

        /// <summary>
        /// Creates a task container
        /// </summary>
        /// <param name="action">Action assigned to the container</param>
        public AsyncTask(Action action)
        {
            _action = action;
        }

        internal void Run()
        {
            try
            {
                _started = true;
                _action();
            }
            finally
            {
                // Release
                lock (_lock)
                {
                    _completed = true;
                    Monitor.PulseAll(_lock);
                }
            }
        }

        /// <summary>
        /// Checks if the task has been started
        /// </summary>
        public bool HasStarted
        {
            get
            {
                return _started;
            }
        }

        /// <summary>
        /// Checks if the task has been completed
        /// </summary>
        public bool HasCompleted
        {
            get
            {
                return _completed;
            }
        }

        /// <summary>
        /// Waits for the task to finish running
        /// </summary>
        public void Await()
        {
            if (_completed)
                return;
            lock (_lock)
            {
                // Check completed
                if (_completed)
                    return;

                // Wait
                while (!_completed)
                    Monitor.Wait(_lock);
            }
        }

    }
}