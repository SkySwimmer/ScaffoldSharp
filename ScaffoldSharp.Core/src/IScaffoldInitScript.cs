namespace ScaffoldSharp.Core
{
    /// <summary>
    /// Scaffold init script, called on scaffold initialization
    /// </summary>
    public interface IScaffoldInitScript
    {
        /// <summary>
        /// Called to run the script
        /// 
        /// <para>Note: avoid logic that can cause blocking, otherwise the loading process wont be able to continue</para>
        /// </summary>
        public void OnInit();
    }
}