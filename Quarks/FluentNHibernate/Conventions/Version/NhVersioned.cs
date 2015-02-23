using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Version
{
	/// <summary>
	/// This convention maps a ulong "Version" property using Nhibernate's built-in incremental versioning.
	/// This is useful for entity's that won't be updated by other applications that may not include the version property
	/// on their POCO. See also <seealso cref="MssqlVersioned"/>. 
	/// The benefit of this strategy over MssqlVersionedEntityConvention is that there is no need to execute an extra
	/// SELECT after each INSERT or UPDATE to retrieve the database generated version.
	/// </summary>
	class NhVersioned : IVersionConvention, IVersionConventionAcceptance
	{
		public void Apply(IVersionInstance instance)
		{
			instance.Column("Version");
			instance.UnsavedValue("0");
		}

		public void Accept(IAcceptanceCriteria<IVersionInspector> criteria)
		{
			criteria.Expect(x => x.Name == "Version" &&
								 (x.Type.GetUnderlyingSystemType() == typeof(long) ||
								  x.Type.GetUnderlyingSystemType() == typeof(ulong)));
		}
	}
}
