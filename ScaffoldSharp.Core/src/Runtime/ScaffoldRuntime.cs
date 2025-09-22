using System;
using System.Collections.Generic;
using ScaffoldSharp.Core.Modules;

namespace ScaffoldSharp.Core.Runtime
{
    /// <summary>
    /// Scaffold runtime controller
    /// </summary>
    public static class ScaffoldRuntime
    {
        private static Dictionary<string, IScaffoldFrameworkModule> modulesByType = new Dictionary<string, IScaffoldFrameworkModule>();
        private static List<IScaffoldFrameworkModule> frameworkModules = new List<IScaffoldFrameworkModule>();

        // FIXME: loading logic

        /// <summary>
        /// Retrieves all loaded framework modules
        /// </summary>
        /// <returns>Array of IScaffoldFrameworkModule instances</returns>
        public static IScaffoldFrameworkModule[] GetAllLoadedModules()
        {
            return frameworkModules.ToArray();
        }

        /// <summary>
        /// Retrieves loaded modules by type
        /// </summary>
        /// <typeparam name="T">Module type</typeparam>
        /// <returns>Module instance or null</returns>
        public static T GetLoadedModule<T>() where T : IScaffoldFrameworkModule
        {
            // Try fetch
            string type = typeof(T).FullName;
            lock (modulesByType)
            {
                // Check existing
                if (modulesByType.ContainsKey(type))
                    return (T)modulesByType[type];

                // Loop
                foreach (IScaffoldFrameworkModule mod in frameworkModules)
                {
                    // Check
                    if (mod is T)
                    {
                        // Success
                        modulesByType[type] = mod;
                        return (T)mod;
                    }
                }
            }

            // Fail
            return default(T);
        }
    }
}