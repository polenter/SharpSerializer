using System;
using System.IO;
using System.Windows.Forms;

namespace HelloWorldApp
{
    public partial class Form1
    {
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sharpserializer.com");
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyCustomSimpleValueConverter : Polenter.Serialization.Advanced.Xml.ISimpleValueConverter
        {
            /// <summary>
            /// </summary>
            /// <param name = "value"></param>
            /// <returns>string.Empty if the value is null</returns>
            public string ConvertToString(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name = "text"></param>
            /// <param name = "type">expected type. Result should be of this type.</param>
            /// <returns>null if the text is null</returns>
            public object ConvertFromString(string text, Type type)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyTypeNameConverterWithCompressedTypeNames : Polenter.Serialization.Advanced.Serializing.ITypeNameConverter
        {
            /// <summary>
            ///   Gives back Type as text.
            /// </summary>
            /// <param name = "type"></param>
            /// <returns>string.Empty if the type is null</returns>
            public string ConvertToTypeName(Type type)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Gives back Type from the text.
            /// </summary>
            /// <param name = "typeName"></param>
            /// <returns></returns>
            public Type ConvertToType(string typeName)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyJsonWriter : Polenter.Serialization.Advanced.Xml.IXmlWriter
        {
            ///<summary>
            ///  Writes start tag/node/element
            ///</summary>
            ///<param name = "elementId"></param>
            public void WriteStartElement(string elementId)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes end tag/node/element
            ///</summary>
            public void WriteEndElement()
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes attribute of type string
            ///</summary>
            ///<param name = "attributeId"></param>
            ///<param name = "text"></param>
            public void WriteAttribute(string attributeId, string text)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes attribute of type Type
            ///</summary>
            ///<param name = "attributeId"></param>
            ///<param name = "type"></param>
            public void WriteAttribute(string attributeId, Type type)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes attribute of type integer
            ///</summary>
            ///<param name = "attributeId"></param>
            ///<param name = "number"></param>
            public void WriteAttribute(string attributeId, int number)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes attribute of type array of int
            ///</summary>
            ///<param name = "attributeId"></param>
            ///<param name = "numbers"></param>
            public void WriteAttribute(string attributeId, int[] numbers)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Writes attribute of a simple type (value of a SimpleProperty)
            ///</summary>
            ///<param name = "attributeId"></param>
            ///<param name = "value"></param>
            public void WriteAttribute(string attributeId, object value)
            {
                throw new NotImplementedException();
            }

            ///<summary>
            ///  Opens the stream
            ///</summary>
            ///<param name = "stream"></param>
            public void Open(Stream stream)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes all data to the stream, the stream can be further used.
            /// </summary>
            public void Close()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyVeryStrongCompressedAndEncryptedBinaryWriter : Polenter.Serialization.Advanced.Binary.IBinaryWriter
        {
            /// <summary>
            ///   Writes Element Id
            /// </summary>
            /// <param name = "id"></param>
            public void WriteElementId(byte id)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes type
            /// </summary>
            /// <param name = "type"></param>
            public void WriteType(Type type)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes property name
            /// </summary>
            /// <param name = "name"></param>
            public void WriteName(string name)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes a simple value (value of a simple property)
            /// </summary>
            /// <param name = "value"></param>
            public void WriteValue(object value)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes an integer. It saves the number with the least required bytes
            /// </summary>
            /// <param name = "number"></param>
            public void WriteNumber(int number)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Writes an array of numbers. It saves numbers with the least required bytes
            /// </summary>
            /// <param name = "numbers"></param>
            public void WriteNumbers(int[] numbers)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Opens the stream for writing
            /// </summary>
            /// <param name = "stream"></param>
            public void Open(Stream stream)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Saves the data to the stream, the stream is not closed and can be further used
            /// </summary>
            public void Close()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyVeryStrongCompressedAndEncryptedBinaryReader : Polenter.Serialization.Advanced.Binary.IBinaryReader
        {
            /// <summary>
            ///   Reads single byte
            /// </summary>
            /// <returns></returns>
            public byte ReadElementId()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Read type
            /// </summary>
            /// <returns>null if no type defined</returns>
            public Type ReadType()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Read integer which was saved as 1,2 or 4 bytes, according to its size
            /// </summary>
            /// <returns></returns>
            public int ReadNumber()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Read array of integers which were saved as 1,2 or 4 bytes, according to their size
            /// </summary>
            /// <returns>empty array if no numbers defined</returns>
            public int[] ReadNumbers()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Reads property name
            /// </summary>
            /// <returns>null if no name defined</returns>
            public string ReadName()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Reads simple value (value of a simple property)
            /// </summary>
            /// <param name = "expectedType"></param>
            /// <returns>null if no value defined</returns>
            public object ReadValue(Type expectedType)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Opens the stream for reading
            /// </summary>
            /// <param name = "stream"></param>
            public void Open(Stream stream)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///   Does nothing, the stream can be further used and has to be manually closed
            /// </summary>
            public void Close()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// It's only a fake class!
        /// </summary>
        private class MyVerySophisticatedPropertyProvider : Polenter.Serialization.Advanced.PropertyProvider
        {
        }        
    }
}