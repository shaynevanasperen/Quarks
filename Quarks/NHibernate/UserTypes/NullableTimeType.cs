using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;

namespace Quarks.NHibernate.UserTypes
{
	/// <summary>
	/// IUserType implementation for mapping nullable TimeSpan to an SQL2008 Time type.
	/// </summary>
	[Serializable]
	public class NullableTimeType : ImmutableUserTypeBase<TimeSpan?>
	{
		public override SqlType[] SqlTypes
		{
			get { return new[] { NHibernateUtil.TimeAsTimeSpan.SqlType }; }
		}

		public override object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			return NHibernateUtil.TimeAsTimeSpan.NullSafeGet(rs, names[0]);
		}

		public override void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			if (value == null)
				((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
			else
			{
				// The way we have to convert timespan to datetime that starts at 1753 is complete rubbish.
				// It works so it seems that this DateTime is later converted back to db Time type.
				((IDataParameter)cmd.Parameters[index]).Value = new DateTime(1753, 1, 1).Add((TimeSpan)value);
			}
		}
	}
}
