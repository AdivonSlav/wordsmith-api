## Overview

This is a utility library, designed to hold any helper methods and classes referenced by the rest of the solution.

Some notes on code organization:
- Classes which implement an interface, or any functionality that encompasses multiple files should be placed in their own folder for namespace segregation.
- Classes which are meant to be dependency-injected should implement an interface

All of the classes added to the utlity library should be properly documented in the code for easier usage down the line.

### Logger

The [Logger](Logger.cs) warrants some further information. It is a wrapper class around NLog functionality and abstracts it away in favour of a static-class approach, instead of dependency injection. The NLog config is located in the project root and specifies some logger targets and rules. The config file is embedded into the assembly at build-time and is loaded automatically.

To use the logger, Init() must be called on startup. Afterwards, calling any of the static logger methods will route the message to stdout and a file, by default located in the build directory under /logs.