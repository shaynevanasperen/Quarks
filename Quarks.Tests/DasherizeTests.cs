using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_dasherize
	{
		It should_handle_these_cases = () =>
			TestData.Dasherized().ForEach(x => x.Key.Dasherize().ShouldEqual(x.Value));
	}

	partial class TestData
	{
		public static IDictionary<string, string> Dasherized()
		{
			return new Dictionary<string, string>
			{
				{ "some_title", "some-title" },
				{ "some-title", "some-title" },
				{ "some_title_goes_here", "some-title-goes-here" },
				{ "some_title and_another", "some-title and-another" }
			};
		}
	}
}