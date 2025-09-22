using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScaffoldSharp.Core.Objects
{
    /// <summary>
    /// Attribute scanner system - used to check attributes of types, fields, methods and properties
    /// </summary>
    public class AttributeScanner : IDisposable
    {
        private Dictionary<string, TypeAttributeInformation> typeCache = new Dictionary<string, TypeAttributeInformation>();

        // FIXME: port to caching of attribute information instead of direct fetching (make sure to deal with generic types properly with non-primitive values too)

        private class TypeAttributeInformation : AttributeInformation
        {
            public Type type;
            public TypeAttributeInformation baseType;

            public Dictionary<MethodInfo, MethodAttributeInformation> methods;
            public Dictionary<MethodInfo, FieldAttributeInformation> fields;
            public Dictionary<MethodInfo, PropertyAttributeInformation> properties;
        }

        private class MethodAttributeInformation : AttributeInformation
        {
            public MethodInfo method;
        }

        private class FieldAttributeInformation : AttributeInformation
        {
            public FieldInfo field;
        }

        private class PropertyAttributeInformation : AttributeInformation
        {
            public PropertyInfo field;
        }

        private class AttributeInformation
        {
            public Attribute[] declaredAttributes;
            public Attribute[] allAttributes;

            // FIXME: cache by type?
        }

        /// <summary>
        /// Retrieves a type's attributes
        /// </summary>
        /// <param name="type">Type to retrieve the attributes for</param>
        /// <param name="allowAttributeInheritance">Controls if attributes of base types should be collected</param>
        /// <returns>Array of Attribute instances</returns>
        public Attribute[] GetAttributesFor(Type type, bool allowAttributeInheritance = false)
        {
            return GetAttributesOfTypeFor<Attribute>(type, allowAttributeInheritance, true);
        }

        /// <summary>
        /// Retrieves all of a type's attributes that match the given attribute tpye
        /// </summary>
        /// <typeparam name="T">Attribute type to search for</typeparam>
        /// <param name="type">Type to retrieve the attributes for</param>
        /// <param name="allowAttributeInheritance">Controls if attributes of base types should be collected</param>
        /// <param name="allowExtendingAttributes">Controls if attributes extending the given attribute type should be allowed or ignored</param>
        /// <returns>Array of Attribute instances</returns>
        public T[] GetAttributesOfTypeFor<T>(Type type, bool allowAttributeInheritance = false, bool allowExtendingAttributes = false) where T : Attribute
        {
            return GetAttributesOfTypeFor(type, typeof(T), allowAttributeInheritance, allowExtendingAttributes).Select(t => (T)t).ToArray();
        }

        /// <summary>
        /// Retrieves all of a type's attributes that match the given attribute tpye
        /// </summary>
        /// <param name="type">Type to retrieve the attributes for</param>
        /// <param name="attributeType">Attribute type to search for</param>
        /// <param name="allowAttributeInheritance">Controls if attributes of base types should be collected</param>
        /// <param name="allowExtendingAttributes">Controls if attributes extending the given attribute type should be allowed or ignored</param>
        /// <returns>Array of Attribute instances</returns>
        public Attribute[] GetAttributesOfTypeFor(Type type, Type attributeType, bool allowAttributeInheritance = false, bool allowExtendingAttributes = false)
        {
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException("Value of attributeType does not extend Attribute!");

            // Go through types
            List<Attribute> attrs = new List<Attribute>();
            Type p = type;
            while (p != null)
            {
                // Get for type
                foreach (object attr in p.GetCustomAttributes(false))
                {
                    // Check type
                    if ((allowExtendingAttributes && attributeType.IsAssignableFrom(attr.GetType())) || (!allowExtendingAttributes && attr.GetType().FullName == attributeType.FullName))
                    {
                        // Add
                        attrs.Add((Attribute)attr);
                    }
                }

                // Recurse
                if (!allowAttributeInheritance)
                    break;
                p = p.BaseType;
            }
            return attrs.ToArray();
        }

        /// <summary>
        /// Clears the pool and cleans up
        /// </summary>
        public void Dispose()
        {
            typeCache.Clear();
        }

        /// <summary>
        /// Called when garbage collected
        /// </summary>
        ~AttributeScanner()
        {
            Dispose();
        }
    }
}