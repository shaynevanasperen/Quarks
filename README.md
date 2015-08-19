# Quarks

A collection of source-only [NuGet packages](https://www.nuget.org/packages?q=quarks) representing
tiny bits of functionality that don't belong together in a monolithic "utility" library. These are
typically delivered as self-contained source files added to your projects as internal classes that
can be easily kept up-to-date with NuGet.

The name "Quarks" was chosen for these packages' root namespace to signify that they are **very
small**, fundamental building blocks in your application, just like in quantum physics:
> A [quark](https://en.wikipedia.org/wiki/Quark) is an elementary particle and a fundamental
constituent of matter.

The inspiration for creating this collection of packages was born out of my own frustrations related
to bundling miscellaneous helpers into a [monolithic library](http://ayende.com/blog/3986/let-us-burn-all-those-pesky-util-common-libraries),
seeing it done before in [NETFx](https://netfx.codeplex.com/), and from reading a blog post titled
[Packaging Source Code With NuGet](http://nikcodes.com/2013/10/23/packaging-source-code-with-nuget/).
I decided to create this project instead of contribute to NETFx because I was frustrated with how
cumbersome it is to contribute new packages to NETFx, as well as the fact that NETFx packages are
not marked as [development dependencies](https://docs.nuget.org/release-notes/nuget-2.8#development-dependencies).

## Why
Who doesn't have an ever growing and ever less cohesive miscellaneous collection of helpers,
extension methods and utility classes in the usual "[Common.dll](http://ayende.com/blog/3986/let-us-burn-all-those-pesky-util-common-libraries)"?
Well, the problem is that there's really no good place for all that baggage: do we split them by
actual behavioral area and create "proper" projects for them?

In most cases, that's totally overkill and you end up in short time with the same pile of assorted
files as you try to avoid setting up an entire new project to contain just a couple cohesive classes.

But it turns out that in the vast majority of cases, those helpers are just meant for internal
consumption by the actual important parts of your code. In many cases, they are just little
improvements and supplements over the base class libraries, such as adding missing overloads via
extension methods, adding factory methods for otherwise convoluted object initialization, etc. It's
almost inevitable that as the .NET framework and its languages evolve, existing APIs will start to
look dated and lacking (i.e. lack of generics from v1 APIs, or lack of async-friendly
Task-based APIs, etc.).

But with the advent of NuGet there's a new way to maintain, evolve and share those useful little
helpers: just make them content files in a NuGet package!

And thus Quarks was born: a repository of the source and accompanying unit tests for all those
helpers, neatly organized by target namespace being extended, deployed exclusively using NuGet,
and licensed entirely under MIT for everyone to use and contribute.

## Quarks package conventions and practices
Quarks NuGet packages follow a number of useful conventions which make them less intrusive and more
consistent/predictable when installed in target projects:

* If the package is a class definition which derives from an existing class, the package ID should
be prefixed with "Quarks", followed by the namespace of the class being derived from, followed by
the name of the class. For example, "Quarks.System.Web.FakeHttpResponse" derives from
System.Web.HttpResponseBase.

* If the package is a class definition which doesn't derive from an existing class, the package ID
should be prefixed with "Quarks", followed by the namespaced name of your class. For example,
"Quarks.AppSettings" or "Quarks.Page" in the "root" namespace, or "Quarks.Machine.Fakes.ConfigForASystemTimeOf"
in the "Quarks.Machine.Fakes" namespace.

* If the package is an extension method, the package ID should be prefixed with "Quarks", followed
by the target type you are extending (with an "Extensions" suffix added), followed by the name of the
extension method. For example, "Quarks.StringExtensions.Contains" extends System.String with a method
name "Contains". If the type you are extending is in the root "System" namespace, you can omit the
"System" part in the package ID. For the class that contains your extension method, it should be
named in the singular and defined as partial, so for "Quarks.StringExtensions.Contains", the class
is defined as `static partial class StringExtension`. This allows other extension method packages
for the same type to share the same class name for consistency and ease of discovery.

* Pick a name that is very specific to the helper/extension method you're creating. For example,
instead of "Quarks.StringExtensions" containing two or more extension methods, create a
"Quarks.StringExtensions.SomeMethod" package for each extension method individually. This rule
minimizes the chances of a single package becoming too big and evolving into its own Common.cs hell.

* Each Quarks package is contained in a single .cs file, with the corresponding .nuspec file having the
same name and nested beneath it using the &lt;DependentUpon&gt; syntax in the .csproj file. A useful
Visual Studio extension called [File Nesting](https://visualstudiogallery.msdn.microsoft.com/3ebde8fb-26d8-4374-a0eb-1e4e2665070c)
makes this easy to do. The .nupsec file should specify that the package is a [development dependency](https://docs.nuget.org/release-notes/nuget-2.8#development-dependencies)
in the metadata, and the target of the file should be a folder that matches the namespace of the package.

* Quarks packages should only specify dependencies on other [development dependency](https://docs.nuget.org/release-notes/nuget-2.8#development-dependencies)
packages, be they other Quarks packages or other source-only packages (or even build-time tools etc.).
It's okay to have code in a Quarks package that depends on another NuGet package that is not a
[development dependency](https://docs.nuget.org/release-notes/nuget-2.8#development-dependencies), but
in that case it should just omit that dependency in the .nuspec file. The reason for this is that the
consumer of the Quarks package would necessarily already depend on a version of that dependent package
which is being extended, thus allowing ultimate flexibility regarding versioning.

* Types and methods in Quarks packages should be marked as internal in order to not pollute the target
code's public API.

## When to create a Quarks package
There are plenty of situations where a Quarks package does not make sense. Here's a few things
to consider:

* **DO** consider creating a Quarks package for "utility" libraries that feature heavy usage of static
and/or extension methods. Examples of these types of utility libraries include unit test assertion
libraries and the popular [DataAnnotationsExtensions](https://www.nuget.org/packages/DataAnnotationsExtensions/)
package. _However, the idea behind Quarks is best suited to individual classes or methods rather than
full featured libraries._

* **DO** consider creating a Quarks package for small single purpose libraries. [SimpleJson](https://www.nuget.org/packages/SimpleJson/)
is already doing this (though not in the Quarks namespace and not following these conventions) but
you can imagine any code appropriate for a blog post or Gist would fit the definition well. _However,
the idea behind Quarks is best suited to individual classes or methods rather than full featured
libraries._

* **DO** consider creating a Quarks package for common configuration and setup code or any code
which will _require_ tweaking by the user.

* **DO NOT** consider creating a Quarks package as a means to simply make step-debugging easier.
Instead leverage a [symbols package](http://docs.nuget.org/create/creating-and-publishing-a-symbol-package).

## Modifying the code deployed by a Quarks package
The way that source-only packages work with NuGet, permits a user to modify the code that was delivered
when the package was installed. These modifications will persist when a NuGet package restore
happens, but when updating the package these changes will be overwritten (after accepting a prompt
to do so). In that case, a user can then merge their changes into the new code (if they are still
relevant).
