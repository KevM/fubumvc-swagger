using System;
using System.Diagnostics;
using System.Reflection;
using FubuCore.Reflection;

namespace FubuMVC.Swagger.Actions
{
    public static class SwaggerExtensions
    {
        public static string GetVersion(this Assembly assembly)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.ProductVersion;
        }

        public static string GetAttribute<T>(this PropertyInfo property, Func<T, string> func) where T : Attribute
        {
            var attribute = property.GetAttribute<T>();
            
            return attribute == null ? String.Empty : func(attribute);
        }

        public static string GetAttribute<T>(this Type type, Func<T, string> func) where T : Attribute
        {
            var attribute = type.GetAttribute<T>();

            return attribute == null ? String.Empty : func(attribute);
        }
    }
}