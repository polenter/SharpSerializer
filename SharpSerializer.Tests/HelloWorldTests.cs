using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HelloWorldApp.BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polenter.Serialization.Core;

namespace Polenter.Serialization
{
    [TestClass]
    public class HelloWorldTests
    {

        [TestMethod]
        public void HelloWorldTest()
        {
            foreach (var testCase in getHelloWorldTestCases())
            {
                // Xml
                serialize(testCase, new SharpSerializer());

                // Binary SizeOptimized
                serialize(testCase, new SharpSerializer(true));

                // Binary Burst
                var settings = new SharpSerializerBinarySettings(BinarySerializationMode.Burst);
                serialize(testCase, new SharpSerializer());
            }
        }

        #region HelloWorldTests

        private static void serialize(HelloWorldTestCase testCase, SharpSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                // serialize
                serializer.Serialize(testCase.Source, stream);

                // reset stream
                stream.Position = 0;

                // deserialize
                var result = serializer.Deserialize(stream);

                // reset stream to test if it is not closed 
                // the stream will be closed by the user
                stream.Position = 0;

                // Fix assertions
                Assert.AreEqual(testCase.Source.GetType(), result.GetType());

                // Custom assertions
                testCase.MakeAssertion(result);
            }
        }

        private static IEnumerable<HelloWorldTestCase> getHelloWorldTestCases()
        {
            yield return new SimplePropertyTestCase(Convert.ToSByte(-57));
            yield return new SimplePropertyTestCase((int)42);
            yield return new SimplePropertyTestCase(Convert.ToSingle(-352));
            yield return new SimplePropertyTestCase((double)42.42);
            yield return new SimplePropertyTestCase(new DateTime(2010,9,8));
            yield return new SimplePropertyTestCase(new TimeSpan(5, 4, 3));
            yield return new SimplePropertyTestCase(SimpleEnum.Three);
            yield return new SimplePropertyTestCase(FlagEnum.Beta | FlagEnum.Gamma);
            yield return new SimplePropertyTestCase(Convert.ToDecimal(17.123));
            yield return new SimplePropertyTestCase("simple string");
            yield return new SimplePropertyTestCase(string.Empty);

            // very long string
            char[] c = new char[70000];
            var c1 = from cc in c select 'a';
            string  veryLongString = new string(c1.ToArray());
            yield return new SimplePropertyTestCase(veryLongString);


            yield return
                new AdvancedStructTestCase(new AdvancedStruct()
                                                       {DateTime = new DateTime(2010, 4, 10), SimpleText = "nix"});

            yield return new SingleArrayTestCase<string>(new[] {"ala", "ma", null, "kota"});
            yield return new DoubleArrayTestCase(new[,] {{"k1", "k2"}, {"b1", "b2"}, {"z1", "z2"}});
            yield return new DoubleArrayWithLowerBoundsTestCase();

            yield return new PolymorphicSingleArrayTestCase();

            yield return new GenericDictionaryTestCase();
            yield return new ComplexObjectPolymorphicCollectionTestCase();

        }

        private abstract class HelloWorldTestCase
        {
            public HelloWorldTestCase()
            {
            }

            public HelloWorldTestCase(object source)
            {
                Source = source;
            }

            public object Source { get; set; }

            public abstract void MakeAssertion(object result);
        }

        #endregion

        private class SimplePropertyTestCase : HelloWorldTestCase
        {
            public SimplePropertyTestCase(object sourceObject) : base(sourceObject)
            {
            }


            public override void MakeAssertion(object result)
            {
                Assert.AreEqual(Source, result);
            }
        }

        private class AdvancedStructTestCase : HelloWorldTestCase
        {
            public AdvancedStructTestCase(object source) : base(source)
            {
            }

            public override void MakeAssertion(object result)
            {
                var s = (AdvancedStruct) Source;
                var r = (AdvancedStruct) result;

                Assert.AreEqual(s.DateTime, r.DateTime);
                Assert.AreEqual(s.SimpleInt, r.SimpleInt);
                Assert.AreEqual(s.SimpleText, r.SimpleText);
            }
        }

        private class SingleArrayTestCase<TElement> : HelloWorldTestCase
        {
            public SingleArrayTestCase(object source) : base(source)
            {
            }

