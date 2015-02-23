using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Class
{
	/// <summary>
	/// Apply standard table name convention of "dbo.EntityClassName".
	/// </summary>
	class Table : IClassConvention, IClassConventionAcceptance
	{
		public void Apply(IClassInstance instance)
		{
			instance.Table(instance.EntityType.GetTableName());
		}

		public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
		{
			// Table is empty or the default value.
			criteria.Expect(c => string.IsNullOrEmpty(c.TableName) || c.TableName == string.Format("`{0}`", c.EntityType.Name));
		}
	}
}
