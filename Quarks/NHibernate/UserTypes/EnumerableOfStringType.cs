using System;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;

namespace Quarks.NHibernate.UserTypes
{
	[Serializable]
	public class EnumerableOfStringType : ImmutableUserTypeBase<IEnumerable<string>>
	{
		public const string LineSeparator = "\r\n";

		public override SqlType[] SqlTypes
		{
			get { return new[] { NHibernateUtil.String.SqlType }; }
		}

		public override object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			var obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);
			return decodeString(obj as string);
		}

		static IEnumerable<string> decodeString(string rawData)
		{
			return string.IsNullOrEmpty(rawData)
				? new string[0]
				: rawData.Split(new[] { LineSeparator }, StringSplitOptions.None);
		}

		public override void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			((IDataParameter)cmd.Parameters[index]).Value = encodeStrings(value as IEnumerable<string>);
		}

		static string encodeStrings(IEnumerable<string> strings)
		{
			return strings == null
				? null
				: string.Join(LineSeparator, strings);
		}
	}
}