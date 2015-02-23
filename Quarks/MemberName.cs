using System;
using System.Linq.Expressions;

namespace Quarks
{
	static class Type<T> where T : class
	{
		/// <summary>
		/// Get the name of an instance member from a member access lambda.
		/// </summary>
		/// <typeparam name="T">Type of containing class</typeparam>
		/// <typeparam name="TResult">The return type of the member</typeparam>
		/// <param name="expression">Lambda expression of a member</param>
		/// <returns>The name of the member</returns>
		internal static string MemberName<TResult>(Expression<Func<T, TResult>> expression)
		{
			return expression.GetMemberName();
		}

		/// <summary>
		/// Get the name of an instance member from a member access lambda.
		/// </summary>
		/// <param name="expression">Lambda expression of a member</param>
		/// <returns>The name of the member</returns>
		internal static string MemberName(Expression<Action<T>> expression)
		{
			return expression.GetMemberName();
		}
	}

	static class Instance
	{
		/// <summary>
		/// Get the name of an instance member from a member access lambda.
		/// </summary>
		/// <typeparam name="T">Type of containing class</typeparam>
		/// <typeparam name="TResult">The return type of the member</typeparam>
		/// <param name="instance">The instance being extended</param>
		/// <param name="expression">Lambda expression of a member</param>
		/// <returns>The name of the member</returns>
		internal static string MemberName<T, TResult>(this T instance, Expression<Func<T, TResult>> expression) where T : class
		{
			return expression.GetMemberName();
		}

		/// <summary>
		/// Get the name of an instance member from a member access lambda.
		/// </summary>
		/// <typeparam name="T">Type of containing class</typeparam>
		/// <param name="instance">The instance being extended</param>
		/// <param name="expression">Lambda expression of a member</param>
		/// <returns>The name of the member</returns>
		internal static string MemberName<T>(this T instance, Expression<Action<T>> expression) where T : class
		{
			return expression.GetMemberName();
		}
	}

	internal static class MemberName
	{
		internal static string GetMemberName<T, TResult>(this Expression<Func<T, TResult>> expression)
			where T : class
		{
			return getMemberName(expression.Body);
		}

		internal static string GetMemberName<T>(this Expression<Action<T>> expression)
		{
			return getMemberName(expression);
		}

		static string getMemberName(Expression expression)
		{
			var memberExpression = expression as MemberExpression;
			if (memberExpression == null)
				throw new ArgumentException("Invalid expression");

			return memberExpression.Member.Name;
		}
	}
}