using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Godot;
using StarDefense.Attrubutes;

namespace StarDefense.Helpers
{
    public static class TypeHelper
    {
        public static IEnumerable<T> WithAttributeDefined<T>(Type attribute) =>
            Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => Attribute.IsDefined(x, attribute))
            .Select(x => (T)Activator.CreateInstance(x));

        public static IEnumerable<T> WithResourceAttributeDefined<TResource, T>(IResourceDescriptor<T> descriptor)
            where T : Node
            where TResource : ResourceAttribute
            => WithResourceAttributeDefinedNames<TResource>().Select(x => descriptor.Create(x));

        public static IEnumerable<string> WithResourceAttributeDefinedNames<T>()
            where T : ResourceAttribute
            => WithResourceAttributeDefinedAttributes<T>().Select(x => x.Name);

        public static IEnumerable<T> WithResourceAttributeDefinedAttributes<T>()
            where T : ResourceAttribute
            => Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => Attribute.IsDefined(x, typeof(T)))
            .Select(x => x.GetCustomAttribute<T>());

        public static void WithResourceAttributeDefinedNames<T>(Action<T> onAttribute)
            where T : ResourceAttribute
        {
            foreach (var resource in Assembly.GetExecutingAssembly().GetTypes().Where(x => Attribute.IsDefined(x, typeof(T))))
            {
                var attribute = resource.GetCustomAttribute<T>();
                onAttribute(attribute);
            }
        }

        public static IEnumerable<string> WithResourceAttributeDefinedNames<T>(ResourceAttribute attribute)
            where T : ResourceAttribute
            => Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => Attribute.IsDefined(x, attribute.GetType()))
            .Select(x => ((ResourceAttribute)x.GetCustomAttribute(attribute.GetType())).Name);
    }
}
