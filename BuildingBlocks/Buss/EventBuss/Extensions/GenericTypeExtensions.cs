using System;
using System.Linq;

namespace ShopDog.BuildingBlocks.Buss.EventBuss.Extensions
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            if (type.IsGenericType) return type.Name;

            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }

        public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
    }
}