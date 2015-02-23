using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_underscore
	{
		It should_handle_these_cases = () =>
		{
			// Makes an underscored lowercase string
			"SomeTitle".Underscore().ShouldEqual("some_title");
			"SomeTitle".Underscore().ShouldEqual("some_title");
			"someTitle".Underscore().ShouldEqual("some_title");
			"some title".Underscore().ShouldEqual("some_title");
			"some title that will be underscored".Underscore().ShouldEqual("some_title_that_will_be_underscored");
			"SomeTitleThatWillBeUnderscored".Underscore().ShouldEqual("some_title_that_will_be_underscored");
		};
	}
}