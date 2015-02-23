using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Quarks.AssemblyExtensions
{
	static partial class AssemblyExtension
	{
		/// <summary>
		/// Checks for the DebuggableAttribute on the assembly provided to determine
		/// whether it has been built in Debug mode.
		/// </summary>
		internal static bool GetIsDebugBuild(this Assembly assembly)
		{
			return assembly
				.GetCustomAttributes(false)
				.OfType<DebuggableAttribute>()
				.Select(x => x.IsJITTrackingEnabled)
				.FirstOrDefault();
		}
	}
}
