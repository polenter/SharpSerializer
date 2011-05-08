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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Polenter.Serialization.Advanced;
using Polenter.Serialization.Core;

namespace Polenter.Serialization.Serializing
{
    /// <summary>
    ///   Decomposes object to a Property and its Sub-Properties as part of the format independent serialisatoin.
    ///   Recursive traverse of the object until the complete Property with allsub- and parent-properties is loaded.
    ///   If a <see cref="ComplexProperty"/> is recursivly pointing to some other object already processed
    ///   its RecursionId is set to a non-0 value to indicate possible
    ///   endless-Recursion.
    /// </summary>
    public sealed class PropertyFactory
    {
        #region local memory
        /// <summary>
        /// needed to invoke parameterless functions via reflection
        /// </summary>
        private readonly object[] _emptyObjectArray = new object[0];

        /// <summary>
        /// Collects subdata from source object
        /// </summary>
        private readonly PropertyProvider _propertyProvider;

        /// <summary>
        /// All complex item already processed. Used to detect if a source object is referenced more than once.
        /// </summary>
        private IDictionary<object, ComplexProperty> nonDuplicateValues = new Dictionary<object, ComplexProperty>();

        /// <summary>
        /// Sequencegenerator: Every complex item that is used more than once gets an id out of this sequence.
        /// </summary>
        private int nextReferenceId = 1;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyFactory"/> class.
        /// </summary>
        /// <param name="propertyProvider">provides all important properties of the decomposed object</param>
        public PropertyFactory(PropertyProvider propertyProvider)
        {
            _propertyProvider = propertyProvider;
        }

        /// <summary>
        /// Creates the Property of specialized type.
        ///   If a <see cref="ComplexProperty"/> is recursivly pointing to some other object already processed
        ///   its RecursionId is set to a non-0 value to indicate possible
        ///   endless-Recursion.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>NullProperty if the value is null</returns>
        public Property CreateProperty(string name, object value)
        {
            if (value == null) return new NullProperty(name);

            // If value type is recognized, it will be taken from typeinfo cache
            TypeInfo typeInfo = TypeInfo.GetTypeInfo(value);

            // Is it simple type
            Property property = createSimpleProperty(name, typeInfo, value);
            if (property != null)
            {
                // It is simple type
                return property;
            }

            // Is it array?
            if (typeInfo.IsArray)
            {
                if (typeInfo.ArrayDimensionCount < 2)
                {
                    // 1D-Array
                    property = createSingleDimensionalArrayProperty(name, typeInfo, value);
                }
                else
                {
                    // MultiD-Array
                    property = createMultiDimensionalArrayProperty(name, typeInfo, value);
                }
            }
            else
            {
                if (typeInfo.IsDictionary)
                {
                    property = createDictionaryProperty(name, typeInfo, value);
                }
                else
                {
                    if (typeInfo.IsCollection)
                    {
                        property = createCollectionProperty(name, typeInfo, value);
                    }
                    else
                    {
                        if (typeInfo.IsEnumerable)
                        {
                            // Actually it would be enough to check if the typeinfo.IsEnumerable is true...
                            property = createCollectionProperty(name, typeInfo, value);
                        }
                    }
                }
            }

            ComplexProperty complexProperty;
            if (property == null)
            {
                // special Property not created yet
                if (nonDuplicateValues.TryGetValue(value, out complexProperty))
                {
                    // it was already processed => its recursive
                    if (!complexProperty.IsReferencedMoreThanOnce)
                        complexProperty.ComplexReferenceId = nextReferenceId++; // mark as recursive, if necessary
                    return new ComplexReferenceProperty(name, complexProperty);
                }

                // If nothing was recognized, a complex type will be created
                property = new ComplexProperty(name, typeInfo.Type, value);
            }

            // Estimating properties of the complex type
            complexProperty = property as ComplexProperty;
            if (complexProperty != null)
            {
                nonDuplicateValues.Add(value, complexProperty);

                IList<PropertyInfo> propertyInfos = _propertyProvider.GetProperties(typeInfo);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    object subValue = propertyInfo.GetValue(value, _emptyObjectArray);

                    Property subProperty = CreateProperty(propertyInfo.Name, subValue);

                    complexProperty.Properties.Add(subProperty);
                }
            }
            return property;
        }

        private Property createCollectionProperty(string name, TypeInfo info, object value)
        {
            var property = new CollectionProperty(name, info.Type);
            property.ElementType = info.ElementType;

            // Items
            var collection = (ICollection) value;
            foreach (object item in collection)
            {
                Property itemProperty = CreateProperty(null, item);

                property.Items.Add(itemProperty);
            }

            return property;
        }

        private Property createDictionaryProperty(string name, TypeInfo info, object value)
        {
            var property = new DictionaryProperty(name, info.Type);

            property.KeyType = info.KeyType;
            property.ValueType = info.ElementType;

            // Items
            var dictionary = (IDictionary) value;
            foreach (DictionaryEntry entry in dictionary)
            {
                Property keyProperty = CreateProperty(null, entry.Key);

                Property valueProperty = CreateProperty(null, entry.Value);

                property.Items.Add(new KeyValueItem(keyProperty, valueProperty));
            }

            return property;
        }

        private Property createMultiDimensionalArrayProperty(string name, TypeInfo info, object value)
        {
            var property = new MultiDimensionalArrayProperty(name, info.Type);
            property.ElementType = info.ElementType;

            var analyzer = new ArrayAnalyzer(value);

            // DimensionInfos
            property.DimensionInfos = analyzer.ArrayInfo.DimensionInfos;

            // Items
            foreach (var indexSet in analyzer.GetIndexes())
            {
                object subValue = ((Array) value).GetValue(indexSet);
                Property itemProperty = CreateProperty(null, subValue);

                property.Items.Add(new MultiDimensionalArrayItem(indexSet, itemProperty));
            }
            return property;
        }

        private Property createSingleDimensionalArrayProperty(string name, TypeInfo info, object value)
        {
            var property = new SingleDimensionalArrayProperty(name, info.Type);
            property.ElementType = info.ElementType;

            var analyzer = new ArrayAnalyzer(value);

            // Dimensionen
            DimensionInfo dimensionInfo = analyzer.ArrayInfo.DimensionInfos[0];
            property.LowerBound = dimensionInfo.LowerBound;

            // Items
            foreach (object item in analyzer.GetValues())
            {
                Property itemProperty = CreateProperty(null, item);

                property.Items.Add(itemProperty);
            }

            return property;
        }

        private static Property createSimpleProperty(string name, TypeInfo typeInfo, object value)
        {
            if (!typeInfo.IsSimple) return null;
            var result = new SimpleProperty(name, typeInfo.Type);
            result.Value = value;
            return result;
        }
    }
}