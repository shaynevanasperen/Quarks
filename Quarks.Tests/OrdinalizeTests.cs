using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_ordinalize
	{
		It should_handle_these_cases = () =>
			TestData.Ordinalized().ForEach(x => x.Key.Ordinalize().ShouldEqual(x.Value));
	}

	partial class TestData
	{
		public static IDictionary<int, string> Ordinalized()
		{
			return new Dictionary<int, string>
			{
				{ 0, "0th" },
				{ 1, "1st" },
				{ 2, "2nd" },
				{ 3, "3rd" },
				{ 4, "4th" },
				{ 5, "5th" },
				{ 6, "6th" },
				{ 7, "7th" },
				{ 8, "8th" },
				{ 9, "9th" },
				{ 10, "10th" },
				{ 11, "11th" },
				{ 12, "12th" },
				{ 13, "13th" },
				{ 14, "14th" },
				{ 20, "20th" },
				{ 21, "21st" },
				{ 22, "22nd" },
				{ 23, "23rd" },
				{ 24, "24th" },
				{ 100, "100th" },
				{ 101, "101st" },
				{ 102, "102nd" },
				{ 103, "103rd" },
				{ 104, "104th" },
				{ 110, "110th" },
				{ 1000, "1000th" },
				{ 1001, "1001st" },
			};
		}
	}
}