            public override void MakeAssertion(object result)
            {
                var s = (TElement[]) Source;
                var r = (TElement[])result;

                for(int i=0;i<s.Length;i++)
                {
                    Assert.AreEqual(s[i],r[i]);
                }
            }
        }

        private class DoubleArrayTestCase : HelloWorldTestCase
        {
            public DoubleArrayTestCase(object source) : base(source)
            {
            }

            public override void MakeAssertion(object result)
            {
                var s = (string[,])Source;
                var r = (string[,])result;



                for (int i = 0; i < s.GetLength(0); i++)
                {
                    for(int j=0;j<s.GetLength(1);j++)
                    {
                        Assert.AreEqual(s[i, j], r[i, j]);    
                    }
                    
                }                
            }
        }

        /// <summary>
        /// Multidimensional array with lower bounds bigger then 0
        /// </summary>
        private class DoubleArrayWithLowerBoundsTestCase : HelloWorldTestCase
        {
            public DoubleArrayWithLowerBoundsTestCase()
            {
                // Creates and initializes a multidimensional Array of type String.
                var myLengthsArray = new int[] { 3, 5 };
                var myBoundsArray = new int[] { 2, 3 };
                var myArray = Array.CreateInstance(typeof(string), myLengthsArray, myBoundsArray);
                for (var i = myArray.GetLowerBound(0); i <= myArray.GetUpperBound(0); i++)
                {
                    for (var j = myArray.GetLowerBound(1); j <= myArray.GetUpperBound(1); j++)
                    {
                        var myIndicesArray = new int[] {i, j};
                        myArray.SetValue(Convert.ToString(i) + j, myIndicesArray);
                    }
                }
                Source = myArray;
            }

            public override void MakeAssertion(object result)
            {
                var s = (Array) Source;
                var r = (Array) result;
                for (int i = s.GetLowerBound(0); i <= s.GetUpperBound(0); i++)
                {
                    for (int j = s.GetLowerBound(1); j <= s.GetUpperBound(1); j++)
                    {
                        int[] myIndicesArray = new int[2] { i, j };
                        Assert.AreEqual(s.GetValue(myIndicesArray),r.GetValue(myIndicesArray));
                    }
                }
            }
        }

        private class PolymorphicSingleArrayTestCase : HelloWorldTestCase
        {
            public PolymorphicSingleArrayTestCase()
                : base(new IComplexObject[] { new ComplexObject() { SimpleInt = 999 } })
            {
            }

            public override void MakeAssertion(object result)
            {
                var s = (IComplexObject[])Source;
                var r = (IComplexObject[])result;

                for (int i = 0; i < s.Length; i++)
                {
                    Assert.AreEqual(s[i].SimpleInt, r[i].SimpleInt);
                }                
            }
        }

        private class GenericDictionaryTestCase : HelloWorldTestCase
        {
            public GenericDictionaryTestCase()
            {
                var genericDictionary = new Dictionary<int, string>();
                genericDictionary.Add(5, null);
                genericDictionary.Add(10, "ten");
                genericDictionary.Add(20, "twenty");

                this.Source = genericDictionary;
            }

            public override void MakeAssertion(object result)
            {
                var s = (Dictionary<int, string>)Source;
                var r = (Dictionary<int, string>)result;

                Assert.AreEqual(s[5], r[5]);
                Assert.AreEqual(s[10], r[10]);
                Assert.AreEqual(s[20], r[20]);
            }
        }

        private class ComplexObjectPolymorphicCollectionTestCase : HelloWorldTestCase
        {
            public ComplexObjectPolymorphicCollectionTestCase()
            {
                var complexObjectCollection =
                    new Collection<IComplexObject> { new ComplexObject { SimpleInt = 11 }, new ComplexObject { SimpleInt = 12 } };
                Source = complexObjectCollection;
            }

            public override void MakeAssertion(object result)
            {
                var s = (Collection<IComplexObject>)Source;
                var r = (Collection<IComplexObject>)result;

                for (int i = 0; i < s.Count; i++)
                {
                    Assert.AreEqual(s[i].SimpleInt, r[i].SimpleInt);
                }  
            }
        }
    }
}
