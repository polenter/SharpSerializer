using System;
using System.Collections.Generic;
using System.Reflection;

namespace Polenter.Serialization.Advanced
{
    ///<summary>
    /// Supports multithreading. It's slower but neccessary, 
    /// because Portable Framework does not support ThreadStaticAttribute.
    ///</summary>
    internal class PropertyCache
    {
        private readonly Dictionary<Type, IList<PropertyInfo>> _cache = new Dictionary<Type, IList<PropertyInfo>>();

        /// <summary>
        /// </summary>
        /// <returns>null if the key was not found</returns>
        public IList<PropertyInfo> TryGetPropertyInfos(Type type)
        {
            lock (_cache)
            {
                if (!_cache.ContainsKey(type))
                {
                    return null;
                }
                return _cache[type];
            }
        }

        /// <summary>
        /// If the key exists, the value will not be added.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(Type key, IList<PropertyInfo> value)
        {
            lock (_cache)
            {
                if (_cache.ContainsKey(key))
                {
                    return;
                }
                _cache.Add(key, value);
            }
        }
    }
}