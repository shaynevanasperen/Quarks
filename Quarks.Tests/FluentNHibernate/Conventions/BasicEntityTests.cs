using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Machine.Specifications;

namespace Quarks.Tests.FluentNHibernate.Conventions
{
	[Subject(typeof(NHibernateContext))]
	class When_using_nhibernate
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.Name.ShouldEqual(persisted.Name);
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<BasicEntity>(123);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persisted = new BasicEntity { Id = 123, Name = "Name123" };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static BasicEntity persisted, retrieved;
	}

	public class BasicEntity : IdentityFieldProvider<BasicEntity, int>
	{
		public virtual string Name { get; set; }
	}

	public class BasicEntityMap : IAutoMappingOverride<BasicEntity>
	{
		public void Override(AutoMapping<BasicEntity> mapping)
		{
			mapping.Id(x => x.Id)
				.GeneratedBy.Assigned()
				.UnsavedValue(0);
		}
	}
}
