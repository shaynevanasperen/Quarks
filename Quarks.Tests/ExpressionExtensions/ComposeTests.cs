using System;
using System.Linq.Expressions;
using Machine.Specifications;
using Quarks.ExpressionExtensions;

namespace Quarks.Tests.ExpressionExtensions
{
	[Subject(typeof(ExpressionExtension))]
	class When_composing_expression_with_another_one_using_and_also
	{
		It should_return_product_of_this_combination = () =>
			combinedExpression.Compile()(3).ShouldBeFalse();

		Establish context = () =>
		{
			Expression<Func<int, bool>> baseExpression = x => x > 1;
			Expression<Func<int, bool>> additionalExpression = x => x < 3;
			combinedExpression = baseExpression.AndAlso(additionalExpression);
		};

		static Expression<Func<int, bool>> combinedExpression;
	}

	[Subject(typeof(ExpressionExtension))]
	class When_composing_expression_with_another_one_using_or_else
	{
		It should_return_product_of_this_combination = () =>
			combinedExpression.Compile()(3).ShouldBeTrue();

		Establish context = () =>
		{
			Expression<Func<int, bool>> baseExpression = x => x == 1;
			Expression<Func<int, bool>> additionalExpression = x => x == 3;
			combinedExpression = baseExpression.OrElse(additionalExpression);
		};

		static Expression<Func<int, bool>> combinedExpression;
	}
}