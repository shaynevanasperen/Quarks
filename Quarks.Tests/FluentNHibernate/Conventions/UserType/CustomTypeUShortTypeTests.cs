using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeUIntType))]
	class When_using_ushort_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.UShort.ShouldEqual(persisted.UShort);
		};

		It should_map_property_type_as_ushort_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(UShortEntity));
			var property = persistentClass.GetProperty(Type<UShortEntity>.MemberName(x => x.UShort));
			((CustomType)property.Type).UserType.ShouldBeOfExactType<Quarks.NHibernate.UserTypes.UShortType>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<UShortEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new UShortEntity { UShort = 3 };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static UShortEntity persisted, retrieved;
	}

	class UShortEntity : IdentityFieldProvider<UShortEntity, int>
	{
		public virtual ushort UShort { get; set; }
	}
}
