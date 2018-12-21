using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            public ClassWithPrivateConstructor(String name)
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
    }
}