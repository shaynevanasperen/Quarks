using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	public class When_using_pluralize_for_count
	{
		It should_pluralize_when_count_is_greater_than_two = () =>
			"item".PluralizeForCount(2).ShouldEqual("items");

		It should_pluralize_when_count_is_zero = () =>
			"item".PluralizeForCount(0).ShouldEqual("items");

		It should_not_pluralize_when_count_is_one = () =>
			"item".PluralizeForCount(1).ShouldEqual("item");
	}
}
