using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_capitalize
	{
		It should_handle_these_cases = () =>
			TestData.Capitalized().ForEach(x => x.Key.Capitalize().ShouldEqual(x.Value));
	}

	partial class TestData
	{
		public static IDictionary<string, string> Capitalized()
		{
			return new Dictionary<string, string>
			{
				{ "some title", "Some title" },
				{ "some Title", "Some title" },
				{ "SOMETITLE", "Sometitle" },
				{ "someTitle", "Sometitle" },
				{ "some title goes here", "Some title goes here" },
				{ "some TITLE", "Some title" }
			};
		}
	}
}