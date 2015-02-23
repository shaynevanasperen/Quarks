using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_humanize
	{
		It should_handle_these_cases = () =>
			TestData.Humanized().ForEach(x => x.Key.Humanize().ShouldEqual(x.Value));
	}

	partial class TestData
	{
		public static IDictionary<string, string> Humanized()
		{
			return new Dictionary<string, string>
			{
				{ "some_title", "Some title" },
				{ "some-title", "Some-title" },
				{ "Some_title", "Some title" },
				{ "someTitle", "Sometitle" },
				{ "someTitle_Another", "Sometitle another" }
			};
		}
	}
}