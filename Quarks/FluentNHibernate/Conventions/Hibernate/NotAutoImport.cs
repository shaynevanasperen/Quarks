using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Hibernate
{
	class NotAutoImport : IHibernateMappingConvention
	{
		public void Apply(IHibernateMappingInstance instance)
		{
			instance.Not.AutoImport(); // Required in order to have entity classes with same name under different namespaces
		}
	}
}
