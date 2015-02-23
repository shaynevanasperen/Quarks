using System;
using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Instance))]
	class When_getting_property_name_using_extension_method
	{
		It should_return_instance_property_name = () =>
			new Test().MemberName(x => x.TestProperty).ShouldEqual("TestProperty");
	}

	[Subject(typeof(Instance))]
	class When_getting_field_name_using_extension_method
	{
		It should_return_instance_property_name = () =>
			new Test().MemberName(x => x.TestField).ShouldEqual("TestField");
	}

	[Subject(typeof(Instance))]
	class When_getting_method_name_using_extension_method
	{
		It should_throw_argument_exception = () =>
			Catch.Exception(() => new Test().MemberName(x => x.TestMethod())).ShouldBeOfExactType<ArgumentException>();
	}

	[Subject(typeof(Type<>))]
	class When_getting_property_name_from_class
	{
		It should_return_instance_property_name = () =>
			Type<Test>.MemberName(x => x.TestProperty).ShouldEqual("TestProperty");
	}

	[Subject(typeof(Type<>))]
	class When_getting_field_name_from_class
	{
		It should_return_instance_property_name = () =>
			Type<Test>.MemberName(x => x.TestField).ShouldEqual("TestField");
	}

	[Subject(typeof(Type<>))]
	class When_getting_method_name_from_class
	{
		It should_throw_argument_exception = () =>
			Catch.Exception(() => Type<Test>.MemberName(x => x.TestMethod())).ShouldBeOfExactType<ArgumentException>();
	}

	class Test
	{
		public string TestProperty { get; set; }
		public int TestField;
		public void TestMethod() { }
	}
}