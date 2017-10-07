###########################
SharpSerializer
###########################

SharpSerializer is an open source XML and binary serializer for .NET Framework Full and Core. 
It is especially usefull for tombstoning - i.e. binary serialization to isolated storage in WP7 or WinRT. With only one line of code it can serialize types like: multidimensional array, nested array, array-of-arrays, polymorphic object (where value is inherited from the property type), generic type, generic listing (i.e. dictionary, collection) and many more...


###########################
Prerequisites
###########################

SharpSerializer is deployed for the following target platforms:
.NET 4.5.2
.NET Standard 1.3
.NET Standard 1.0

The .NET Standard 1.0 is for backward compatibility with Windows Phone 8.1 and Windows 8.0.

Please refer to the following page and find a minimal version of .NET Framework required for working with SharpSerializer:
https://docs.microsoft.com/en-us/dotnet/standard/net-standard


Note 1: Since SharpSerializer v.2.12 all types are default serialized as AssemblyQualifiedName. If you are using any previous version and there are problems with the object deserialization, please activate at first serializing of type definitions as AssemblyQualifiedName. For more details please refer to the tutorial.

Note 2: SharpSerializer serializes only object properties. Fields are not serialized. If you like to serialize fields, they have to be wrapped in properties.

Note 3: SharpSerializer can deserialize only types with the default constructor.


###########################
Installing
###########################

Use NuGet in your Visual Studio project.

Open NuGet Package Manager in Visual Studio and type:
Install-Package SharpSerializer

or use .NET CLI
dotnet add package SharpSerializer


###########################
Usage
###########################

var obj = CreateMyVerySophisticatedObject();

// using default constructor serializes to xml
var serializer = new SharpSerializer();

serializer.Serialize(obj, "test.xml");

var obj2 = serializer.Deserialize("test.xml");


###########################
Deployment
###########################

You need to include the file Polenter.SharpSerializer.dll in your deployment package.


###########################
Built With
###########################

Source code was edited with Visual Studio 2017.


###########################
Contributing
###########################


###########################
Versioning
###########################


###########################
Authors
###########################

Pawel Idzikowski


###########################
License
###########################

This project is licensed under the MIT License - see LICENSE.txt file for details.


###########################
Acknowledgments
###########################
Additionaly it works with Silverlight, Windows Phone, Windows RT, Xbox and Xamarin. It is especially usefull for tombstoning - i.e. binary serialization to isolated storage in WP7 or WinRT. With only one line of code it can serialize types like: multidimensional array, nested array, array-of-arrays, polymorphic object (where value is inherited from the property type), generic type, generic listing (i.e. dictionary, collection) and many more... Refer to http://www.sharpserializer.com for details.