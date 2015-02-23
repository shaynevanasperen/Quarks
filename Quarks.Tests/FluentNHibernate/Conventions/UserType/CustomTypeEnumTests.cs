using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeByteType))]
	class When_using_enum_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.Enum.ShouldEqual(persisted.Enum);
		};

		It should_map_property_type_as_actual_enum_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(EnumEntity));
			var property = persistentClass.GetProperty(Type<EnumEntity>.MemberName(x => x.Enum));
			property.Type.ShouldBeOfExactType<EnumType<EnumType>>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<EnumEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new EnumEntity { Enum = EnumType.Member1 };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static EnumEntity persisted, retrieved;
	}

	class EnumEntity : IdentityFieldProvider<EnumEntity, int>
	{
		public virtual EnumType Enum { get; set; }
	}

	enum EnumType
	{
		Member1,
		Member2
	}
}
