using System.Linq;
using Machine.Specifications;
using NHibernate.Engine;
using NHibernate.Mapping;
using Quarks.FluentNHibernate.Conventions.Version;

namespace Quarks.Tests.FluentNHibernate.Conventions.Version
{
	[Subject(typeof(NhVersioned))]
	class When_using_long_version_property
	{
		It should_map_the_class_as_versioned = () =>
			persistentClass.IsVersioned.ShouldBeTrue();

		It should_map_the_class_with_version_optimistic_locking = () =>
			persistentClass.OptimisticLockMode.ShouldEqual(Versioning.OptimisticLock.Version);

		It should_map_column_name_as_version = () =>
			persistentClass.Version.ColumnIterator.Single().Text.ShouldEqual("Version");

		It should_set_the_version_property_generation_to_never = () =>
			persistentClass.Version.Generation.ShouldEqual(PropertyGeneration.Never);

		It should_increment_the_version_upon_saving_changes = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<NhVersionedEntity>(persisted.Id);
				retrieved.Name = "New Name!";
				retrieved.AnotherField = "Updated Field Value";
				retrieved.YetAnotherField = 6;
				retrieved.Version.ShouldEqual(persisted.Version);
				transaction.Commit();
				retrieved.Version.ShouldNotEqual(persisted.Version);
			}
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<NhVersionedEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(NhVersionedEntity));
			persisted = new NhVersionedEntity
			{
				Name = "Original Name",
				YetAnotherField = 4
			};
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
			persisted.Version.ShouldNotEqual(0);
		};

		static NhVersionedEntity persisted, retrieved;
		static PersistentClass persistentClass;
	}

	class NhVersionedEntity : IdentityFieldProvider<NhVersionedEntity, int>
	{
		public virtual string Name { get; set; }
		public virtual string AnotherField { get; set; }
		public virtual int YetAnotherField { get; set; }
		public virtual long Version { get; set; }
	}
}
