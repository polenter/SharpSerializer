﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;

namespace Polenter.Serialization
{
    /// <summary>
    ///   All labeled with that Attribute object properties are ignored during the serialization. 
    ///   See PropertyProvider
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MyExcludeAttribute : Attribute
    {
    }

    [TestClass]
    public class SerializationTests
    {
        #region XmlSerial_IgnoredAttributesShouldNotBeSerialized with helpers
        /// <summary>
        /// Local testclass to be serialized
        /// </summary>
        public class Class2BeSerialized
        {
            public string Name { get; set; }

            public string NameRule { get; set; }

            [Polenter.Serialization.ExcludeFromSerialization]
            public string NameSystemAttribute { get; set; }

            [MyExcludeAttribute]
            public string NamePrivateAttribute { get; set; }

            public virtual Class2BeSerialized Complex { get; set; }

            public virtual Class2BeSerialized ComplexRule { get; set; }

            [Polenter.Serialization.ExcludeFromSerialization]
            public virtual Class2BeSerialized ComplexSystemAttribute { get; set; }

            [MyExcludeAttribute]
            public virtual Class2BeSerialized ComplexPrivateAttribute { get; set; }
        }

        [TestMethod]
        public void XmlSerial_IgnoredAttributesShouldNotBeSerialized()
        {
            var child = new Class2BeSerialized()
            {
                Name = "child",
            };

            var data = new Class2BeSerialized()
            {
                Name = "MyName",
                NameSystemAttribute = "NameSystemAttribute invisible",
                NameRule = "NameRule invisible",
                NamePrivateAttribute = "NamePrivateAttribute invisible",
                Complex = child,
                ComplexSystemAttribute = child,
                ComplexPrivateAttribute = child,
                ComplexRule = child,
            };

            /*
<Complex name="Root" type="Polenter.Serialization.IgnoredAttributeTests+Class2BeSerialized, SharpSerializer.Tests">
  <Properties>
    <Simple name="Name" value="MyName" />
    <Complex name="Complex">
      <Properties>
        <Simple name="Name" value="child" />
        <Null name="Complex" />
      </Properties>
    </Complex>
  </Properties>
</Complex>             
             */
            XmlDocument doc = Save(data);

            // these are serialized
            Assert.AreEqual(1, doc.SelectNodes("//Simple[@name='Name' and @value='MyName']").Count, "Name=MyName");
            Assert.AreEqual(1, doc.SelectNodes("//Complex[@name='Complex']").Count, "Complex");

            // these are not serialized
            Assert.AreEqual(0, doc.SelectNodes("//Simple[@name='NameRule']").Count, "NameRule");
            Assert.AreEqual(0, doc.SelectNodes("//Simple[@name='NameSystemAttribute']").Count, "NameSystemAttribute");
            Assert.AreEqual(0, doc.SelectNodes("//Simple[@name='NamePrivateAttribute']").Count, "NamePrivateAttribute");

            Assert.AreEqual(0, doc.SelectNodes("//Complex[@name='ComplexRule']").Count, "ComplexRule");
            Assert.AreEqual(0, doc.SelectNodes("//Complex[@name='ComplexSystemAttribute']").Count, "ComplexSystemAttribute");
            Assert.AreEqual(0, doc.SelectNodes("//Complex[@name='ComplexPrivateAttribute']").Count, "ComplexPrivateAttribute");
        }

        private static XmlDocument Save(object data)
        {
            var stream = new MemoryStream();
            var settings = new SharpSerializerXmlSettings();

            settings.AdvancedSettings.PropertiesToIgnore.Add(typeof(Class2BeSerialized), "NameRule");
            settings.AdvancedSettings.PropertiesToIgnore.Add(typeof(Class2BeSerialized), "ComplexRule");

            settings.AdvancedSettings.AttributesToIgnore.Add(typeof(MyExcludeAttribute));
            // this does not work
            //settings.AdvancedSettings.PropertiesToIgnore.Add(null, "NameRule");
            //settings.AdvancedSettings.PropertiesToIgnore.Add(null, "ComplexRule");
            var serializer = new SharpSerializer(settings);

            serializer.Serialize(data, stream);

            stream.Position = 0;

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            return doc;
        }
        #endregion

