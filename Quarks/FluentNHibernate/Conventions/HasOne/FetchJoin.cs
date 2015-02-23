using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasOne
{
	/// <summary>
	/// All one-to-one references, unless explicitly specified, use Fetch.Join().
	/// </summary>
	class FetchJoin : IHasOneConvention
	{
		public void Apply(IOneToOneInstance instance)
		{
			instance.Fetch.Join();
		}
	}
}
