using NHibernate.Sessions.Configuration;
using Remotion.Linq.Utilities;

// ReSharper disable once CheckNamespace
namespace Quarks.FluentNHibernate
{
	static partial class Types
	{
		internal static ITypeFilter ForSessionFactory(string sessionFactoryKey)
		{
			if (string.IsNullOrWhiteSpace(sessionFactoryKey)) throw new ArgumentEmptyException("sessionFactoryKey");
			return FilterableTypeSource.CreateTypeFilter(x => x.IsForSessionFactory(sessionFactoryKey));
		}
	}
}
