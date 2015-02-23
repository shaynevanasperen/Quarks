using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Property
{
	class Column : IPropertyConvention, IPropertyConventionAcceptance
	{
		public void Apply(IPropertyInstance instance)
		{
			var abbreviation = AttributeHelper.GetTypeAttribute<AbbreviationAttribute>(instance.EntityType);
			var columnName = abbreviation == null
				? instance.Name
				: abbreviation.Abbreviation + instance.Name;
			instance.Column(columnName.EscapeColumnName());
		}

		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(p => p.Columns, Is.Not.Set);
			criteria.Expect(p => string.IsNullOrEmpty(p.Formula));
		}
	}
}
