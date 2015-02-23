using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Id
{
	/// <summary>
	/// Applies server-side autoincrement setting to primary keys
	/// </summary>
	class GeneratedByNative : IIdConvention, IIdConventionAcceptance
	{
		public void Apply(IIdentityInstance instance)
		{
			instance.UnsavedValue("0");
			// Use Native instead of Identity for extra compatibility. They are equivalent on SQL Server
			instance.GeneratedBy.Native();
		}

		/// <summary>
		/// Apply convention only to integer types.
		/// </summary>
		public void Accept(IAcceptanceCriteria<IIdentityInspector> criteria)
		{
			criteria.Expect(id => id.Type.GetUnderlyingSystemType().IsAny(
				typeof(Int16),
				typeof(Int32),
				typeof(Int64),
				typeof(UInt16),
				typeof(UInt32),
				typeof(UInt64)));
		}
	}
}
