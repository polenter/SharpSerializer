using System;
using System.Collections.Generic;

namespace HelloWorldApp.BusinessObjects
{
    public class RootContainer
    {
        public SByte SimpleSByte { get; set; }

        /// <summary>
        /// Int is of simple type and is converted to SimpleProperty
        /// </summary>
        public int SimpleInt { get; set; }

        /// <summary>
        /// 4 Byte Float
        /// </summary>
        public Single SimpleSingle { get; set; }

        /// <summary>
        /// Double is simple type too.
        /// </summary>
        public double SimpleDouble { get; set; }

        /// <summary>
        /// DateTime is also simple type
        /// </summary>
        public DateTime SimpleDateTime { get; set; }

        /// <summary>
        /// TimeSpan is simple type
        /// </summary>
        public TimeSpan SimpleTimeSpan { get; set; }

        /// <summary>
        /// Guid is simple type (since 2.8
        /// </summary>
        public Guid SimpleGuid { get; set; }

        /// <summary>
        /// Every enumeration is simple type
        /// </summary>
        public SimpleEnum SimpleEnum { get; set; }

        /// <summary>
        /// Every enumeration is of simple type.		
        /// Enumeration can inherit from another primitive type which is not longer than Int32. 
        /// </summary>
        public SimpleEnumInheritedFromByte SimpleEnumInheritedFromByte { get; set; }

        /// <summary>
        /// Enumeration with FlagsAttribute is SimpleType. 
        /// It is correct serialized if the result of the flag combination has unique int value,
        /// i.e. Flag1 = 2, Flag2 = 4, Flag3 = 8 ...
        /// </summary>
        public FlagEnum FlagsEnum { get; set; }

        /// <summary>
        /// Enumeration with FlagsAttribute is of simple type. 
        /// It can be inherited from another primitive type which is not longer than Int32
        /// </summary>
        public FlagEnumInheritedFromUInt16 FlagEnumInheritedFromUInt16 { get; set; }

        /// <summary>
        /// Decimal is 16 bytes long
        /// </summary>
        public decimal SimpleDecimal { get; set; }

        /// <summary>
        /// String is always of simple type and will be serialized as SimpleProperty. 
        /// If the string is null, it will be serialized as NullProperty
        /// </summary>
        public string SimpleString { get; set; }

        /// <summary>
        /// Default will be string.Empty serialized.
        /// </summary>
        public string EmptyString { get; set; }

        /// <summary>
        /// Structures are handled as objects during serialization
        /// They are serialized as ComplexProperty
        /// </summary>
        public AdvancedStruct AdvancedStruct { get; set; }

        /// <summary>
        /// Type is serialized as a simple object
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// One dimensional array of simple type.
        /// It is serialized as SingleDimensionalArrayProperty
        /// </summary>
        public string[] SingleArray { get; set; }

        /// <summary>
        /// All arrays are serialized as collections but
        /// array of byte is serialized as a simple type
        /// </summary>
        public byte[] ArrayOfByte { get; set; }

        /// <summary>
        /// Multidimensional array of simple type.
        /// Is is serialized as MultiDimensionalArrayProperty
        /// </summary>
        public string[,] DoubleArray { get; set; }

        /// <summary>
        /// Control signs (Tab, NewLine, \0) are also serialized
        /// </summary>
        public char[] SingleArrayOfChars { get; set; }

        /// <summary>
        /// Single array of derived objects. 
        /// This is polymorphic collection - Items derive from the interface
        /// </summary>
        public IComplexObject[] PolymorphicSingleArray { get; set; }

        /// <summary>
        /// Generic list is serialized as a collection.
        /// It is serialized as CollectionProperty
        /// </summary>
        public IList<string> GenericList { get; set; }

        /// <summary>
        /// Generic Dictionary of simple types.
        /// Is is serialized as DictionaryProperty
        /// </summary>
        public IDictionary<int, string> GenericDictionary { get; set; }

