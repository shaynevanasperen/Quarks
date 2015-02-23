using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.UserType
{
	/// <summary>
	/// Map enums to TinyInt sql types.
	/// </summary>
	class CustomSqlTypeEnum : IUserTypeConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			// Apply only to Enum properties
			criteria.Expect(x => x.Property.PropertyType.IsEnum);
			criteria.Expect(x => string.IsNullOrEmpty(x.SqlType));
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomSqlType("tinyint");
		}
	}
}
