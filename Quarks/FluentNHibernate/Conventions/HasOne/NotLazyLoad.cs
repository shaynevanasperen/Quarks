using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasOne
{
	/// <summary>
	/// All one-to-one references, unless explicitly specified, are NOT lazy loaded.
	/// </summary>
	class NotLazyLoad : IHasOneConvention
	{
		public void Apply(IOneToOneInstance instance)
		{
			instance.Not.LazyLoad();
		}
	}
}
