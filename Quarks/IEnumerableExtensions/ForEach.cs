using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quarks.IEnumerableExtensions
{
	/// <summary>
	/// Extends the IEnumerable&lt;T&gt; interface to include a ForEach method.
	/// </summary>
	static partial class EnumerableExtension
	{
		/// <summary>
		/// ForEach extension that enumerates over all items in an <see cref="IEnumerable{T}"/> and executes 
		/// an action.
		/// </summary>
		/// <typeparam name="T">The type that this extension is applicable for.</typeparam>
		/// <param name="collection">The enumerable instance that this extension operates on.</param>
		/// <param name="action">The action executed for each iten in the enumerable.</param>
		[DebuggerNonUserCode]
		internal static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (T item in collection)
				action(item);
		}

		/// <summary>
		/// ForEach extension that enumerates over all items in an <see cref="IEnumerator{T}"/> and executes 
		/// an action.
		/// </summary>
		/// <typeparam name="T">The type that this extension is applicable for.</typeparam>
		/// <param name="collection">The enumerator instance that this extension operates on.</param>
		/// <param name="action">The action executed for each iten in the enumerable.</param>
		[DebuggerNonUserCode]
		internal static void ForEach<T>(this IEnumerator<T> collection, Action<T> action)
		{
			while (collection.MoveNext())
				action(collection.Current);
		}
	}
}
