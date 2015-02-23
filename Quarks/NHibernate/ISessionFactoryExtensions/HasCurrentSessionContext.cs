using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Quarks.NHibernate.ISessionFactoryExtensions
{
	static partial class SessionFactoryExtension
	{
		internal static bool HasCurrentSessionContext(this ISessionFactory sessionFactory)
		{
			return HasCurrentSessionContext<CurrentSessionContext>(sessionFactory);
		}

		internal static bool HasCurrentSessionContext<T>(this ISessionFactory sessionFactory) where T : CurrentSessionContext
		{
			var sessionFactoryImplementor = sessionFactory as ISessionFactoryImplementor;
			return sessionFactoryImplementor != null &&
				   sessionFactoryImplementor.CurrentSessionContext as T != null;
		}
	}
}
