using Machine.Specifications;
using Quarks.ObjectExtensions;

namespace Quarks.Tests.ObjectExtensions
{
	[Subject(typeof(ObjectExtension))]
	class When_using_change_type_to
	{
		It should_be_able_to_convert_a_long_to_a_nullable_int = () =>
		{
			const long value = 2220;
			var result = value.ChangeTypeTo<int?>();
			result.ShouldBeAssignableTo<int?>();
			result.ShouldEqual(2220);
		};
	}
}
