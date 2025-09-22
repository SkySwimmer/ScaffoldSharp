using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScaffoldSharp.Core.Objects
{
    /// <summary>
    /// Scaffold type pool system - part of class scanning
    /// </summary>
    public class TypePool : IDisposable
    {
        private List<Assembly> assemblies = new List<Assembly>();
        private List<Type> types = new List<Type>();
        private bool needsBuilding = true;

        /// <summary>
        /// Adds all loaded assemblies of the current domain to the pool
        /// </summary>
        public void AddAllLoadedAssemblies()
        {
            AddAllLoadedAssemblies(AppDomain.CurrentDomain);
        }

        /// <summary>
        /// Adds all loaded assemblies of a specific app domain to the pool
        /// </summary>
        /// <param name="domain">Domain to scan for assemblies</param>
        public void AddAllLoadedAssemblies(AppDomain domain)
        {
            AddAssemblies(domain.GetAssemblies());
        }

        /// <summary>
        /// Adds a list of assemblies to the type pool
        /// </summary>
        /// <param name="assemblies">Assemblies to add</param>
        public void AddAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly asm in assemblies)
            {
                if (asm != null)
                    AddAssembly(asm);
            }
        }

        /// <summary>
        /// Adds an assembly to the type pool
        /// </summary>
        /// <param name="assembly">Assembly to add</param>
        public void AddAssembly(Assembly assembly)
        {
            if (!assemblies.Contains(assembly))
            {
                assemblies.Add(assembly);
                needsBuilding = true;
            }
        }

        /// <summary>
        /// Builds the type pool, called by most operations
        /// </summary>
        public void BuildPool()
        {
            if (needsBuilding)
            {
                // Rebuild

                // Go through assemblies
                types.Clear();
                foreach (Assembly asm in assemblies)
                {
                    try
                    {
                        foreach (Type type in asm.GetTypes())
                        {
                            try
                            {
                                types.Add(type);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }

                // Done
                needsBuilding = false;
            }
        }

        /// <summary>
        /// Retrieves all pooled assemblies
        /// </summary>
        /// <returns>Array of Assembly instances</returns>
        public Assembly[] GetAssemblies()
        {
            return assemblies.ToArray();
        }

        /// <summary>
        /// Retrieves all loaded types
        /// </summary>
        /// <returns>Array of types</returns>
        public Type[] GetAllTypes()
        {
            BuildPool();
            return types.ToArray();
        }

        /// <summary>
        /// Retrieves all types extending a specific base type
        /// </summary>
        /// <param name="baseType">Base type</param>
        /// <param name="allowAbstractOrInterface">Toggles if abstracts are permitted</param>
        /// <returns>Array of types matching the base type</returns>
        public Type[] GetTypesExtending(Type baseType, bool allowAbstractOrInterface = false)
        {
            return GetAllTypes().Where(t => baseType.IsAssignableFrom(t) && !(!allowAbstractOrInterface && (t.IsInterface || t.IsAbstract))).ToArray();
        }

        /// <summary>
        /// Clears the type pool and cleans up
        /// </summary>
        public void Dispose()
        {
            assemblies.Clear();
            types.Clear();
            needsBuilding = true;
        }

        /// <summary>
        /// Called when garbage collected
        /// </summary>
        ~TypePool()
        {
            Dispose();
        }

    }
}