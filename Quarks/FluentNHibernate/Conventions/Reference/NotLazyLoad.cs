using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Reference
{
	/// <summary>
	/// All many-to-one references, unless explicitly specified, are NOT lazy loaded.
	/// </summary>
	class NotLazyLoad : IReferenceConvention
	{
		public void Apply(IManyToOneInstance instance)
		{
			instance.Not.LazyLoad();
		}
	}
}
