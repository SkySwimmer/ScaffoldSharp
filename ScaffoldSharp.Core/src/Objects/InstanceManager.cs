using System;

namespace ScaffoldSharp.Core.Objects
{
    /// <summary>
    /// Scaffold object instance manager
    /// </summary>
    public class InstanceManager : IDisposable
    {
        private static InstanceManager _default;

        /// <summary>
        /// Retrieves the default instance manager
        /// </summary>
        public static InstanceManager Default
        {
            get
            {
                if (_default == null)
                    _default = new InstanceManager();
                return _default;
            }
        }

        /// <summary>
        /// Creates an instance manager with all assemblies preloaded onto the pool
        /// </summary>
        public InstanceManager() : this(new TypePool())
        {
            TypePool.AddAllLoadedAssemblies();
        }

        /// <summary>
        /// Creates an instance manager with a specified pool
        /// 
        /// <para>Note: does not add assemblies to the pool</para>
        /// </summary>
        /// <param name="pool">Type pool to use</param>
        public InstanceManager(TypePool pool)
        {
            // Set pool
            TypePool = pool;
            TypeScanner = new TypeScanner(pool);
        }

        /// <summary>
        /// Retrieves the type pool associated with this instance manager
        /// </summary>
        public TypePool TypePool { get; private set; }

        /// <summary>
        /// Retrieves the type scanner associated with this instance manager
        /// </summary>
        public TypeScanner TypeScanner { get; private set; }

        /// <summary>
        /// Clears the type pool and cleans up
        /// </summary>
        public void Dispose()
        {
            TypePool.Dispose();
            TypeScanner.Dispose();
        }

        /// <summary>
        /// Called when garbage collected
        /// </summary>
        ~InstanceManager()
        {
            Dispose();
        }

    }
}