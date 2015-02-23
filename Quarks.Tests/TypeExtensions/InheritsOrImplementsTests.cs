using System;
using System.Collections.Generic;
using Machine.Specifications;
using Quarks.TypeExtensions;

namespace Quarks.Tests.TypeExtensions
{
	[Subject(typeof(TypeExtension))]
	class When_checking_if_a_type_inherits_or_implements_another_type
	{
		It should_inherit_or_implement_nested_interface_by_subclass_of_outer_interface = () =>
			typeof(BarImplementor).InheritsOrImplements<IFooInterface>().ShouldBeTrue();

		It should_not_inherit_or_implement_non_generic_interface = () =>
			typeof(FooImplementor).InheritsOrImplements<IComparable>().ShouldBeFalse();

		It should_not_inherit_or_implement_non_generic_class = () =>
			typeof(FooImplementor).InheritsOrImplements<Type>().ShouldBeFalse();

		It should_not_inherit_or_implement_open_generic_interface = () =>
			typeof(FooImplementor).InheritsOrImplements(typeof(IEnumerable<>)).ShouldBeFalse();

		It should_not_inherit_or_implement_open_generic_class = () =>
			typeof(FooImplementor).InheritsOrImplements(typeof(List<>)).ShouldBeFalse();

		It should_not_inherit_or_implement_closed_generic_interface = () =>
			typeof(FooImplementor).InheritsOrImplements<IEnumerable<int>>().ShouldBeFalse();

		It should_not_inherit_or_implement_closed_generic_class = () =>
			typeof(FooImplementor).InheritsOrImplements<List<int>>().ShouldBeFalse();

		It should_inherit_or_implement_non_generic_interface = () =>
			typeof(FooImplementor).InheritsOrImplements<IFooInterface>().ShouldBeTrue();

		It should_inherit_or_implement_non_generic_class = () =>
			typeof(FooImplementor).InheritsOrImplements<FooBase>().ShouldBeTrue();

		It should_inherit_or_implement_any_base_type = () =>
			typeof(GenericFooImplementor<>).InheritsOrImplements<FooBase>().ShouldBeTrue();

		It should_inherit_or_implement_open_generic_interface_by_closed_generic_same_interface = () =>
			typeof(IGenericFooInterface<int>).InheritsOrImplements(typeof(IGenericFooInterface<>)).ShouldBeTrue();

		It should_inherit_or_implement_open_generic_interface_by_closed_generic_subclass = () =>
			typeof(GenericFooImplementor<string>).InheritsOrImplements(typeof(IGenericFooInterface<>)).ShouldBeTrue();

		It should_inherit_or_implement_open_generic_interface_by_non_generic_subclass = () =>
			typeof(GenericFooBase).InheritsOrImplements(typeof(IGenericFooInterface<>)).ShouldBeTrue();

		It should_inherit_or_implement_open_generic_class_by_non_generic_subclass = () =>
			typeof(StringFooImplementor).InheritsOrImplements(typeof(GenericFooImplementor<>)).ShouldBeTrue();

		It should_inherit_or_implement_closed_generic_class_by_non_generic_subclass = () =>
			typeof(StringFooImplementor).InheritsOrImplements<GenericFooImplementor<string>>().ShouldBeTrue();

		It should_not_inherit_or_implement_closed_generic_class_by_non_generic_subclass = () =>
			typeof(StringFooImplementor).InheritsOrImplements<GenericFooImplementor<int>>().ShouldBeFalse();

		It should_inherit_or_implement_open_generic_interface_by_open_generic_subclass = () =>
			(typeof(GenericFooImplementor<>)).InheritsOrImplements(typeof(IGenericFooInterface<>)).ShouldBeTrue();

		It should_inherit_or_implement_closed_generic_interface_by_closed_generic_subclass_matching_in_type_argument = () =>
			typeof(GenericFooImplementor<string>).InheritsOrImplements<IGenericFooInterface<string>>().ShouldBeTrue();

		It should_not_inherit_or_implement_closed_generic_interface_by_closed_generic_subclass_not_matching_in_type_argument = () =>
			typeof(GenericFooImplementor<string>).InheritsOrImplements<IGenericFooInterface<int>>().ShouldBeFalse();

		interface IFooInterface { }

		interface IGenericFooInterface<T> { }

		class FooBase { }

		class FooImplementor : FooBase, IFooInterface { }

		class GenericFooBase : FooImplementor, IGenericFooInterface<object> { }

		class GenericFooImplementor<T> : FooImplementor, IGenericFooInterface<T> { }

		class StringFooImplementor : GenericFooImplementor<string> { }

		interface IBarInterface : IFooInterface { }

		class BarImplementor : IBarInterface { }
	}
}
