# ScaffoldSharp - Modularity-oriented Development Framework
ScaffoldSharp is a __work-in-progress__ C# implementation of the Scaffold project.

Scaffold is a framework designed to help develop in a modular fashion, however note this project is in extremely early phases.


## ScaffoldSharp.Logging
Simple logging library used by scaffold, by default mapping to Console.WriteLine and Console.Error.WriteLine

## ScaffoldSharp.Core
This is the core project, it implements the instance manager, object & component API and base module framework for Scaffold.

## ScaffoldSharp.EventBus
The event bus is a message routing API, to dynamically dispatch and handle mesages without needing direct references to method calls across code, an example can be a webservice: the webservice receives an authenticate call, instead of calling an authenticator service, one can use the event bus, dispatching an AuthenticateEvent, and letting any type bound to the event handle it, to modularly handle authentication across the project, without direct references to method calls, instead using shared event types.

## ScaffoldSharp.Services
The services library implements a service framework, services are singleton instances that manage parts of a program, eg. an example service would be ConnectiveHttpService, implementing a HTTP environment to bind webservices to.


# Building
Building can be done through the .NET command line, simply have .NET SDKs 6 and 8 installed and run `dotnet build`


# Things to note
- Scaffold is a base framework, it doesn't provide the examples above, only the code to be able to implement own services and objects
- The project is organized in a modular fashion, however some components depend on one another and cannot function without their dependencies, overall all projects depend on the Core and Logging library, but many also depend on the EventBus and Services library
- While we will try to target multiple frameworks, such as netstandard 2.0, we cannot guarantee legacy C# will remain supported, but for modding purposes we will try to retain support for older frameworks as long as possible
- Scaffold was originally designed for Java, and the Java implementation is still planned, this means that some implementations of Scaffold might go out of sync over time
- Everything is in very early stages, do not expect a polished framework

# Benefits
- Scaffold is designed to help develop modular programs, with a abstract+implementation fashion for services with a priority system so services can be reimplemented by extending types, __however this is designed primarily for java__, as a result, __you will have to keep modularity in mind when developing services and work with abstracts, or at the very least virtual voids in the base type of a service should it not have an abstract base but a primary implementation__
- Scaffold's design revolves around dynamic loading based on Interfaces, Attributes(term "annotated" is often used to describe attribute use) and base types, the instance manager is highly dynamic, letting dynamically-injected types add additional function to types and objects as they are created or loaded, a good example would be this: imagine a server implements a base webservice type, instead of directly referencing any HTTP handlers within the webservice, the handlers instead reverse-bind to the webservice, allowing handlers to be developed without needing to reference them during registration, instead the instance manager reverse-registers by searching for any types by their binding
