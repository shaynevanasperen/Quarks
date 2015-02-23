using System;
using System.Collections.Generic;
using Machine.Specifications;
using Quarks.TypeExtensions;

namespace Quarks.Tests.TypeExtensions
{
	[Subject(typeof(TypeExtension))]
	class When_invoking_a_generic_method
	{
		It should_be_able_to_invoke_a_generic_method = () =>
		{
			var test = new TestInvokeGenericMethodClass();
			typeof(TestInvokeGenericMethodClass).InvokeGenericMethod(test, typeof(int), "ReturnSameType").ShouldEqual(typeof(int));
		};

		It should_be_able_to_invoke_a_generic_method_with_parameters = () =>
		{
			var test = new TestInvokeGenericMethodClass();
			var result = (KeyValuePair<Type, int>)typeof(TestInvokeGenericMethodClass).InvokeGenericMethod(test, typeof(string), "ReturnSameTypeAndSum", 1, 2);
			result.Key.ShouldEqual(typeof(string));
			result.Value.ShouldEqual(3);
		};

		class TestInvokeGenericMethodClass
		{
			public Type ReturnSameType<T>()
			{
				return typeof(T);
			}

			public KeyValuePair<Type, int> ReturnSameTypeAndSum<T>(int x, int y)
			{
				return new KeyValuePair<Type, int>(typeof(T), x + y);
			}
		}
	}
}
