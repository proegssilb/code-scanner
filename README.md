# code-scanner
A code scanner for .NET applications

[![Build status](https://ci.appveyor.com/api/projects/status/pfk5k8adlq94l545/branch/master?svg=true)](https://ci.appveyor.com/project/proegssilb/code-scanner/branch/master)

# Dev work
You'll need Visual Studio 2017, .NET Framework 4.7.1, and nuget in order to get
things setup. I'm also using ncrunch, but feel free to use your own 
nunit-compatible test runner.

Other dependencies will be added later.

# The major parts

Obviously, as this project is an early-stage Work In Progress, not everything
described here is implemented yet. By the time v1.0 is released, everything
should be implemented.

Furthermore, each feature implemented should have a spec for the feature being
used, and therefore test steps should be written testing each feature. See the
test solution for both the specs and steps.

## Console Application

This is intended to be a quick hack to get the ability to search for classes and
index code. Other interfaces should be possible, but created not within this 
project.

There is a split between the main Program and CodeSearchConsoleSession. The intent
is that CodeSearchConsoleSession should be the main application class, and be very
easy to unit test. Any data from the environment (say, the stream that represents
stdin or a stream for a config file) should be passed in from Program.

## Library

Most of the actual business logic should go here. IoC containers, how to handle
plugins, standard interfaces for major pieces of the application, and wiring
everything together, all happens here.

As hinted, the library itself doesn't do much for the application. Instead, it
manages plugins, provides a bootstrapped IoC container, and provides a class
to get objects from the IoC container and hook them up. Because this system is
based on Reactive Extensions, there can be a lot of event processing going on
without having to worry too much about how those events get handled.

The major concepts that exist are each represented by interfaces:

	- `IScanner` finds things for the first time, and notifies the system about
	  them.
	- `IRescanner` uses existing found things to find more things. For example,
	  finding Projects in a Solution.
    - `IElaborator` is used to add more detail to an existing found item. This 
      could be indexing methods, adding references to other files, or other
      enrichments.
    - `IWriter` takes a particular kind of found things, and writes them to a
      persistent store. This can be used later for querying.
    - `IPluginInstaller` is responsible for registering a plugin's classes with
      an IoC container.

Aside from plugin installers, no components ever directly reference each other.
Instead, they read from and publish to Observables, from Reactive Extensions.
This way, each component can be tested/coded in isolation, and the library can
easily wire up components based purely on interfaces, types, and observables.

## Tests

There's a single test library for both the command line interface and the library.
That will probably get refactored when there's an actual library of tests for both
parts of the system. In the meantime, this will do.

Because I'm using LightBDD, most tests are partial classes. So, there's a quirky 
folder organization to keep things separate. When setting namespaces, ignore the
folder structure, and just set the namespace to be the same as all the other tests.
This makes sure the partial classes can match up properly.
