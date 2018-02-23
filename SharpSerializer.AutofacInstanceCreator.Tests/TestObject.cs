using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSerializer.AutofacInstanceCreator.Tests
{
  using Polenter.Serialization;

  public class TestObject : ITestObject, IMyInterface
  {
    public TestObject()
    {

    }

    public TestObject(ITestChild testChild)
    {
      this.TestChild = testChild;
    }

    [ExcludeFromSerialization]
    public ITestChild TestChild { get; set; }
  }
}
