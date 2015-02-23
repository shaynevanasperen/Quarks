using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Class
{
	class SchemaDbo : IClassConvention, IClassConventionAcceptance
	{
		public void Apply(IClassInstance instance)
		{
			instance.Schema("dbo");
		}

		public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
		{
			criteria.Expect(x => string.IsNullOrEmpty(x.Schema));
		}
	}
}