        /// <summary>
        /// Generic dictionary where values are inherited from the value type
        /// </summary>
        public IDictionary<int, IComplexObject> GenericDictionaryOfPolymorphicValues { get; set; }

        /// <summary>
        /// Polymorphic property. Object instance derives from the property type
        /// Is serialized as ComplexProperty
        /// </summary>
        public IComplexObject ComplexObject { get; set; }

        /// <summary>
        /// ComplexProperty that contains a self reference
        /// </summary>
        public ComplexObject ComplexObjectWithSelfReference { get; set; }

        /// <summary>
        /// List wich contains itself in its items
        /// </summary>
        public List<object> ListWithSelfReference { get; set; }

        /// <summary>
        /// Collection where item values are 
        /// derived from the collection item type
        /// </summary>
        public ComplexObjectPolymorphicCollection ComplexObjectCollection { get; set; }

        /// <summary>
        /// Collection where item values are partally re-referenced
        /// </summary>
        public ComplexObjectPolymorphicCollection ComplexObjectCollectionWithReferences { get; set; }

        /// <summary>
        /// Dictionary where values are derived 
        /// from the prefedined dictionary value type
        /// </summary>
        public ComplexObjectPolymorphicDictionary ComplexObjectDictionary { get; set; }

        /// <summary>
        /// Dictionary containing properties and items which references itself
        /// </summary>
        public ComplexObjectExtendedDictionary ComplexObjectDictionaryWithSelfReference { get; set; }

        /// <summary>
        /// List items are derived from the generic attribute. 
        /// This is polymorphic attribute.
        /// </summary>
        public IList<IComplexObject> GenericListOfComplexObjects { get; set; }

        /// <summary>
        /// Generic object with polymorphic attribute.
        /// It is serialized as ComplexProperty
        /// </summary>
        public GenericObject<IComplexObject> GenericObjectOfComplexObject { get; set; }

        /// <summary>
        /// Multidimensional array of generic object with polymorphic attribute
        /// </summary>
        public GenericObject<IComplexObject>[,] MultiArrayOfGenericObjectWithPolymorphicArgument { get; set; }

        /// <summary>
        /// Since the v.2.20.0.0 HashSet is converted to IEnumerable during serialization.
        /// Earlier versions cannot serialized HashSet.
        /// </summary>
        public HashSet<Type> HashSetOfTypes { get; set; }

        /// <summary>
        /// Array of objects where every item can be of other type
        /// It is serialized as SingleDimensionalArrayProperty
        /// </summary>
        public object[] SingleArrayOfObjects { get; set; }

