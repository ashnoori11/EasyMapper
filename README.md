### EasyMapper.Standard

Easy Mapper is a lightweight and easy-to-use .NET mapping library that allows you to easily map objects in your C# code. With no dependencies and no configuration needed, you can get started with Easy Mapper right away by installing it from NuGet.

# Installation 
You can install Easy Mapper via NuGet package manager console by running the following command:
-🤖 Install-Package EasyMapper.Standard -Version 1.0.0

# Getting Started
To get started with Easy Mapper, simply install the NuGet package and add a reference to it in your project. Then, use the Map method to map objects in your code.

using EasyMapper.Mapper;

// Define source and destination objects
- var source = new SourceObject { /*...*/ };
- var destination = new DestinationObject();

// Map the source object to the destination object
- destination.Map<SourceObject,DestinationObject>(source);

# Features
- 😎 Simple and intuitive API for object mapping in C#.
- ✌️ Supports mapping of complex object hierarchies.
- 🎉 No configuration required - just install the NuGet package and start using it in your code.

# Contributing
We welcome contributions from the community! If you find a bug, have a feature request, or want to contribute code or documentation, please open an issue or a pull request on GitHub.

# License
Easy Mapper is released under the MIT License.

