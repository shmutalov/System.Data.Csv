#region Copyright

/*
	Copyright (c) Sherzod Mutalov, 2015
	mailto:shmutalov@gmail.com
*/

#endregion

using System.Reflection;

namespace System.Data.Csv.Extensions
{
    /// <summary>
    /// Reflection extension methods
    /// </summary>
    internal static class ReflectionExt
    {
        private const BindingFlags BindFlags = 
            BindingFlags.Instance | 
            BindingFlags.Public | 
            BindingFlags.NonPublic | 
            BindingFlags.Static;

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetInstanceFieldValue(this object instance, string fieldName)
        {
            var instanceType = instance.GetType();
            var field = instanceType.GetField(fieldName, BindFlags);

            return field?.GetValue(instance);
        }

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="instanceType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetInstanceFieldValue(this object instance, Type instanceType, string fieldName)
        {
            var field = instanceType.GetField(fieldName, BindFlags);

            return field?.GetValue(instance);
        }

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetInstanceFieldValue(this object instance, string fieldName, object value)
        {
            var instanceType = instance.GetType();
            var field = instanceType.GetField(fieldName, BindFlags);

            field?.SetValue(instance, value);
        }

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="instanceType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static void SetInstanceFieldValue(this object instance, Type instanceType, string fieldName, object value)
        {
            var field = instanceType.GetField(fieldName, BindFlags);

            field?.SetValue(instance, value);
        }

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetInstancePropertyValue(this object instance, string propertyName)
        {
            var instanceType = instance.GetType();
            var field = instanceType.GetProperty(propertyName, BindFlags);

            return field?.GetValue(instance);
        }

        /// <summary>
        /// Get instance field object by its name
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="instanceType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetInstancePropertyValue(this object instance, Type instanceType, string propertyName)
        {
            var field = instanceType.GetProperty(propertyName, BindFlags);

            return field?.GetValue(instance);
        }
    }
}
