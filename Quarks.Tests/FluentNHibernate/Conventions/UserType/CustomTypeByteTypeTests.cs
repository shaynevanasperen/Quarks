using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeByteType))]
	class When_using_byte_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.Byte.ShouldEqual(persisted.Byte);
		};

		It should_map_property_type_as_byte_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(ByteEntity));
			var property = persistentClass.GetProperty(Type<ByteEntity>.MemberName(x => x.Byte));
			((CustomType)property.Type).UserType.ShouldBeOfExactType<Quarks.NHibernate.UserTypes.ByteType>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<ByteEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new ByteEntity { Byte = 3 };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static ByteEntity persisted, retrieved;
	}

	class ByteEntity : IdentityFieldProvider<ByteEntity, int>
	{
		public virtual byte Byte { get; set; }
	}
}
