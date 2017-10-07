using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsAssignableFrom(this Type type, Type other)
        {
            if (other==null)
            {
                return false;
            }
            return type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsSubclassOf(this Type type, Type other)
        {
            return type.GetTypeInfo().IsSubclassOf(other);
        }

        public static MethodInfo GetMethod(this Type type, string propertyName)
        {
            return type.GetTypeInfo().GetDeclaredMethod(propertyName);
        }

        public static PropertyInfo GetProperty(this Type type, string propertyName)
        {
            return type.GetRuntimeProperty(propertyName);
        }

        public static PropertyInfo[] GetPublicInstanceProperties(this Type type)
        {
            var result = type.GetRuntimeProperties().ToArray();
            return result;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }

    }
}