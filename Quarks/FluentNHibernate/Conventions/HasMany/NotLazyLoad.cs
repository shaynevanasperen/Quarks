using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasMany
{
	/// <summary>
	/// All one-to-many references, unless explicitly specified, are NOT lazy loaded.
	/// </summary>
	class NotLazyLoad : IHasManyConvention
	{
		public void Apply(IOneToManyCollectionInstance instance)
		{
			instance.Not.LazyLoad();
		}
	}
}
