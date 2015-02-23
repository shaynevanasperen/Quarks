using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasMany
{
	class NotKeyUpdate : IHasManyConvention
	{
		public void Apply(IOneToManyCollectionInstance instance)
		{
			instance.Not.KeyUpdate();
		}
	}
}
