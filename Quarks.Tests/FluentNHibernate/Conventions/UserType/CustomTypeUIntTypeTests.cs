using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeUIntType))]
	class When_using_uint_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.UInt.ShouldEqual(persisted.UInt);
		};

		It should_map_property_type_as_uint_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(UIntEntity));
			var property = persistentClass.GetProperty(Type<UIntEntity>.MemberName(x => x.UInt));
			((CustomType)property.Type).UserType.ShouldBeOfExactType<Quarks.NHibernate.UserTypes.UIntType>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<UIntEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new UIntEntity { UInt = 3 };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static UIntEntity persisted, retrieved;
	}

	class UIntEntity : IdentityFieldProvider<UIntEntity, int>
	{
		public virtual uint UInt { get; set; }
	}
}
