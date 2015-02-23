using System;
using NHibernate;
using NHibernate.Context;

namespace Quarks.NHibernate.ISessionFactoryExtensions
{
	static partial class SessionFactoryExtension
	{
		internal static ISession GetCurrentOrNewSession(this ISessionFactory sessionFactory, Action<ISession> onSessionOpened = null)
		{
			if (CurrentSessionContext.HasBind(sessionFactory))
				return sessionFactory.GetCurrentSession();
			var session = sessionFactory.OpenSession();
			if (onSessionOpened != null)
				onSessionOpened(session);
			CurrentSessionContext.Bind(session);
			return session;
		}
	}
}
