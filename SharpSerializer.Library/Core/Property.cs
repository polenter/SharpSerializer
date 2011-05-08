#region Copyright © 2010 Pawel Idzikowski [idzikowski@sharpserializer.com]

//  ***********************************************************************
//  Project: sharpSerializer
//  Web: http://www.sharpserializer.com
//  
//  This software is provided 'as-is', without any express or implied warranty.
//  In no event will the author(s) be held liable for any damages arising from
//  the use of this software.
//  
//  Permission is granted to anyone to use this software for any purpose,
//  including commercial applications, and to alter it and redistribute it
//  freely, subject to the following restrictions:
//  
//      1. The origin of this software must not be misrepresented; you must not
//        claim that you wrote the original software. If you use this software
//        in a product, an acknowledgment in the product documentation would be
//        appreciated but is not required.
//  
//      2. Altered source versions must be plainly marked as such, and must not
//        be misrepresented as being the original software.
//  
//      3. This notice may not be removed or altered from any source distribution.
//  
//  ***********************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Base class for all properties. Every object can be defined with inheritors of the Property class.
    /// </summary>
    public abstract class Property
    {
        /// <summary>
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "type"></param>
        protected Property(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        ///   Not all properties have name (i.e. items of a collection)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Of what type is the property or its value
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///   If the properties are nested, i.e. collection items are nested in the collection
        /// </summary>
        public Property Parent { get; set; }
    }

    /// <summary>
    ///   Represents the null value. Null values are serialized too.
    /// </summary>
    public sealed class NullProperty : Property
    {
        ///<summary>
        ///</summary>
        public NullProperty() : base(null, null)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        public NullProperty(string name)
            : base(name, null)
        {
        }
    }

    /// <summary>
    ///   It represents some properties of an object, or some items of a collection/dictionary/array
    /// </summary>
    public sealed class PropertyCollection : Collection<Property>
    {
        ///<summary>
        ///  Parent property
        ///</summary>
        public Property Parent { get; set; }

        /// <summary>
        /// </summary>
        protected override void ClearItems()
        {
            foreach (Property item in Items)
            {
                item.Parent = null;
            }
            base.ClearItems();
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        /// <param name = "item"></param>
        protected override void InsertItem(int index, Property item)
        {
            base.InsertItem(index, item);
            item.Parent = Parent;
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        protected override void RemoveItem(int index)
        {
            Items[index].Parent = null;
            base.RemoveItem(index);
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        /// <param name = "item"></param>
        protected override void SetItem(int index, Property item)
        {
            Items[index].Parent = null;
            base.SetItem(index, item);
            item.Parent = Parent;
        }
    }


    /// <summary>
    ///   Represents all primitive types (i.e. int, double...) and additionally
    ///   DateTime, TimeSpan, Decimal und enumerations
    ///   Contains no nested properties
    /// </summary>
    /// <remarks>
    ///   See SimpleValueConverter for a list of supported types.
    /// </remarks>
    public sealed class SimpleProperty : Property
    {
        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public SimpleProperty(string name, Type type)
            : base(name, type)
        {
        }

        ///<summary>
        ///  It could only one of the simple types, see Tools.IsSimple(...)
        ///</summary>
        public object Value { get; set; }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            string text = base.ToString();
            return Value != null
                       ? string.Format("{0}, ({1})", text, Value)
                       : string.Format("{0}, (null)", text);
        }
    }

    /// <summary>
    ///   Represents complex type which contains properties.
    /// </summary>
    public class ComplexProperty : Property
    {
        private PropertyCollection _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexProperty"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public ComplexProperty(string name, Type type, object value)
            : base(name, type)
        {
            this.Value = value;
            this.ComplexReferenceId = 0; // assume it is not recursive (yet)
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexProperty"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public ComplexProperty(string name, System.Type type)
            : this(name, type, null)
        {
        }

        ///<summary>
        ///  The actual complex object.
        ///  On Serialisation used to find out if object was serialized before
        ///</summary>
        public object Value { get; set; }

        /// <summary>
        /// [Get or Set] If not 0, this is an Item that is used more than once. 
        /// </summary>
        /// <value>The recursion id.</value>
        public int ComplexReferenceId { get; set; }

        /// <summary>
        /// [Gets] True if this property is referenced more than once.
        /// </summary>
        public bool IsReferencedMoreThanOnce
        {
            get
            {
                return ComplexReferenceId != 0;
            }
        }

        ///<summary>
        /// Sub properties
        ///</summary>
        public PropertyCollection Properties
        {
            get
            {
                if (_properties == null) _properties = new PropertyCollection {Parent = this};
                return _properties;
            }
            set { _properties = value; }
        }
    }

    /// <summary>
    ///   Represents complex type via a reference of an other complex type.
    /// </summary>
    public class ComplexReferenceProperty : Property
    {
        /// <summary>
        /// [Get, Set} where this is pointing to.
        /// </summary>
        public ComplexProperty ReferenceTarget  { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexReferenceProperty"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="referenceTarget">The reference target.</param>
        public ComplexReferenceProperty(string name, ComplexProperty referenceTarget)
            : base(name, null)
        {
            this.ReferenceTarget = referenceTarget;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexReferenceProperty"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ComplexReferenceProperty(string name)
            : this(name, null)
        {
        }
    }

    /// <summary>
    ///   Represents type which is ICollection
    /// </summary>
    public sealed class CollectionProperty : ComplexProperty
    {
        private IList<Property> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public CollectionProperty(string name, Type type)
            : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<Property> Items
        {
            get
            {
                if (_items == null) _items = new List<Property>();
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        ///   Of what type are items. It's important for polymorphic collection
        /// </summary>
        public Type ElementType { get; set; }
    }


    /// <summary>
    ///   Represents dictionary. Every item is composed of the key and value
    /// </summary>
    public sealed class DictionaryProperty : ComplexProperty
    {
        private IList<KeyValueItem> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public DictionaryProperty(string name, Type type)
            : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<KeyValueItem> Items
        {
            get
            {
                if (_items == null) _items = new List<KeyValueItem>();
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        ///   Of what type are keys
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        ///   Of what type are values
        /// </summary>
        public Type ValueType { get; set; }
    }

    /// <summary>
    ///   Represents one dimensional array
    /// </summary>
    public sealed class SingleDimensionalArrayProperty : Property
    {
        private PropertyCollection _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public SingleDimensionalArrayProperty(string name, Type type)
            : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public PropertyCollection Items
        {
            get
            {
                if (_items == null) _items = new PropertyCollection {Parent = this};
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        ///   As default is 0, but there can be higher start index
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        ///   Of what type are elements
        /// </summary>
        public Type ElementType { get; set; }
    }

    /// <summary>
    ///   Represents multidimensional array. Array properties are in DimensionInfos
    /// </summary>
    public sealed class MultiDimensionalArrayProperty : Property
    {
        private IList<DimensionInfo> _dimensionInfos;
        private IList<MultiDimensionalArrayItem> _items;

        ///<summary>
        ///</summary>
        ///<param name = "name"></param>
        ///<param name = "type"></param>
        public MultiDimensionalArrayProperty(string name, Type type)
            : base(name, type)
        {
        }

        ///<summary>
        ///</summary>
        public IList<MultiDimensionalArrayItem> Items
        {
            get
            {
                if (_items == null) _items = new List<MultiDimensionalArrayItem>();
                return _items;
            }
            set { _items = value; }
        }

        /// <summary>
        ///   Information about the array
        /// </summary>
        public IList<DimensionInfo> DimensionInfos
        {
            get
            {
                if (_dimensionInfos == null) _dimensionInfos = new List<DimensionInfo>();
                return _dimensionInfos;
            }
            set { _dimensionInfos = value; }
        }

        /// <summary>
        ///   Of what type are elements. All elements in all all dimensions must be inheritors of this type.
        /// </summary>
        public Type ElementType { get; set; }
    }

    /// <summary>
    ///   Information about one item in a multidimensional array
    /// </summary>
    public sealed class MultiDimensionalArrayItem
    {
        ///<summary>
        ///</summary>
        ///<param name = "indexes"></param>
        ///<param name = "value"></param>
        public MultiDimensionalArrayItem(int[] indexes, Property value)
        {
            Indexes = indexes;
            Value = value;
        }

        /// <summary>
        ///   Represents item coordinates in the array (i.e. [1,5,3] - item has index 1 in the dimension 0, index 5 in the dimension 1 and index 3 in the dimension 2).
        /// </summary>
        public int[] Indexes { get; set; }

        /// <summary>
        ///   Item value. It can contain any type.
        /// </summary>
        public Property Value { get; set; }
    }

    /// <summary>
    ///   Every array is composed of dimensions. Singledimensional arrays have only one info,
    ///   multidimensional have more dimension infos.
    /// </summary>
    public sealed class DimensionInfo
    {
        /// <summary>
        ///   Start index for the array
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        ///   How many items are in this dimension
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    ///   Represents one item from the dictionary, a key-value pair.
    /// </summary>
    public sealed class KeyValueItem
    {
        ///<summary>
        ///</summary>
        ///<param name = "key"></param>
        ///<param name = "value"></param>
        public KeyValueItem(Property key, Property value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        ///   Represents key. There can be everything
        /// </summary>
        public Property Key { get; set; }

        /// <summary>
        ///   Represents value. There can be everything
        /// </summary>
        public Property Value { get; set; }
    }
}