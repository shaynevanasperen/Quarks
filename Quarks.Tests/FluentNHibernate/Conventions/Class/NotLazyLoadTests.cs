using System.Collections.Generic;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Machine.Specifications;
using Quarks.FluentNHibernate.Conventions.Class;

namespace Quarks.Tests.FluentNHibernate.Conventions.Class
{
	[Subject(typeof(NotLazyLoad))]
	class When_none_of_the_class_members_are_specified_to_lazy_load
	{
		It should_map_the_class_as_a_whole_to_not_be_lazy = () =>
			NHibernateContext.Configuration.GetClassMapping(typeof(EntityWithoutLazyLoadReference)).IsLazy.ShouldBeFalse();
	}

	[Subject(typeof(NotLazyLoad))]
	class When_any_of_the_class_properties_are_specified_to_lazy_load
	{
		It should_map_the_class_as_a_whole_to_be_lazy = () =>
			NHibernateContext.Configuration.GetClassMapping(typeof(EntityWithLazyLoadProperty)).IsLazy.ShouldBeTrue();
	}

	[Subject(typeof(NotLazyLoad))]
	class When_any_of_the_class_one_to_ones_are_specified_to_lazy_load
	{
		It should_map_the_class_as_a_whole_to_be_lazy = () =>
			NHibernateContext.Configuration.GetClassMapping(typeof(EntityWithLazyLoadOneToOne)).IsLazy.ShouldBeTrue();
	}

	[Subject(typeof(NotLazyLoad))]
	class When_any_of_the_class_references_are_specified_to_lazy_load
	{
		It should_map_the_class_as_a_whole_to_be_lazy = () =>
			NHibernateContext.Configuration.GetClassMapping(typeof(EntityWithLazyLoadReference)).IsLazy.ShouldBeTrue();
	}

	[Subject(typeof(NotLazyLoad))]
	class When_any_of_the_class_collections_are_specified_to_lazy_load
	{
		It should_map_the_class_as_a_whole_to_be_lazy = () =>
			NHibernateContext.Configuration.GetClassMapping(typeof(EntityWithLazyLoadCollection)).IsLazy.ShouldBeTrue();
	}

	interface IEntityWithReference<TKey>
	{
		TKey Id { get; set; }
		ReferencedEntity ReferencedEntity { get; set; }
	}

	public class EntityWithLazyLoadReference : IdentityFieldProvider<EntityWithLazyLoadReference, int>, IEntityWithReference<int>
	{
		public virtual ReferencedEntity ReferencedEntity { get; set; }
	}

	public class EntityWithoutLazyLoadReference : IdentityFieldProvider<EntityWithoutLazyLoadReference, int>, IEntityWithReference<int>
	{
		public ReferencedEntity ReferencedEntity { get; set; }
	}

	public class ReferencedEntity : IdentityFieldProvider<ReferencedEntity, int>
	{
		public virtual string Name { get; set; }
	}

	public class EntityWithLazyLoadReferenceMap : IAutoMappingOverride<EntityWithLazyLoadReference>
	{
		public void Override(AutoMapping<EntityWithLazyLoadReference> mapping)
		{
			mapping.References(x => x.ReferencedEntity).LazyLoad();
		}
	}

	public class ReferencedEntityMap : IAutoMappingOverride<ReferencedEntity>
	{
		public void Override(AutoMapping<ReferencedEntity> mapping)
		{
			mapping.LazyLoad();
		}
	}

	public class EntityWithLazyLoadProperty : IdentityFieldProvider<EntityWithLazyLoadProperty, int>
	{
		public virtual string Name { get; set; }
	}

	public class EntityWithLazyLoadPropertyMap : IAutoMappingOverride<EntityWithLazyLoadProperty>
	{
		public void Override(AutoMapping<EntityWithLazyLoadProperty> mapping)
		{
			mapping.Map(x => x.Name).LazyLoad();
		}
	}

	public class EntityWithLazyLoadOneToOne : IdentityFieldProvider<EntityWithLazyLoadOneToOne, int>
	{
		public virtual ReferencedEntity ReferencedEntity { get; set; }
	}

	public class EntityWithLazyLoadOneToOneMap : IAutoMappingOverride<EntityWithLazyLoadOneToOne>
	{
		public void Override(AutoMapping<EntityWithLazyLoadOneToOne> mapping)
		{
			mapping.HasOne(x => x.ReferencedEntity).LazyLoad();
		}
	}

	public class EntityWithLazyLoadCollection : IdentityFieldProvider<EntityWithLazyLoadCollection, int>
	{
		public virtual IEnumerable<ReferencedEntity> Items { get; set; }
	}

	public class EntityWithLazyLoadCollectionMap : IAutoMappingOverride<EntityWithLazyLoadCollection>
	{
		public void Override(AutoMapping<EntityWithLazyLoadCollection> mapping)
		{
			mapping.HasMany(x => x.Items).LazyLoad();
		}
	}
}
