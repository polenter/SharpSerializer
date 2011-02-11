using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;

namespace Polenter.Serialization
{
    /// <summary>
    ///   All labeled with that Attribute object properties are ignored during the serialization. See PropertyProvider
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MyExcludeAttribute : Attribute
    {
    }

    public class Class2BeSerialized
    {
        public string Name {get;set;}

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

    [TestClass]
    public class IgnoredAttributeTests
    {
        [TestMethod]
        public void SerializeAsXml()
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
<Complex name="Root" type="Polenter.Serialization.Class2BeSerialized, SharpSerializer.Tests">
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
    }
}
