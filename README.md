# code-scanner
A code scanner for .NET applications

# Dev work
You'll need Visual Studio 2017, .NET Core, and nuget in order to get things setup.
I'm also using ncrunch, but feel free to use your own nunit-compatible test runner.

Other dependencies will be added later.

# The major parts

## Console Application

This is intended to be a quick hack to get the ability to search for classes and
index code. Other interfaces should be possible, but created elsewhere.

## Library

Most of the actual business logic should go here. Stay tuned for what the actual 
architecture looks like, once I stub it out.

## Tests

There's a single test library for both the command line interface and the library.
That will probably get refactored when there's an actual library of tests for both
parts of the system. In the meantime, this will do.

Because I'm using LightBDD, most tests are partial classes. So, there's a quirky 
folder organization to keep things separate. When setting namespaces, ignore the
folder structure, and just set the namespace to be the same as all the other tests.
This makes sure the partial classes can match up properly.
