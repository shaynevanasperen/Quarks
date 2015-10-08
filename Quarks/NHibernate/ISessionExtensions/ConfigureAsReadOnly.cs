using NHibernate;

namespace Quarks.NHibernate.ISessionExtensions
{
	static partial class SessionExtension
	{
		internal static void ConfigureAsReadOnly(this ISession session)
		{
			session.DefaultReadOnly = true;
			session.FlushMode = FlushMode.Never;
		}
	}
}
