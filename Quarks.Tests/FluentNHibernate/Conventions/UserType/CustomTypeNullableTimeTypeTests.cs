using System;
using Machine.Specifications;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.UserType;

namespace Quarks.Tests.FluentNHibernate.Conventions.UserType
{
	[Subject(typeof(CustomTypeNullableTimeType))]
	class When_using_nullable_timepsan_property
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.NullableTime.ShouldEqual(persisted.NullableTime);
		};

		It should_map_property_type_as_nullable_time_type = () =>
		{
			var persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(NullableTimeEntity));
			var property = persistentClass.GetProperty(Type<NullableTimeEntity>.MemberName(x => x.NullableTime));
			((CustomType)property.Type).UserType.ShouldBeOfExactType<Quarks.NHibernate.UserTypes.NullableTimeType>();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<NullableTimeEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new NullableTimeEntity { NullableTime = TimeSpan.FromSeconds(1) };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static NullableTimeEntity persisted, retrieved;
	}

	public class NullableTimeEntity : IdentityFieldProvider<NullableTimeEntity, int>
	{
		public virtual TimeSpan? NullableTime { get; set; }
	}
}
