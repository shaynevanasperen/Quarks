using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_pascalize
	{
		It should_handle_these_cases = () =>
			TestData.Pascalized().ForEach(x => x.Key.Pascalize().ShouldEqual(x.Value));
	}

	[Subject(typeof(Inflector))]
	class When_using_camalize
	{
		It should_handle_these_cases = () =>
			TestData.Pascalized().ForEach(x =>
			{
				var lowercaseFirstChar = x.Value.Substring(0, 1).ToLower() + x.Value.Substring(1);
				x.Key.Camelize().ShouldEqual(lowercaseFirstChar);
			});
	}

	partial class TestData
	{
		public static IDictionary<string, string> Pascalized()
		{
			return new Dictionary<string, string>
			{
				{ "customer", "Customer" },
				{ "CUSTOMER", "CUSTOMER" },
				{ "CUStomer", "CUStomer" },
				{ "customer_name", "CustomerName" },
				{ "customer_first_name", "CustomerFirstName" },
				{ "customer_first_name_goes_here", "CustomerFirstNameGoesHere" },
				{ "customer name", "Customer name" }
			};
		}
	}
}