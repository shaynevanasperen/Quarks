using System;
using System.Linq;
using System.Linq.Expressions;
using Quarks.ExpressionVisitors;

namespace Quarks.ExpressionExtensions
{
	/// <summary>
	/// Expression tree extensions that helps to combine together filtering expression trees
	/// </summary>
	static partial class ExpressionExtension
	{
		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			// Build parameter map (from parameters of second to parameters of first)
			var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

			// Replace parameters in the second lambda expression with parameters from the first
			var secondBody = ParameterRebinderVisitor.ReplaceParameters(map, second.Body);

			// Apply composition of lambda expression bodies to parameters from the first expression
			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}

		internal static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.And);
		}

		internal static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.AndAlso);
		}

		internal static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.Or);
		}

		internal static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.OrElse);
		}
	}
}