using System;

namespace ScaffoldSharp.Core.Modules
{
    /// <summary>
    /// Marks classes as framework modules, which will automatically load them up at runtime startup
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ScaffoldFrameworkModuleAttribute : Attribute
    {
    }
}