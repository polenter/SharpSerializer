using System;
using System.Collections.ObjectModel;

namespace Polenter.Serialization.Serializing
{
    ///<summary>
    /// On the contrary to the full framework and Silverlight version,
    /// this Collection supports multithreading. It makes the item managing
    /// slower, but this is the price of lacking ThreadStaticAttribute in
    /// Portable Framework.
    ///</summary>
    public sealed class TypeInfoCollection : KeyedCollection<Type, TypeInfo>
    {
        private readonly object _locker = new object();

        /// <summary>
        /// </summary>
        /// <returns>null if the key was not found</returns>
        public TypeInfo TryGetTypeInfo(Type type)
        {
            lock (_locker)
            {
                if (!Contains(type))
                {
                    return null;
                }
                return this[type];
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="item"></param>
        public void AddIfNotExists(TypeInfo item)
        {
            if (item == null) throw new ArgumentNullException("item");
            lock (_locker)
            {
                if (Contains(item.Type))
                {
                    return;
                }
                Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ClearItems()
        {
            lock (_locker)
            {
                base.ClearItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, TypeInfo item)
        {
            lock (_locker)
            {
                base.InsertItem(index, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected override void RemoveItem(int index)
        {
            lock (_locker)
            {
                base.RemoveItem(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, TypeInfo item)
        {
            lock (_locker)
            {
                base.SetItem(index, item);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "item"></param>
        /// <returns></returns>
        protected override Type GetKeyForItem(TypeInfo item)
        {
            return item.Type;
        }
    }
}