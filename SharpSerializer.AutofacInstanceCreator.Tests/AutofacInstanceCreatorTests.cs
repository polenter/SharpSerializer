namespace SharpSerializer.AutofacInstanceCreator.Tests
{
  using System.IO;
  using System.Text;
  using Autofac;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Polenter.Serialization;

  [TestClass]
  public class AutofacInstanceCreatorTests
  {
    [TestMethod]
    public void CreateInstanceWithAutofacCreatesRegisteredType()
    {
      ContainerBuilder builder = new ContainerBuilder();
      builder.RegisterType<TestObject>().As<ITestObject>();
      builder.RegisterType<TestChild>().As<ITestChild>();
      IContainer container = builder.Build();

      string serializeResult = this.Serialize(container, new TestObject());

      ITestObject deserializeResult = this.Deserialize(container, serializeResult);
      Assert.IsNotNull(deserializeResult);

      Assert.IsNotNull(deserializeResult.TestChild);
    }

    [TestMethod]
    public void CreateInstanceWithAutofacUsesDefaultCreatorWhenNotKnown()
    {
      ContainerBuilder builder = new ContainerBuilder();
      IContainer container = builder.Build();

      string serializeResult = this.Serialize(container, new TestObject());

      ITestObject deserializeResult = this.Deserialize(container, serializeResult);
      Assert.IsNotNull(deserializeResult);

      Assert.IsNull(deserializeResult.TestChild);
    }

    private string Serialize(IContainer container, ITestObject testObject)
    {
      SharpSerializer serializer = this.CreateSerializer(container);
      string result;
      using (MemoryStream stream = new MemoryStream())
      {
        serializer.Serialize(testObject, stream);
        result = Encoding.UTF8.GetString(stream.ToArray());
      }

      return result;
    }

    private SharpSerializer CreateSerializer(IContainer container)
    {
      SharpSerializerXmlSettings settings = new SharpSerializerXmlSettings();
      settings.InstanceCreator = new AutofacInstanceCreator(container);

      return new SharpSerializer(settings);
    }

    private ITestObject Deserialize(IContainer container, string serializeResult)
    {
      SharpSerializer serializer = this.CreateSerializer(container);
      object result;
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(serializeResult)))
      {
        result = serializer.Deserialize(stream);
      }

      return result as ITestObject;
    }

  }
}
