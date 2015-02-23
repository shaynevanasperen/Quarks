using System;
using System.IO;
using System.Reflection;

namespace Quarks.AssemblyExtensions
{
	static partial class AssemblyExtension
	{
		internal static string GetExecutionFolder(this Assembly assembly)
		{
			var uri = new UriBuilder(assembly.CodeBase);
			var path = Uri.UnescapeDataString(uri.Path);
			return Path.GetDirectoryName(path);
		}
	}
}
