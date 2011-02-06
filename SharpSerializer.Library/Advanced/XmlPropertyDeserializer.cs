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
using System.IO;
using System.Reflection;
using Polenter.Serialization.Advanced.Deserializing;
using Polenter.Serialization.Advanced.Xml;
using Polenter.Serialization.Core;
using Polenter.Serialization.Core.Xml;

namespace Polenter.Serialization.Advanced
{
    /// <summary>
    ///   Contains logic to read data stored with XmlPropertySerializer
    /// </summary>
    public sealed class XmlPropertyDeserializer : IPropertyDeserializer
    {
        private readonly IXmlReader _reader;

        ///<summary>
        ///</summary>
        ///<param name = "reader"></param>
        public XmlPropertyDeserializer(IXmlReader reader)
        {
            _reader = reader;
        }

        #region IPropertyDeserializer Members

        /// <summary>
        ///   Open the stream to read
        /// </summary>
        /// <param name = "stream"></param>
        public void Open(Stream stream)
        {
            _reader.Open(stream);
        }

        /// <summary>
        ///   Reading the property
        /// </summary>
        /// <returns></returns>
        public Property Deserialize()
        {
            // give the first valid tag back
            string elementName = _reader.ReadElement();

            // In what xml tag is the property saved
            PropertyTag propertyTag = getPropertyTag(elementName);

            // check if the property was found
            if (propertyTag == PropertyTag.Unknown) return null;

            Property result = deserialize(propertyTag, null);
            return result;
        }

        /// <summary>
        ///   Cleans all
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        #endregion

        private Property deserialize(PropertyTag propertyTag, Type expectedType)
        {
            // Establish the property name
            string propertyName = _reader.GetAttributeAsString(Attributes.Name);

            // Establish the property type
            Type propertyType = _reader.GetAttributeAsType(Attributes.Type);

            // id propertyType is not defined, we'll take the expectedType)
            if (propertyType == null)
            {
                propertyType = expectedType;
            }

            // create the property
            Property property = createProperty(propertyTag, propertyName, propertyType);
            if (property == null)
                return null;

            // Null property?
            var nullProperty = property as NullProperty;
            if (nullProperty != null)
            {
                return nullProperty;
            }

            // is it simple property?
            var simpleProperty = property as SimpleProperty;
            if (simpleProperty != null)
            {
                parseSimpleProperty(_reader, simpleProperty);
                return simpleProperty;
            }

            var multiDimensionalArrayProperty = property as MultiDimensionalArrayProperty;
            if (multiDimensionalArrayProperty != null)
            {
                parseMultiDimensionalArrayProperty(multiDimensionalArrayProperty);
                return multiDimensionalArrayProperty;
            }

            var singleDimensionalArrayProperty = property as SingleDimensionalArrayProperty;
            if (singleDimensionalArrayProperty != null)
            {
                parseSingleDimensionalArrayProperty(singleDimensionalArrayProperty);
                return singleDimensionalArrayProperty;
            }

            var dictionaryProperty = property as DictionaryProperty;
            if (dictionaryProperty != null)
            {
                parseDictionaryProperty(dictionaryProperty);
                return dictionaryProperty;
            }

            var collectionProperty = property as CollectionProperty;
            if (collectionProperty != null)
            {
                parseCollectionProperty(collectionProperty);
                return collectionProperty;
            }

            var complexProperty = property as ComplexProperty;
            if (complexProperty != null)
            {
                parseComplexProperty(complexProperty);
                return complexProperty;
            }

            return property;
        }

        private void parseCollectionProperty(CollectionProperty property)
        {
            // ElementType
            property.ElementType = property.Type != null ? Polenter.Serialization.Serializing.TypeInfo.GetTypeInfo(property.Type).ElementType : null;

            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Properties)
                {
                    // Properties
                    readProperties(property.Properties, property.Type);
                    continue;
                }

