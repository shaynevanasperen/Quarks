using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Version
{
	/// <summary>
	/// This convention maps a byte[] "Version" property using MSSQL's ROWVERSION or TIMESTAMP versioning.
	/// This is useful for entity's that can be updated by other applications or processes which may not include
	/// a Version property. The versioning is handled entirely by the database. The downside is that it requires
	/// an extra SELECT for each INSERT or UPDATE to retrieve the database generated value.
	/// See also <seealso cref="NhVersioned"/>
	/// </summary>
	class MssqlVersioned : IVersionConvention, IVersionConventionAcceptance
	{
		public void Apply(IVersionInstance instance)
		{
			// These are NH default values for a type named "Version"
			instance.Column("Version");
			instance.UnsavedValue(null);
			instance.CustomSqlType("rowversion");
			// This is the change we are applying
			instance.Generated.Always();
		}

		public void Accept(IAcceptanceCriteria<IVersionInspector> criteria)
		{
			criteria.Expect(isRowVersion);
		}

		static bool isRowVersion(IVersionInspector property)
		{
			return property.Name == "Version" &&
				   property.Type.Name == "BinaryBlob";
		}
	}
}
