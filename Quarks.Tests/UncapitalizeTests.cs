using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_uncapitalize
	{
		It should_handle_these_cases = () =>
		{
			// Just lowers the first char and leaves the rest alone
			"some title".Uncapitalize().ShouldEqual("some title");
			"some Title".Uncapitalize().ShouldEqual("some Title");
			"SOMETITLE".Uncapitalize().ShouldEqual("sOMETITLE");
			"someTitle".Uncapitalize().ShouldEqual("someTitle");
			"some title goes here".Uncapitalize().ShouldEqual("some title goes here");
			"some TITLE".Uncapitalize().ShouldEqual("some TITLE");
		};
	}
}