using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Quarks.NHibernate.UserTypes;

namespace Quarks.FluentNHibernate.Conventions.UserType
{
	class CustomTypeByteType : IUserTypeConvention
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x => x.Property.PropertyType == typeof(byte));
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType(typeof(ByteType));
		}
	}
}
