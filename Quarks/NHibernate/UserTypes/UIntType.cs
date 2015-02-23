using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;

namespace Quarks.NHibernate.UserTypes
{
	/// <summary>
	/// IUserType implementation for mapping System.UInt32 to a bigint.
	/// </summary>
	[Serializable]
	public class UIntType : ImmutableUserTypeBase<uint>
	{
		public override object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			var obj = NHibernateUtil.UInt32.NullSafeGet(rs, names[0]);

			return obj ?? 0;
		}

		public override void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			((IDataParameter)cmd.Parameters[index]).Value = value;
		}

		/// <remarks>
		/// MSSQL doesn't support UInt types, and Sqlite maps it to a 32-bit signed integer, so we need to map it as a 64-bit signed integer.
		/// </remarks>
		public override SqlType[] SqlTypes
		{
			get { return new[] { NHibernateUtil.Int64.SqlType }; }
		}
	}
}
