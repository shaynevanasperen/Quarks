using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using Machine.Specifications;
using Quarks.FluentNHibernate.Conventions.HasMany;

namespace Quarks.Tests.FluentNHibernate.Conventions.HasMany
{
	[Subject(typeof(NotKeyUpdate))]
	class When_saving_an_entity_with_a_unidirectional_has_many_relationship
	{
		// NotKeyNullable
		It should_save_the_foreign_key_on_the_child_items = () =>
			retrieved.Children.Any().ShouldBeTrue();

		// NotKeyUpdate
		It should_not_use_an_additional_update_to_save_the_foreign_key_on_the_child_items = () =>
			consoleOutput.ToString().ToLower().ShouldNotContain("update");

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<EntityWithUnidirectionalChildCollection>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			consoleOutput = new StringBuilder();
			Console.SetOut(new DuplicateStringWriter(consoleOutput, Console.Out));

			persisted = new EntityWithUnidirectionalChildCollection();
			persisted.AddChildEntity(new ChildEntity());
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static EntityWithUnidirectionalChildCollection persisted, retrieved;
		static StringBuilder consoleOutput;
	}

	class DuplicateStringWriter : StringWriter
	{
		readonly TextWriter _duplicateTextWriter;

		public DuplicateStringWriter(StringBuilder sb, TextWriter duplicateTextWriter)
			: base(sb)
		{
			_duplicateTextWriter = duplicateTextWriter;
		}

		public override void WriteLine(string value)
		{
			_duplicateTextWriter.WriteLine(value);
			base.WriteLine(value);
		}
	}

	public class EntityWithUnidirectionalChildCollection : IdentityFieldProvider<EntityWithUnidirectionalChildCollection, long>
	{
		public EntityWithUnidirectionalChildCollection()
		{
			_children = new HashSet<ChildEntity>();
		}

		readonly ISet<ChildEntity> _children;
		public IEnumerable<ChildEntity> Children
		{
			get { return _children; }
		}

		public void AddChildEntity(ChildEntity child)
		{
			_children.Add(child);
		}
	}

	public class EntityWithUnidirectionalChildCollectionMap : IAutoMappingOverride<EntityWithUnidirectionalChildCollection>
	{
		public void Override(AutoMapping<EntityWithUnidirectionalChildCollection> mapping)
		{
			mapping.HasMany(x => x.Children)
				.Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore)
				.Cascade.AllDeleteOrphan();
		}
	}

	public class ChildEntity : IdentityFieldProvider<ChildEntity, long>
	{
		public virtual string Name { get; set; }
	}
}
