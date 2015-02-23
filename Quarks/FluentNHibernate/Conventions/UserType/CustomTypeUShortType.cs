using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Quarks.NHibernate.UserTypes;

namespace Quarks.FluentNHibernate.Conventions.UserType
{
	class CustomTypeUShortType : IUserTypeConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Property.PropertyType == typeof(ushort));
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType(typeof(UShortType));
		}
	}
}
