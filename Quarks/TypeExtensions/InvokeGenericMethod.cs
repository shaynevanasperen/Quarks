using System;
using System.Linq;
using System.Reflection;

namespace Quarks.TypeExtensions
{
	static partial class TypeExtension
	{
		/// <summary>
		/// Allows the invocation of a generic method specifying the generic type at runtime.
		/// eg. If a method's signature is:
		///   void MyMethod&lt;T&gt;()
		/// you can use InvokeGenericMethod(instance, runtimeType, "MyMethod") to invoke it with any type.
		/// </summary>
		internal static object InvokeGenericMethod(this Type type, object instance, Type genericType, string methodName, params object[] parameters)
		{
			if (type == null) throw new ArgumentNullException("type", "cannot be null");
			if (genericType == null) throw new ArgumentNullException("genericType", "cannot be null");
			if (string.IsNullOrEmpty(methodName)) throw new ArgumentException("cannot be null or empty", "methodName");

			var parameterTypes = parameters == null ? Type.EmptyTypes : parameters.Select(x => x.GetType()).ToArray();
			var method = type.GetMethod(methodName, parameterTypes).MakeGenericMethod(genericType);
			if (method == null) throw new ArgumentException(string.Format("No method with given signature could be found on type {0}.", type), "methodName");

			try
			{
				return method.Invoke(instance, parameters ?? new object[] { });
			}
			catch (TargetInvocationException e)
			{
				// Expose the exception raised by the invoked method
				throw e.InnerException;
			}
		}
	}
}
