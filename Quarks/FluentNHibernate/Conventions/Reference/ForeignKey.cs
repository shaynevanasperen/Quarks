using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Reference
{
	class ForeignKey : IReferenceConvention, IReferenceConventionAcceptance
	{
		public void Apply(IManyToOneInstance instance)
		{
			instance.ForeignKey(getForeignKeyName(instance.Property.DeclaringType, instance.Property.PropertyType));
		}

		/// <summary>
		/// Ignore properties that have already been set.
		/// </summary>
		public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
		{
			criteria.Expect(x => string.IsNullOrEmpty(x.ForeignKey));
		}

		static string getForeignKeyName(Type referencingType, Type referencedType)
		{
			return string.Format("FK_{0}_{1}", referencingType.GetTableName(), referencedType.GetTableName());
		}
	}
}
