using System.Collections.Generic;
using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeEnumerableOfStringType))]
	class When_using_enumerable_of_string_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.EnumerableOfString.ShouldEqual(persisted.EnumerableOfString);
		};

		It should_map_property_type_as_enumerable_of_string_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(EnumerableOfStringEntity));
			var property = persistentClass.GetProperty(Type<EnumerableOfStringEntity>.MemberName(x => x.EnumerableOfString));
			((CustomType)property.Type).UserType.ShouldBeOfExactType<Quarks.NHibernate.UserTypes.EnumerableOfStringType>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<EnumerableOfStringEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new EnumerableOfStringEntity { EnumerableOfString = new[] { "string1", "string2", "string3" } };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static EnumerableOfStringEntity persisted, retrieved;
	}

	class EnumerableOfStringEntity : IdentityFieldProvider<EnumerableOfStringEntity, int>
	{
		public virtual IEnumerable<string> EnumerableOfString { get; set; }
	}
}
