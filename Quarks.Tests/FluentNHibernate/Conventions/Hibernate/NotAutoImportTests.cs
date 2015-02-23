using Machine.Specifications;
using Quarks.FluentNHibernate.Conventions.Hibernate;

namespace Quarks.Tests.FluentNHibernate.Conventions.Hibernate
{
	namespace EntityNamespace1
	{
		[Subject(typeof(NotAutoImport), "This is to prove that we can have multiple classes with the same name but different namespaces, mapped to the same table.")]
		class MultiEntity : IdentityFieldProvider<MultiEntity, int>
		{
			public virtual string Name { get; set; }
			public virtual string Property1 { get; set; }
		}
	}

	namespace EntityNamespace2
	{
		[Subject(typeof(NotAutoImport), "This is to prove that we can have multiple classes with the same name but different namespaces, mapped to the same table.")]
		class MultiEntity : IdentityFieldProvider<MultiEntity, int>
		{
			public virtual string Name { get; set; }
			public virtual string Property2 { get; set; }
		}
	}
}
