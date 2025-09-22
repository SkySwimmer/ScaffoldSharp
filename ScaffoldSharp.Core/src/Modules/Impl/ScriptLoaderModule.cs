using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ScaffoldSharp.Core.AsyncTasks;
using ScaffoldSharp.Logging;

namespace ScaffoldSharp.Core.Modules.Impl
{
    /// <summary>
    /// Script loader module for scaffold, automatically picked up at runtime initialization
    /// </summary>
    [ScaffoldFrameworkModule]
    public class ScriptLoaderModule : IScaffoldFrameworkModule
    {
        private object _lock = new object();
        private object _lockWaitStart = new object();

        private List<IScaffoldInitScript> initScripts = new List<IScaffoldInitScript>();
        private List<IScaffoldPostInitScript> postInitScripts = new List<IScaffoldPostInitScript>();
        private List<IScaffoldProgramScript> programScripts = new List<IScaffoldProgramScript>();

        private bool startedExec;
        private bool allScriptsStarted;
        private bool allScriptsFinished;
        private Dictionary<IScaffoldProgramScript, AsyncTask> programTasks = new Dictionary<IScaffoldProgramScript, AsyncTask>();

        /// <inheritdoc/>
        public void Load()
        {
            // Search for all scripts
            // FIXME
        }

        /// <inheritdoc/>
        public void Unload()
        {
            // Clean up
            initScripts.Clear();
            postInitScripts.Clear();
            programScripts.Clear();
            programTasks.Clear();
            startedExec = false;
            allScriptsStarted = false;
            allScriptsFinished = false;
        }

        /// <inheritdoc/>
        public void StartScripts()
        {
            // Call all init scripts
            foreach (IScaffoldInitScript script in initScripts)
            {
                // Run

                // Encase in trycatch
                try
                {
                    script.OnInit();
                }
                catch (Exception e)
                {
                    Logger.GetLogger("Initializer").Error("An exception occurred while running init script " + script.GetType().FullName + "!", e);
                    if (Debugger.IsAttached)
                        throw;
                }
            }
        }

        /// <inheritdoc/>
        public void StartProgram(string[] args)
        {
            // Call all program scripts
            int totalScripts = programScripts.Count;
            int finishedScripts = 0;
            int startedScripts = 0;
            foreach (IScaffoldProgramScript script in programScripts)
            {
                // Run task
                bool finished = false;
                AsyncTask task = AsyncTaskManager.RunAsync(() =>
                {
                    // Mark started
                    startedScripts++;

                    // Check
                    if (startedScripts >= totalScripts)
                    {
                        // All started

                        // Release
                        lock (_lockWaitStart)
                        {
                            allScriptsStarted = true;
                            Monitor.PulseAll(_lockWaitStart);
                        }
                    }
                    // Encase in trycatch
                    try
                    {
                        script.Run(args);
                    }
                    catch (Exception e)
                    {
                        Logger.GetLogger("Initializer").Error("An exception occurred while running init script " + script.GetType().FullName + "!", e);
                        if (Debugger.IsAttached)
                            throw;
                    }
                    finally
                    {
                        // Remove
                        lock (programTasks)
                        {
                            // Mark finished and remove
                            finished = true;
                            if (programTasks.ContainsKey(script))
                                programTasks.Remove(script);

                            // Increase counter
                            finishedScripts++;

                            // Check
                            if (finishedScripts >= totalScripts)
                            {
                                // All finished

                                // Release
                                lock (_lock)
                                {
                                    allScriptsFinished = true;
                                    Monitor.PulseAll(_lock);
                                }
                            }
                        }
                    }
                });

                // Add to list 
                lock (programTasks)
                {
                    if (!finished)
                        programTasks[script] = task;
                }
            }
            startedExec = true;
        }

        /// <inheritdoc/>
        public void PostStart()
        {
            // Call postinit
            foreach (IScaffoldPostInitScript script in postInitScripts)
            {
                // Run

                // Encase in trycatch
                try
                {
                    script.OnPostInit();
                }
                catch (Exception e)
                {
                    Logger.GetLogger("Initializer").Error("An exception occurred while running post-init script " + script.GetType().FullName + "!", e);
                    if (Debugger.IsAttached)
                        throw;
                }
            }

            // Clean up
            initScripts.Clear();
            postInitScripts.Clear();
            programScripts.Clear();
        }

        /// <summary>
        /// Retrieves all scaffold init scripts
        /// 
        /// <para>Note: after program initialization finishes, all scripts are cleared from memory</para>
        /// </summary>
        /// <returns>Array of IScaffoldInitScript instances</returns>
        public IScaffoldInitScript[] GetAllInitScripts()
        {
            return initScripts.ToArray();
        }

        /// <summary>
        /// Retrieves all scaffold post-init scripts
        /// 
        /// <para>Note: after program initialization finishes, all scripts are cleared from memory</para>
        /// </summary>
        /// <returns>Array of IScaffoldPostInitScript instances</returns>
        public IScaffoldPostInitScript[] GetAllPostInitScripts()
        {
            return postInitScripts.ToArray();
        }

        /// <summary>
        /// Retrieves all scaffold program scripts
        /// 
        /// <para>Note: after program initialization finishes, all scripts are cleared from memory</para>
        /// </summary>
        /// <returns>Array of IScaffoldProgramScript instances</returns>
        public IScaffoldProgramScript[] GetAllProgramScripts()
        {
            return programScripts.ToArray();
        }

        /// <summary>
        /// Checks if program execution has finished
        /// </summary>
        public bool FinishedProgramExecution
        {
            get
            {
                // Check if program execution started
                if (!startedExec)
                    return false;
                
                // Check finished
                if (allScriptsFinished)
                    return true;

                // Check tasks
                lock (programTasks)
                {
                    bool finished = true;

                    // Check
                    foreach (IScaffoldProgramScript script in programTasks.Keys)
                    {
                        if (!programTasks[script].HasCompleted)
                            finished = false;
                    }

                    // Return
                    return finished;
                }
            }
        }

        /// <summary>
        /// Holds the thread until all program scripts begin execution
        /// </summary>
        public void WaitForProgramStart()
        {
            if (allScriptsStarted)
                return;
            lock (_lockWaitStart)
            {
                // Check completed
                if (allScriptsStarted)
                    return;

                // Wait
                while (!allScriptsStarted)
                    Monitor.Wait(_lockWaitStart);
            }
        }

        /// <summary>
        /// Holds the thread until the program finishes running
        /// </summary>
        public void WaitForProgramFinish()
        {
            if (allScriptsFinished)
                return;
            lock (_lock)
            {
                // Check completed
                if (allScriptsFinished)
                    return;

                // Wait
                while (!allScriptsFinished)
                    Monitor.Wait(_lock);
            }
        }
    }
}