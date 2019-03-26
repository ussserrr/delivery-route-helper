# DeliveryRoute


## Overview
C# .NET Core project showing the concept of forming the complete connected delivery route from the set of scattered segments. For example, let's say we have the following input data:

| Segment | Start     | End           |
|---------|-----------|---------------|
| 3       | Queens    | The Bronx     |
| 1       | Manhattan | Brooklyn      |
| 4       | The Bronx | Staten Island |
| 2       | Brooklyn  | Queens        |

So the output should looks like this:

| Segment | Start     | End           |
|---------|-----------|---------------|
| 1       | Manhattan | Brooklyn      |
| 2       | Brooklyn  | Queens        |
| 3       | Queens    | The Bronx     |
| 4       | The Bronx | Staten Island |


## Approach
To describe and solve the task we introduce following entities:

 - `Point` - class representing a single point of delivery. Here we use simple `string` to identify the `Point`
 - `Segment` - class representing a directed set of 2 `Point`s so it has the defined `Start` and the `End`
 - `Route` - class encapsulating input and output data structures with arranging and reversing methods as well


## Project structure
3 projects are combined into single Visual Studio Solution.

### DeliveryRouteHelper
Project containing the library itself and some utility functions.

Dependencies:

 - *[Optional]* DocFX - generates fancy documentation


### DeliveryRoute
Example project showing the library features and usage.

Dependencies:

 - NLog - prints logs instead of `Console.WriteLine()`s


### DeliveryRouteHelper.Tests
Unit tests for the `DeliveryRouteHelper` library.

Dependencies:

 - xUnit - modern testing framework


## Build & Run

### Windows, macOS
Use Visual Studio as all-in-one solution to build, run, test and generate docs (make sure you have NuGet, dotnet, .NET Core Build Tools installed).

### Linux
Configuration was tested on Ubuntu 18.10 under `dotnet` CLI. Prerequisites:

 - .NET Core 2 - refer to official Microsoft documentation to add custom package sources and install `dotnet` from `apt`
 - Mono 5 - also refer to official Mono documentation to add custom package sources and install latest complete Mono framework (including MSBuild)
