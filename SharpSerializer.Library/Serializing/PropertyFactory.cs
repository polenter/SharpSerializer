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
    ///   Decomposes object to a Property and its Subproperties
    /// </summary>
    public sealed class PropertyFactory
    {
        private readonly object[] _emptyObjectArray = new object[0];
        private readonly PropertyProvider _propertyProvider;


        /// <summary>
        /// </summary>
        /// <param name = "propertyProvider">provides all important properties of the decomposed object</param>
        public PropertyFactory(PropertyProvider propertyProvider)
        {
            _propertyProvider = propertyProvider;
        }

        /// <summary>
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "value"></param>
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

            if (property == null)
            {
                // If nothing was recognized, a complex type will be taken
                property = new ComplexProperty(name, typeInfo.Type);
            }

            // Estimating properties of the complex type
            var complexProperty = property as ComplexProperty;
            if (complexProperty != null)
            {
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

            Type[] arguments = info.Type.GetGenericArguments();
            if (arguments.Length > 0)
            {
                property.KeyType = arguments[0];
                property.ValueType = arguments[1];
            }

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