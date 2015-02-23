using System;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Sessions.Configuration;

namespace Quarks.NHibernate.Sessions
{
	static class BeginConfigurationExtension
	{
		internal static IPartialConfiguration Using(this IBeginConfiguration extended, Action<SessionFactoryFluentConfiguration> configuration)
		{
			return extended.Using(x =>
			{
				var fluentConfiguration = Fluently.Configure(new Configuration());
				configuration(new SessionFactoryFluentConfiguration
				{
					FluentConfiguration = fluentConfiguration,
					SessionFactoryKey = x
				});
				return fluentConfiguration.BuildConfiguration();
			});
		}
	}

	class SessionFactoryFluentConfiguration
	{
		internal FluentConfiguration FluentConfiguration { get; set; }
		internal string SessionFactoryKey { get; set; }
	}
}
