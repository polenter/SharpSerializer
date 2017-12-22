# SharpSerializer 3.x #

SharpSerializer is an open source XML and binary serializer for .NET. 
With only one line of code it can serialize types like: multidimensional array, nested array, array-of-arrays, polymorphic object (where value is inherited from the property type), generic type, generic listing (i.e. dictionary, collection) and many more...

SharpSerializer 3.x was ported from CodePlex to GitHub. It works with .NET Standard 1.0, .NET Framework 4.5.2 and above. 

Project URL:
https://github.com/polenter/SharpSerializer

License URL:
https://github.com/polenter/SharpSerializer/blob/master/LICENSE.txt



## Prerequisites ##

Development
Due to the new syntax of the csproj file you need Visual Studio 2017 for development of SharpSerializer.


Runtime
SharpSerializer requires a .NET platform implementing .NET Standard 1.0 or above.

According to the .NET Standard implementation support
https://docs.microsoft.com/en-us/dotnet/standard/net-standard
SharpSerializer works with:
*  .NET Core 1.0
*  .NET Framework 4.5 (with .NET Core 1.x or 2.0 SDK)
*  Mono 4.6
*  Xamarin.iOS 10.0
*  Xamarin.Mac 3.0
*  Xamarin.Android 7.0
*  Universal Windows Platform (UVP) 10.0
*  Windows 8.0
*  Windows Phone (WP) 8.1
*  Windows Phone Silverlight 8.0

For .NET Framework 4.5.2 (and above) and all .NET platforms implementing .NET Standard 1.3 there are additional API overloads providing not only serialization to a stream but also directly to a file by its name.



## Installing ##

Using NuGet is recommended.

In Visual Studio open NuGet Package Manager and type-in:
Install-Package SharpSerializer

In the Command Line (.NET CLI) use the dotnet command:
dotnet add package SharpSerializer



## Usage ##

var obj = CreateMyVerySophisticatedObject();
var serializer = new SharpSerializer();
serializer.Serialize(obj, "test.xml");
var obj2 = serializer.Deserialize("test.xml");

There are more usage examples on the tutorial page:
http://sharpserializer.com/en/tutorial/



## Usage Limitations ##

SharpSerializer serializes only public properties. If you like to serialize fields, they have to be wrapped in properties.

SharpSerializer can deserialize only types providing the default constructor.

Please refer to the following article for a workaround:
https://www.codeproject.com/Articles/240621/How-to-serialize-data-effectively-Custom-serializa



## Breaking Changes ##

Comparing to SharpSerializer 2.x

All platforms below .NET Framework 4.5 are no more supported.

Support for Portable Class Library (PCL) was depreciated. The following file is no more deployed:
Polenter.SharpSerializer.Portable.dll

There are no breaking changes in the API in .NET Framework 4.5.2 and above.

There are no breaking changes in the API expected, if using PCL, however it's not thoroughly tested yet. 

The BSD License (BSD) was changed to MIT License



## Deployment ##

The following libraries should be attached to your deployment package:
*   Polenter.SharpSerializer.dll

The following copyright notice should be attached to your software (e.g. about box):
This software includes SharpSerializer - Copyright (c) 2010-2017 Pawel Idzikowski

The content of the LICENSE.txt (MIT License) should be also attached to your software, e.g. as a link, informing people under what conditions SharpSerializer is licensed, even if your software uses other license model.



## Contributing ##

There are some tests concerning deploying and using SharpSerializer 3.x on different platforms, e.g. .NET Framework 4.5.2, .NET Core 1.0, 2.0 and Xamarin.
More tests on different platforms are however appreciated.

Additional contributors are welcome.



## Authors ##

Pawel Idzikowski, polenter (owner)



## License ##

This project is licensed under the MIT License - see LICENSE.txt file for details.



