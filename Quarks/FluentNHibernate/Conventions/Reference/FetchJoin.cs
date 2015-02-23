using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;

namespace Quarks.FluentNHibernate.Conventions.Reference
{
	/// <summary>
	/// All many-to-one references, unless explicitly specified and not lazy, use Fetch.Join().
	/// </summary>
	class ReferenceFetchJoinLoadConvention : IReferenceConvention, IReferenceConventionAcceptance
	{
		public void Apply(IManyToOneInstance instance)
		{
			instance.Fetch.Join();
		}

		public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
		{
			criteria.Expect(x => x.LazyLoad.Equals(Laziness.False));
		}
	}
}
