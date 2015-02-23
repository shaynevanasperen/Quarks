using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;

namespace Quarks.NHibernate.UserTypes
{
	/// <summary>
	/// IUserType implementation for mapping System.UInt16 to a int.
	/// </summary>
	[Serializable]
	public class ByteType : ImmutableUserTypeBase<byte>
	{
		public override object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			var obj = NHibernateUtil.Byte.NullSafeGet(rs, names[0]);

			if (obj == null)
				return 0;

			return obj;
		}

		public override void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			((IDataParameter)cmd.Parameters[index]).Value = value;
		}

		public override SqlType[] SqlTypes
		{
			get { return new[] { NHibernateUtil.Byte.SqlType }; }
		}
	}
}
