using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polenter.Serialization.Core;

namespace Polenter.Serialization
{
    [TestClass]
    public class PrivateConstructorObjectDeserializationTests
    {
        public class ClassWithPrivateConstructor
        {
            public string Name { get; private set; }

            public virtual ClassWithPrivateConstructor Complex { get; set; }

            private ClassWithPrivateConstructor() { }

            public ClassWithPrivateConstructor(string name)
            {
                Name = name;
            }
        }

        [TestMethod]
        public void CanDeserializeClassWithPrivateConstructor()
        {
            var child = new ClassWithPrivateConstructor("child");

            var data = new ClassWithPrivateConstructor("MyName")
            {
                Complex = child
            };

            var settings = new SharpSerializerXmlSettings();
            var serializer = new SharpSerializer(settings);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(data, stream);

                stream.Position = 0;

                var deserialized = serializer.Deserialize(stream) as ClassWithPrivateConstructor;

                Assert.IsNotNull(deserialized);
                Assert.AreEqual(data.Name, deserialized.Name);
                Assert.AreEqual(data.Complex.Name, deserialized.Complex.Name);
            }
        }

        public class ClassWithoutParameterlessConstructor
        {

            public string Name { get; private set; }

            public virtual ClassWithoutParameterlessConstructor Complex { get; set; }

            public ClassWithoutParameterlessConstructor(string name)
            {
                Name = name;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DeserializingException))]
        public void CannotDeserializeClassWithoutParameterlessConstructor()
        {
            var child = new ClassWithoutParameterlessConstructor("child");

            var data = new ClassWithoutParameterlessConstructor("MyName")
            {
                Complex = child
            };

            var settings = new SharpSerializerXmlSettings();
            var serializer = new SharpSerializer(settings);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(data, stream);

                stream.Position = 0;

                serializer.Deserialize(stream);
            }
        }
    }
}