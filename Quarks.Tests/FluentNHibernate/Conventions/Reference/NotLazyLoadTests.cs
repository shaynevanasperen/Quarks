using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Machine.Specifications;
using NHibernate;
using Quarks.FluentNHibernate.Conventions.Reference;

namespace Quarks.Tests.FluentNHibernate.Conventions.Reference
{
	[Subject(typeof(NotLazyLoad))]
	class When_reference_lazy_load_preference_is_not_specified
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.ReferencedEntity.ShouldEqual(persisted.ReferencedEntity);
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<EntityWithUnspecifiedReference>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			var referencedEntity = new ReferencedEntity();
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(referencedEntity);
				transaction.Commit();
			}
			persisted = new EntityWithUnspecifiedReference { ReferencedEntity = referencedEntity };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static EntityWithUnspecifiedReference persisted, retrieved;
	}

	[Subject(typeof(NotLazyLoad))]
	class When_reference_is_specified_to_lazy_load
	{
		It should_map_the_reference_as_lazy = () =>
			Catch.Exception(() => retrieved.ReferencedEntity.Name).ShouldBeOfExactType<LazyInitializationException>();

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<EntityWithLazyLoadReference>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			var referencedEntity = new ReferencedEntity();
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(referencedEntity);
				transaction.Commit();
			}
			persisted = new EntityWithLazyLoadReference { ReferencedEntity = referencedEntity };
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static EntityWithLazyLoadReference persisted, retrieved;
	}

	public interface IEntityWithReference<TKey>
	{
		TKey Id { get; set; }
		ReferencedEntity ReferencedEntity { get; set; }
	}

	public class EntityWithLazyLoadReference : IdentityFieldProvider<EntityWithLazyLoadReference, int>, IEntityWithReference<int>
	{
		public virtual ReferencedEntity ReferencedEntity { get; set; }
	}

	public class EntityWithLazyLoadReferenceMap : IAutoMappingOverride<EntityWithLazyLoadReference>
	{
		public void Override(AutoMapping<EntityWithLazyLoadReference> mapping)
		{
			mapping.References(x => x.ReferencedEntity).LazyLoad();
		}
	}

	public class EntityWithUnspecifiedReference : IdentityFieldProvider<EntityWithUnspecifiedReference, int>, IEntityWithReference<int>
	{
		public ReferencedEntity ReferencedEntity { get; set; }
	}

	public class ReferencedEntity : IdentityFieldProvider<ReferencedEntity, int>
	{
		public virtual string Name { get; set; }
	}

	public class ReferencedEntityMap : IAutoMappingOverride<ReferencedEntity>
	{
		public void Override(AutoMapping<ReferencedEntity> mapping)
		{
			mapping.LazyLoad();
		}
	}
}
