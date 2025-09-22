namespace ScaffoldSharp.Core.Modules
{
    /// <summary>
    /// Base framework module interface
    /// </summary>
    public interface IScaffoldFrameworkModule
    {
        /// <summary>
        /// Called to load the module
        /// </summary>
        public void Load();

        /// <summary>
        /// Called to start user init scripts
        /// </summary>
        public void StartScripts();

        /// <summary>
        /// Called to start user programs
        /// </summary>
        /// <param name="args">Program arguments</param>
        public void StartProgram(string[] args);

        /// <summary>
        /// Called after everything has started up
        /// </summary>
        public void PostStart();

        /// <summary>
        /// Called to unload the module
        /// </summary>
        public void Unload();

    }
}