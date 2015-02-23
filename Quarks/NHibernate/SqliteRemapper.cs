using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Util;

namespace Quarks.NHibernate
{
	/// <summary>
	/// Utility class used for adjusting NHibernate mappings for options not supported by Sqlite.
	/// See http://kozmic.pl/archive/2009/08/17/adjusting-nhibernate-mapping-for-tests.aspx
	/// </summary>
	class SqliteRemapper
	{
		readonly IDictionary<string, string> _map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		internal SqliteRemapper()
		{
			_map.Add("varbinary(max)", "BLOB");
			_map.Add("nvarchar(max)", "TEXT");
			_map.Add("varchar(max)", "TEXT");
		}

		internal virtual void Remap(Configuration configuration)
		{
			// Remap each table in configuration
			configuration.ClassMappings.ForEach(m => remap(m.Table));
		}

		void remap(Table table)
		{
			if (table.Name.Contains("."))
			{
				// found on http://www.nablasoft.com/alkampfer/index.php/2009/07/24/manage-in-memory-nhibernate-test-with-sqlite-and-database-schema
				// this is a table with schema
				table.Name = table.Name.Replace(".", "_");
			}

			// Remap each column
			table.ColumnIterator.ForEach(remap);
		}

		void remap(Column column)
		{
			remapGetDateDefaultValues(column);

			if (string.IsNullOrEmpty(column.SqlType))
				return;

			// Remap SqlTypes as necessary
			string sqlType;
			if (_map.TryGetValue(column.SqlType, out sqlType))
				column.SqlType = sqlType;
		}

		static void remapGetDateDefaultValues(Column column)
		{
			if (string.Compare(column.SqlType, "datetime", StringComparison.OrdinalIgnoreCase) != 0 &&
				string.Compare(column.DefaultValue, "getdate()", StringComparison.OrdinalIgnoreCase) != 0)
				return;

			column.DefaultValue = "CURRENT_TIMESTAMP";
		}
	}
}
