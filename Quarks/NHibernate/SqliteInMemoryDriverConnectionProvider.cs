using System;
using System.Data;
using NHibernate.Connection;

namespace Quarks.NHibernate
{
	/// <summary>
	/// This DriverConnectionProvider can be used with Sqlite In Memory databases to ensure that
	/// a single connection is kept and never closed (so the schema is not lost).
	/// </summary>
	[Serializable]
	public class SqliteInMemoryDriverConnectionProvider : DriverConnectionProvider
	{
		IDbConnection _connection;

		public override IDbConnection GetConnection()
		{
			return _connection ?? (_connection = base.GetConnection());
		}

		public override void CloseConnection(IDbConnection conn)
		{
			// Never close the connection
		}

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposing && _connection != null)
			{
				if (_connection.State == ConnectionState.Open)
					_connection.Close();
				_connection = null;
			}
			base.Dispose(isDisposing);
		}
	}
}

