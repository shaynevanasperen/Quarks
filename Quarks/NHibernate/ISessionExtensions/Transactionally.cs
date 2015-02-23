using System;
using NHibernate;

namespace Quarks.NHibernate.ISessionExtensions
{
	static partial class SessionExtension
	{
		public static void Transactionally(this ISession session, Action<ISession> action)
		{
			if (!session.Transaction.IsActive)
				using (var transaction = session.BeginTransaction())
				{
					action(session);
					transaction.Commit();
				}
			else
				action(session);
		}

		public static T Transactionally<T>(this ISession session, Func<ISession, T> func)
		{
			T result;
			if (!session.Transaction.IsActive)
				using (var transaction = session.BeginTransaction())
				{
					result = func(session);
					transaction.Commit();
				}
			else
				result = func(session);

			return result;
		}
	}
}
