namespace ScaffoldSharp.Core
{
    /// <summary>
    /// Scaffold post-init script, called when the program is fully started up
    /// </summary>
    public interface IScaffoldPostInitScript
    {
        /// <summary>
        /// Called to run the script
        /// 
        /// <para>Note: avoid logic that can cause blocking, otherwise the loading process wont be able to continue</para>
        /// </summary>
        public void OnPostInit();
    }
}