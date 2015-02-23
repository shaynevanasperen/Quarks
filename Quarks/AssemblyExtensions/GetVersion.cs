using System.Reflection;

namespace Quarks.AssemblyExtensions
{
	static partial class AssemblyExtension
	{
		internal static string GetVersion(this Assembly assembly)
		{
			return assembly.GetName().Version.ToString(3);
		}
	}
}
