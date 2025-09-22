using System;

namespace ScaffoldSharp.Core.Objects
{
    /// <summary>
    /// Scaffold type scanner system
    /// </summary>
    public class TypeScanner : IDisposable
    {
        /// <summary>
        /// Creates a type scanner with all assemblies preloaded onto the pool
        /// </summary>
        public TypeScanner() : this(new TypePool())
        {
            TypePool.AddAllLoadedAssemblies();
        }

        /// <summary>
        /// Creates a type scanner with a specified pool
        /// 
        /// <para>Note: does not add assemblies to the pool</para>
        /// </summary>
        /// <param name="pool">Type pool to use</param>
        public TypeScanner(TypePool pool)
        {
            // Set pool
            TypePool = pool;
        }

        /// <summary>
        /// Retrieves the type pool associated with this type scanner
        /// </summary>
        public TypePool TypePool { get; private set; }

        /// <summary>
        /// Retrieves the attribute scanner associated with this type scanner
        /// </summary>
        public AttributeScanner AttributeScanner { get; private set; } = new AttributeScanner();

        /// <summary>
        /// Retrieves all loaded types
        /// </summary>
        /// <returns>Array of types</returns>
        public Type[] FindAllTypes()
        {
            return TypePool.GetAllTypes();
        }

        /// <summary>
        /// Scans all types and collects types extending a specific base type
        /// </summary>
        /// <param name="baseType">Base type</param>
        /// <param name="allowAbstractOrInterface">Toggles if abstracts are permitted</param>
        /// <returns>Array of types matching the base type</returns>
        public Type[] FindClassesExtending(Type baseType, bool allowAbstractOrInterface = false)
        {
            return TypePool.GetTypesExtending(baseType, allowAbstractOrInterface);
        }

        /// <summary>
        /// Finds all classes annotated with a specific attribute
        /// </summary>
        /// <param name="attributeType">Attribute type</param>
        /// <param name="allowBaseTypeRecursion">Toggles if base types are to be scanned until a match is found</param>
        /// <param name="allowExtendingAttributes">Controls if attributes extending the given attribute type should be allowed or ignored</param>
        /// <param name="allowAbstractOrInterface">Toggles if abstracts are permitted</param>
        /// <returns></returns>
        public Type[] FindClassesAnnotatedWith(Type attributeType, bool allowBaseTypeRecursion = false, bool allowExtendingAttributes = false, bool allowAbstractOrInterface = false)
        {
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException("Value of attributeType does not extend Attribute!");

            // Call scanner
            // FIXME
            return null;
        }
        
        /// <summary>
        /// Clears the type pool and cleans up
        /// </summary>
        public void Dispose()
        {
            TypePool.Dispose();
            AttributeScanner.Dispose();
        }

        /// <summary>
        /// Called when garbage collected
        /// </summary>
        ~TypeScanner()
        {
            Dispose();
        }

    }
}