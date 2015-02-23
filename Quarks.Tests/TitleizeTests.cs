using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_titalize
	{
		It should_handle_these_cases = () =>
		{
			// Uppercases individual words and removes some characters 
			"some title".Titleize().ShouldEqual("Some Title");
			"some-title".Titleize().ShouldEqual("Some Title");
			"sometitle".Titleize().ShouldEqual("Sometitle");
			"some-title: The begining".Titleize().ShouldEqual("Some Title: The Begining");
			"some_title:_the_begining".Titleize().ShouldEqual("Some Title: The Begining");
			"some title: The_begining".Titleize().ShouldEqual("Some Title: The Begining");
		};
	}
}