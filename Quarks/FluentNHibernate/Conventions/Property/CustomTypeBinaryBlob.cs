using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Type;

namespace Quarks.FluentNHibernate.Conventions.Property
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class BinaryBlobAttribute : Attribute
	{
		/// <summary>
		/// Specifies whether or not to lazy load the binary blob column data. Defaults to <value>false</value>.
		/// </summary>
		public bool LazyLoad { get; set; }
	}

	class CustomTypeBinaryBlob : IPropertyConvention, IPropertyConventionAcceptance
	{
		public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria.Expect(x =>
							x.Type.GetUnderlyingSystemType() == typeof(byte[]) &&
							getAttribute(x) != null);
		}

		public void Apply(IPropertyInstance instance)
		{
			instance.CustomType<BinaryBlobType>();
			instance.Nullable();

			// Length is set because the NHibernate BinaryBlobType only specifies the SQL type as "binary", defaulting to a max length of 8000
			instance.Length(int.MaxValue);

			lazyLoadIfSpecified(instance);
		}

		private static void lazyLoadIfSpecified(IPropertyInstance instance)
		{
			var binaryBlobAttribute = getAttribute(instance);
			if (binaryBlobAttribute != null && binaryBlobAttribute.LazyLoad)
				instance.LazyLoad();
		}

		private static BinaryBlobAttribute getAttribute(IExposedThroughPropertyInspector propertyInspector)
		{
			return propertyInspector.Property.MemberInfo.GetMemberAttribute<BinaryBlobAttribute>();
		}
	}
}
