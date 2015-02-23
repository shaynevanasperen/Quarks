using System;
using Machine.Specifications;
using Quarks.EnumExtensions;

namespace Quarks.Tests.EnumExtensions
{
	[Subject(typeof(EnumExtension))]
	class When_getting_an_attribute_from_enum_value_that_has_this_attribute
	{
		It should_return_this_attribute = () =>
			TestEnum.Two.GetAttributeOfType<SomeAttribute>().ShouldBeOfExactType<SomeAttribute>();
	}

	[Subject(typeof(EnumExtension))]
	class When_getting_an_attribute_from_enum_value_that_does_not_have_this_attribute
	{
		It should_return_null = () =>
			TestEnum.One.GetAttributeOfType<SomeAttribute>().ShouldBeNull();
	}

	enum TestEnum
	{
		One,
		[Some]
		Two
	}

	class SomeAttribute : Attribute { }
}
