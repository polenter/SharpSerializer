using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HelloWorldApp.BusinessObjects
{
    public interface IComplexObject { int SimpleInt { get; set; } }

    public class ComplexObject : IComplexObject
    {
        public int SimpleInt { get; set; } 
        public string Name { get; set; } 
        public IComplexObject OtherComplex { get; set; }
    }

    public class ComplexObjectPolymorphicCollection : Collection<IComplexObject>{}

    public class ComplexObjectCollection : Collection<ComplexObject>{}

    public class ComplexObjectPolymorphicDictionary : Dictionary<int, IComplexObject>{}

    public class ComplexObjectDictionary : Dictionary<int, ComplexObject>{}
}