        #region XmlSerial_TwoIdenticalChildsShouldBeSameInstance  with helpers
        /// <summary>
        /// Local testclass to be serialized
        /// </summary>
        public class ParentChildTestClass
        {
            public string Name { get; set; }
            public ParentChildTestClass Mother { get; set; }
            public ParentChildTestClass Father { get; set; }
        }

        [TestMethod]
        public void XmlSerial_TwoIdenticalChildsShouldBeSameInstance()
        {
            var parent = new ParentChildTestClass()
            {
                Name = "parent",
            };

            var child = new ParentChildTestClass()
            {
                Name = "child",
                Father = parent,
                Mother = parent,
            };

            Assert.AreSame(child.Father, child.Mother, "Precondition: Saved Father and Mother are same instance");

            var stream = new MemoryStream();
            var settings = new SharpSerializerXmlSettings();
            var serializer = new SharpSerializer(settings);

            serializer.Serialize(child, stream);

            /*
                <Complex name="Root" type="Polenter.Serialization.XmlSerialisationTests+ParentChildTestClass, SharpSerializer.Tests">
	                <Properties>
		                <Simple name="Name" value="child" />
		                <Complex name="Mother" id="1">
			                <Properties>
				                <Simple name="Name" value="parent" />
				                <Null name="Mother" />
				                <Null name="Father" />
			                </Properties>
		                </Complex>
		                <ComplexReference name="Father" id="1" />
	                </Properties>
                </Complex>
             */
            stream.Position = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            System.Console.WriteLine(doc.InnerXml);

            serializer = new SharpSerializer(settings);
            stream.Position = 0;
            ParentChildTestClass loaded = serializer.Deserialize(stream) as ParentChildTestClass;

            Assert.AreSame(loaded.Father, loaded.Mother, "Loaded Father and Mother are same instance");
        }


        #endregion

        #region Serial_Guid with helpers
        /// <summary>
        /// Local testclass to be serialized
        /// </summary>
        public class ClassWithGuid
        {
            public Guid Guid { get; set; }
        }

        [TestMethod]
        public void XmlSerial_ShouldSerializeGuid()
        {
            var parent = new ClassWithGuid()
            {
                Guid = Guid.NewGuid(),
            };

            var stream = new MemoryStream();
            var settings = new SharpSerializerXmlSettings();
            var serializer = new SharpSerializer(settings);

            serializer.Serialize(parent, stream);

            stream.Position = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            System.Console.WriteLine(doc.InnerXml);

            serializer = new SharpSerializer(settings);
            stream.Position = 0;
            ClassWithGuid loaded = serializer.Deserialize(stream) as ClassWithGuid;

            Assert.AreEqual(parent.Guid, loaded.Guid, "same guid");
        }

        [TestMethod]
        public void BinSerial_ShouldSerializeGuid()
        {
            var parent = new ClassWithGuid()
            {
                Guid = Guid.NewGuid(),
            };

            var stream = new MemoryStream();
            var settings = new SharpSerializerBinarySettings(BinarySerializationMode.SizeOptimized);
            var serializer = new SharpSerializer(settings);

            serializer.Serialize(parent, stream);


            serializer = new SharpSerializer(settings);
            stream.Position = 0;
            ClassWithGuid loaded = serializer.Deserialize(stream) as ClassWithGuid;

            Assert.AreEqual(parent.Guid, loaded.Guid, "same guid");
        }
        #endregion


        #region BinSerial
        [TestMethod]
        public void BinSerial_TwoIdenticalChildsShouldBeSameInstance()
        {
            var parent = new ParentChildTestClass()
            {
                Name = "parent",
            };

            var child = new ParentChildTestClass()
            {
                Name = "child",
                Father = parent,
                Mother = parent,
            };

            Assert.AreSame(child.Father, child.Mother, "Precondition: Saved Father and Mother are same instance");

            var stream = new MemoryStream();
            var settings = new SharpSerializerBinarySettings(BinarySerializationMode.SizeOptimized);
            var serializer = new SharpSerializer(settings);

            serializer.Serialize(child, stream);

            serializer = new SharpSerializer(settings);
            stream.Position = 0;
            ParentChildTestClass loaded = serializer.Deserialize(stream) as ParentChildTestClass;

            Assert.AreSame(loaded.Father, loaded.Mother, "Loaded Father and Mother are same instance");
        }
        #endregion
    }
}
