using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Machine.Specifications;
using Quarks.FluentNHibernate;

namespace Quarks.Tests.FluentNHibernate
{
	[Subject(typeof(FilterableTypeSource))]
	class When_creating_a_type_filter_and_then_filtering_further
	{
		It should_apply_both_predicates = () =>
			types.Single().ShouldEqual(typeof(Foo));

		Because of = () =>
			types = FilterableTypeSource.CreateTypeFilter(x => typeof(Foo).IsAssignableFrom(x))
				.FromAssemblies(Assembly.GetExecutingAssembly()).Where(x => x.BaseType == typeof(object)).GetTypes();

		static IEnumerable<Type> types;

		class Foo { }
		class Bar : Foo { }
		class Baz : Bar { }
	}
}