        /// <summary>
        /// Creates testdata.
        /// </summary>
        /// <returns></returns>
        public static RootContainer CreateFakeRoot()
        {
            var root = new RootContainer();

            root.SimpleSByte = -33;
            root.SimpleInt = 42;
            root.SimpleSingle = -352;
            root.SimpleDouble = 42.42;
            root.SimpleDateTime = new DateTime(2004, 5, 5);
            root.SimpleTimeSpan = new TimeSpan(5, 4, 3);
            root.SimpleGuid = Guid.NewGuid();
            root.SimpleEnum = SimpleEnum.Three;
            root.SimpleEnumInheritedFromByte = SimpleEnumInheritedFromByte.ThreeB;
            root.FlagsEnum = FlagEnum.Alfa | FlagEnum.Beta;
            root.FlagEnumInheritedFromUInt16 = FlagEnumInheritedFromUInt16.BetaU | FlagEnumInheritedFromUInt16.GammaU;

            root.SimpleDecimal = Convert.ToDecimal(17.123);
            root.SimpleString = "sth";
            root.EmptyString = string.Empty;
            root.AdvancedStruct = new AdvancedStruct() {DateTime = new DateTime(2010,4,10),SimpleText = "nix"};
            root.Type = typeof(List<string>);

            root.SingleArray = new[] {"ala", "ma", null, "kota"};
            root.ArrayOfByte = new byte[] { 66, 67, 68, 69, 70, 13, 10, 71, 72, 73, 13, 10, 159, 221 };
            root.DoubleArray = new[,] {{"k1", "k2"}, {"b1", "b2"}, {"z1", "z2"}};
            root.SingleArrayOfChars = new char[] { 'o', '\t', '\n',(char)0 };

            root.PolymorphicSingleArray = new IComplexObject[] {new ComplexObject() {SimpleInt = 999}};

            root.GenericList = new List<string> {"item1", "item2", "item3"};
            root.GenericDictionary = new Dictionary<int, string>();
            root.GenericDictionary.Add(5, null);
            root.GenericDictionary.Add(10, "ten");
            root.GenericDictionary.Add(20, "twenty");

            root.GenericDictionaryOfPolymorphicValues = new Dictionary<int, IComplexObject>();
            root.GenericDictionaryOfPolymorphicValues.Add(2012,new ComplexObject(){SimpleInt = 2012000});

            root.ComplexObject = new ComplexObject {SimpleInt = 33};

            root.ComplexObjectWithSelfReference = new ComplexObject { SimpleInt = 794, Name = "Self-Referencing" };
            root.ComplexObjectWithSelfReference.OtherComplex = root.ComplexObjectWithSelfReference;

            root.ListWithSelfReference = new List<object>();
            root.ListWithSelfReference.Add(root.ListWithSelfReference);
            
            root.ComplexObjectCollection = new ComplexObjectPolymorphicCollection
                                               {new ComplexObject {SimpleInt = 11}, new ComplexObject {SimpleInt = 12}};

            root.ComplexObjectCollectionWithReferences = new ComplexObjectPolymorphicCollection { 
                new ComplexObject { SimpleInt = 1311 }, 
                root.ComplexObjectWithSelfReference, 
                new ComplexObject { SimpleInt = 131212 } };

            
            root.ComplexObjectDictionary = new ComplexObjectPolymorphicDictionary();
            root.ComplexObjectDictionary.Add(100, new ComplexObject {SimpleInt = 101});
            root.ComplexObjectDictionary.Add(200, new ComplexObject {SimpleInt = 202});
            root.ComplexObjectDictionary.Add(300, null);

            root.ComplexObjectDictionaryWithSelfReference = new ComplexObjectExtendedDictionary();
            root.ComplexObjectDictionaryWithSelfReference.ReferenceObject =
                root.ComplexObjectDictionaryWithSelfReference;
            root.ComplexObjectDictionaryWithSelfReference.Add(44, root.ComplexObjectDictionaryWithSelfReference);

            root.GenericListOfComplexObjects = new List<IComplexObject> {new ComplexObject {SimpleInt = 303}};
            root.GenericObjectOfComplexObject = new GenericObject<IComplexObject>(new ComplexObject {SimpleInt = 12345});

            root.MultiArrayOfGenericObjectWithPolymorphicArgument = new GenericObject<IComplexObject>[1,1];
            root.MultiArrayOfGenericObjectWithPolymorphicArgument[0,0] = new GenericObject<IComplexObject>() {Data = new ComplexObject(){SimpleInt = 1357}};

            root.HashSetOfTypes = new HashSet<Type>(new Type[] {typeof(string), typeof(Type)});

            // it contains objects of different types, a nested array and a reference to another array
            root.SingleArrayOfObjects = new object[] { 42, "nothing to say", false, BusinessObjects.SimpleEnum.Three, null, new object[] { 42, "nothing to say", false, BusinessObjects.SimpleEnum.Three, null }, root.MultiArrayOfGenericObjectWithPolymorphicArgument };

            return root;
        }
    }

    public enum SimpleEnum
    {
        One,
        Two,
        Three
    }

    public enum SimpleEnumInheritedFromByte : byte
    {
        OneB,
        TwoB,
        ThreeB
    }

    [Flags]
    public enum FlagEnum
    {
        Alfa =2,
        Beta=4,
        Gamma=8
    }

    [Flags]
    public enum FlagEnumInheritedFromUInt16 : ushort
    {
        AlfaU = 2,
        BetaU = 4,
        GammaU = 8
    }
}