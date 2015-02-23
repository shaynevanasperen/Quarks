using System;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;

namespace Quarks
{
	static class ApplicationAssembly
	{
		static readonly Lazy<Assembly> _instance = new Lazy<Assembly>(getApplicationAssembly, LazyThreadSafetyMode.ExecutionAndPublication);

		internal static Assembly Instance
		{
			get { return _instance.Value; }
		}

		static Assembly getApplicationAssembly()
		{
			// Are we in a web application?
			if (HttpContext.Current != null)
			{
				// Get the global application type
				var globalAsax = BuildManager.GetGlobalAsaxType();
				if (globalAsax != null && globalAsax.BaseType != null)
					return globalAsax.BaseType.Assembly;
			}
			// Provide entry assembly and fallback to executing assembly
			return Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
		}
	}
}
