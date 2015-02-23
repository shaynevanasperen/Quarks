using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Quarks.IEnumerableExtensions
{
	/// <summary>
	/// Extends the IEnumerable&lt;T&gt; interface to enable ordering by a dynamic property.
	/// </summary>
	static partial class EnumerableExtension
	{
		/// <summary>
		/// Returns an <c>IOrderedQueryable&lt;<typeparamref name="T"/>&gt;</c> collection sorted
		/// by the property expressed in the <paramref name="propertyName"/> parameter.
		/// </summary>
		/// <typeparam name="T">The underlying type of the collection.</typeparam>
		/// <param name="collection">The source collection to sort.</param>
		/// <param name="propertyName">The name of the property to sort by.</param>
		/// <returns>Sorted <c>IOrderedQueryable</c> collection.</returns>
		[DebuggerNonUserCode]
		internal static IOrderedQueryable<T> OrderBy<T>(this IEnumerable<T> collection, string propertyName)
		{
			return applyOrder(collection, propertyName, "OrderBy");
		}

		/// <summary>
		/// Returns an <c>IOrderedQueryable&lt;<typeparamref name="T"/>&gt;</c> collection sorted in descending order
		/// by the property expressed in the <paramref name="propertyName"/> parameter.
		/// </summary>
		/// <typeparam name="T">The underlying type of the collection.</typeparam>
		/// <param name="collection">The source collection to sort.</param>
		/// <param name="propertyName">The name of the property to sort by.</param>
		/// <returns>Sorted <c>IOrderedQueryable</c> collection.</returns>
		[DebuggerNonUserCode]
		internal static IOrderedQueryable<T> OrderByDescending<T>(this IEnumerable<T> collection, string propertyName)
		{
			return applyOrder(collection, propertyName, "OrderByDescending");
		}

		/// <summary>
		/// Returns an <c>IOrderedQueryable&lt;<typeparamref name="T"/>&gt;</c> collection sorted
		/// by the property expressed in the <paramref name="propertyName"/> parameter.
		/// </summary>
		/// <typeparam name="T">The underlying type of the collection.</typeparam>
		/// <param name="collection">The source collection to sort.</param>
		/// <param name="propertyName">The name of the property to sort by.</param>
		/// <returns>Sorted <c>IOrderedQueryable</c> collection.</returns>
		[DebuggerNonUserCode]
		internal static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> collection, string propertyName)
		{
			return applyOrder(collection, propertyName, "ThenBy");
		}

		/// <summary>
		/// Returns an <c>IOrderedQueryable&lt;<typeparamref name="T"/>&gt;</c> collection sorted in descending order
		/// by the property expressed in the <paramref name="propertyName"/> parameter.
		/// </summary>
		/// <typeparam name="T">The underlying type of the collection.</typeparam>
		/// <param name="collection">The source collection to sort.</param>
		/// <param name="propertyName">The name of the property to sort by.</param>
		/// <returns>Sorted <c>IOrderedQueryable</c> collection.</returns>
		[DebuggerNonUserCode]
		internal static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> collection, string propertyName)
		{
			return applyOrder(collection, propertyName, "ThenByDescending");
		}

		/// <summary>
		/// Returns an <c>IOrderedQueryable&lt;<typeparamref name="T"/>&gt;</c> collection sorted
		/// by the property expressed in the <paramref name="propertyName"/> parameter, 
		/// using the LINQ method specified in <paramref name="methodName"/>.
		/// </summary>
		/// <typeparam name="T">The underlying type of the collection.</typeparam>
		/// <param name="collection">The source collection to sort.</param>
		/// <param name="propertyName">The name of the property to sort by.</param>
		/// <param name="methodName">LINQ method to use </param>
		/// <returns>Sorted <c>IOrderedQueryable</c> collection.</returns>
		[DebuggerNonUserCode]
		static IOrderedQueryable<T> applyOrder<T>(IEnumerable<T> collection, string propertyName, string methodName)
		{
			var props = propertyName.Split('.');
			var type = typeof(T);
			var arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			foreach (var prop in props)
			{
				// use reflection (not ComponentModel) to mirror LINQ
				PropertyInfo pi = type.GetProperty(prop);
				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}
			var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			var lambda = Expression.Lambda(delegateType, expr, arg);

			var result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
					.MakeGenericMethod(typeof(T), type)
					.Invoke(null, new object[] { collection, lambda });
			return (IOrderedQueryable<T>)result;
		}
	}
}
