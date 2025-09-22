namespace ScaffoldSharp.Core
{
    /// <summary>
    /// Scaffold program script, called on program initialization
    /// </summary>
    public interface IScaffoldProgramScript
    {
        /// <summary>
        /// Called to run the script
        /// 
        /// <para>Note: program scripts are run asynchronously and keep the program running until all scripts finish</para>
        /// </summary>
        /// <param name="args">Program arguments</param>
        public void Run(string[] args);
    }
}