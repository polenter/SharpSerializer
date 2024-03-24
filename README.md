# SharpSerializer 4.x

SharpSerializer is an open source XML and binary serializer for .NET. 
With only one line of code it can serialize types like: multidimensional array, nested array, array-of-arrays, polymorphic object (where value is inherited from the property type), generic type, generic listing (i.e. dictionary, collection) and many more...

Project URL:
https://github.com/polenter/SharpSerializer

License URL:
https://github.com/polenter/SharpSerializer/blob/master/LICENSE.txt



## Prerequisites

### Runtime
SharpSerializer requires .NET Framework 4.6.2 or a .NET platform implementing .NET Standard 2.0.

Below link contains a list with supported .NET implementations:  
[.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0#select-net-standard-version)



## Installation

### From NuGet
Using NuGet is recommended.

In Visual Studio open NuGet Package Manager and type-in:
Install-Package Polenter.SharpSerializer

In the Command Line (.NET CLI) use the dotnet command:

    dotnet add package SharpSerializer


### From a standalone file

The following libraries should be attached to your deployment package:
*   Polenter.SharpSerializer.dll



## Usage

    var obj = CreateMyVerySophisticatedObject();
    var serializer = new SharpSerializer();
    serializer.Serialize(obj, "test.xml");
    var obj2 = serializer.Deserialize("test.xml");

There are more usage examples on the tutorial page:  
http://sharpserializer.com/en/tutorial/



## Usage Limitations

SharpSerializer serializes only public properties. If you like to serialize fields or private properties, they have to be wrapped in public properties.

SharpSerializer can deserialize only types providing public or private default constructor.

Please refer to the following article for a workaround:  
https://www.codeproject.com/Articles/240621/How-to-serialize-data-effectively-Custom-serializa




## Breaking Changes

### 3.x -> 4.x

Abandoned support for `netstandard1.0`, `netstandard1.3`, `netcoreapp3.1` and `net452`.

Using .NET 8 in demo projects and in unit tests.

### 2.x -> 3.x

All platforms below .NET Framework 4.5 are no more supported.

Support for Portable Class Library (PCL) was depreciated. The following file is no more deployed:  
* Polenter.SharpSerializer.Portable.dll

There are no breaking changes in the API in .NET Framework 4.5.2 and above.

There are no breaking changes in the API expected, if using PCL, however it's not thoroughly tested yet. 

The BSD License (BSD) was changed to MIT License



## Deployment

### NuGet

```
cd ./SharpSerializer
dotnet build -c Release
dotnet pack -c Release
dotnet nuget push .\bin\Release\SharpSerializer.4.0.2.nupkg -k <your-nuget-api-key>
```


## Contributing

There are some tests concerning deploying and using SharpSerializer on different platforms, e.g. .NET Framework 4.5.2, .NET Core 1.0, 2.0 and Xamarin.  
More tests on different platforms are however appreciated.

Additional contributors are welcome. 
Just change source code, create unit tests and request a pull.



## Authors

Pawel Idzikowski, polenter (owner)



## License

This project is licensed under the MIT License - see LICENSE.txt file for details.


## Copyright notice

The following copyright notice should be attached to your software (e.g. about box):
```
This software includes SharpSerializer - Copyright (c) 2010 Pawel Idzikowski
```