                if (subElement == SubElements.Items)
                {
                    // Items
                    readItems(property.Items, property.ElementType);
                }
            }
        }

        private void parseDictionaryProperty(DictionaryProperty property)
        {
            if (property.Type!=null)
            {
                var typeInfo = Polenter.Serialization.Serializing.TypeInfo.GetTypeInfo(property.Type);
                property.KeyType = typeInfo.KeyType;
                property.ValueType = typeInfo.ElementType;
            }

            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Properties)
                {
                    // Properties
                    readProperties(property.Properties, property.Type);
                    continue;
                }
                if (subElement == SubElements.Items)
                {
                    // Items
                    readDictionaryItems(property.Items, property.KeyType, property.ValueType);
                }
            }
        }

        private void readDictionaryItems(IList<KeyValueItem> items, Type expectedKeyType, Type expectedValueType)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Item)
                {
                    readDictionaryItem(items, expectedKeyType, expectedValueType);
                }
            }
        }

        private void readDictionaryItem(IList<KeyValueItem> items, Type expectedKeyType, Type expectedValueType)
        {
            Property keyProperty = null;
            Property valueProperty = null;
            foreach (string subElement in _reader.ReadSubElements())
            {
                // check if key and value was found
                if (keyProperty != null && valueProperty != null) break;

                // check if valid tag was found
                PropertyTag propertyTag = getPropertyTag(subElement);
                if (propertyTag == PropertyTag.Unknown) continue;

                // items are as pair key-value defined

                // first is always the key
                if (keyProperty == null)
                {
                    // Key was not defined yet (the first item was found)
                    keyProperty = deserialize(propertyTag, expectedKeyType);
                    continue;
                }

                // key was defined (the second item was found)
                valueProperty = deserialize(propertyTag, expectedValueType);
            }

            // create the item
            var item = new KeyValueItem(keyProperty, valueProperty);
            items.Add(item);
        }

        private void parseMultiDimensionalArrayProperty(MultiDimensionalArrayProperty property)
        {
            property.ElementType = property.Type != null ? Polenter.Serialization.Serializing.TypeInfo.GetTypeInfo(property.Type).ElementType : null;

            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Dimensions)
                {
                    // Read dimensions
                    readDimensionInfos(property.DimensionInfos);
                }

                if (subElement == SubElements.Items)
                {
                    // Read items
                    readMultiDimensionalArrayItems(property.Items, property.ElementType);
                }
            }
        }

        private void readMultiDimensionalArrayItems(IList<MultiDimensionalArrayItem> items, Type expectedElementType)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Item)
                {
                    readMultiDimensionalArrayItem(items, expectedElementType);
                }
            }
        }

        private void readMultiDimensionalArrayItem(IList<MultiDimensionalArrayItem> items, Type expectedElementType)
        {
            int[] indexes = _reader.GetAttributeAsArrayOfInt(Attributes.Indexes);
            foreach (string subElement in _reader.ReadSubElements())
            {
                PropertyTag propertyTag = getPropertyTag(subElement);
                if (propertyTag == PropertyTag.Unknown) continue;

                Property value = deserialize(propertyTag, expectedElementType);
                var item = new MultiDimensionalArrayItem(indexes, value);
                items.Add(item);
            }
        }

        private void readDimensionInfos(IList<DimensionInfo> dimensionInfos)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Dimension)
                {
                    readDimensionInfo(dimensionInfos);
                }
            }
        }

        private void readDimensionInfo(IList<DimensionInfo> dimensionInfos)
        {
            var info = new DimensionInfo();
            info.Length = _reader.GetAttributeAsInt(Attributes.Length);
            info.LowerBound = _reader.GetAttributeAsInt(Attributes.LowerBound);
            dimensionInfos.Add(info);
        }

        private void parseSingleDimensionalArrayProperty(SingleDimensionalArrayProperty property)
        {
            // ElementType
            property.ElementType = property.Type != null ? Polenter.Serialization.Serializing.TypeInfo.GetTypeInfo(property.Type).ElementType : null;

            // LowerBound
            property.LowerBound = _reader.GetAttributeAsInt(Attributes.LowerBound);

            // Items
            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Items)
                {
                    readItems(property.Items, property.ElementType);
                }
            }
        }

        private void readItems(ICollection<Property> items, Type expectedElementType)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                PropertyTag propertyTag = getPropertyTag(subElement);
                if (propertyTag != PropertyTag.Unknown)
                {
                    // Property is found
                    Property subProperty = deserialize(propertyTag, expectedElementType);
                    items.Add(subProperty);
                }
            }
        }

        private void parseComplexProperty(ComplexProperty property)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                if (subElement == SubElements.Properties)
                {
                    readProperties(property.Properties, property.Type);
                }
            }
        }

        private void readProperties(PropertyCollection properties, Type ownerType)
        {
            foreach (string subElement in _reader.ReadSubElements())
            {
                PropertyTag propertyTag = getPropertyTag(subElement);
                if (propertyTag != PropertyTag.Unknown)
                {
                    // check if the property with the name exists
                    string subPropertyName = _reader.GetAttributeAsString(Attributes.Name);
                    if (string.IsNullOrEmpty(subPropertyName)) continue;

                    // estimating the propertyInfo
                    PropertyInfo subPropertyInfo = ownerType.GetProperty(subPropertyName);

                    Property subProperty = deserialize(propertyTag, subPropertyInfo.PropertyType);
                    properties.Add(subProperty);
                }
            }
        }

        private void parseSimpleProperty(IXmlReader reader, SimpleProperty property)
        {
            property.Value = _reader.GetAttributeAsObject(Attributes.Value, property.Type);
        }

        private static Property createProperty(PropertyTag tag, string propertyName, Type propertyType)
        {
            switch (tag)
            {
                case PropertyTag.Simple:
                    return new SimpleProperty(propertyName, propertyType);
                case PropertyTag.Complex:
                    return new ComplexProperty(propertyName, propertyType);
                case PropertyTag.Collection:
                    return new CollectionProperty(propertyName, propertyType);
                case PropertyTag.Dictionary:
                    return new DictionaryProperty(propertyName, propertyType);
                case PropertyTag.SingleArray:
                    return new SingleDimensionalArrayProperty(propertyName, propertyType);
                case PropertyTag.MultiArray:
                    return new MultiDimensionalArrayProperty(propertyName, propertyType);
                case PropertyTag.Null:
                    return new NullProperty(propertyName);
                default:
                    return null;
            }
        }

        private static PropertyTag getPropertyTag(string name)
        {
            if (name == Elements.SimpleObject) return PropertyTag.Simple;
            if (name == Elements.ComplexObject) return PropertyTag.Complex;
            if (name == Elements.Collection) return PropertyTag.Collection;
            if (name == Elements.SingleArray) return PropertyTag.SingleArray;
            if (name == Elements.Null) return PropertyTag.Null;
            if (name == Elements.Dictionary) return PropertyTag.Dictionary;
            if (name == Elements.MultiArray) return PropertyTag.MultiArray;

            return PropertyTag.Unknown;
        }

        #region Nested type: PropertyTag

        private enum PropertyTag
        {
            Unknown = 0,
            Simple,
            Complex,
            Collection,
            Dictionary,
            SingleArray,
            MultiArray,
            Null
        }

        #endregion
    }
}