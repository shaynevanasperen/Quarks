using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasMany
{
	class BatchSize100 : IHasManyConvention
	{
		public void Apply(IOneToManyCollectionInstance instance)
		{
			instance.BatchSize(100);
		}
	}
}
