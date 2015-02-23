using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Machine.Specifications;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Quarks.FluentNHibernate;
using Quarks.FluentNHibernate.Conventions;
using Quarks.NHibernate;
using Quarks.TypeExtensions;
using Environment = NHibernate.Cfg.Environment;

namespace Quarks.Tests.FluentNHibernate.Conventions
{
	public class NHibernateContext : IAssemblyContext
	{
		static readonly Lazy<ISessionFactory> _sessionFactory = new Lazy<ISessionFactory>(buildSessionFactoryAndExportSchema);

		public void OnAssemblyStart()
		{
			// Do nothing until a test actually requires the configuration or session factory
		}

		static ISessionFactory buildSessionFactoryAndExportSchema()
		{
			// ReSharper disable once PossibleNullReferenceException
			// ReSharper disable once AssignNullToNotNullAttribute
			var sessionFactory = Fluently.Configure(new Configuration())
				.Database(SQLiteConfiguration.Standard
					.InMemory()
					.Provider<SqliteInMemoryDriverConnectionProvider>()
					.ShowSql()
					.FormatSql()
					.Raw(Environment.GenerateStatistics, "true"))
				.Mappings(m => m.AutoMappings.Add(AutoMap
					.Source(Types.FromAssemblies(Assembly.GetExecutingAssembly())
						.Where(t => t.Namespace.StartsWith(typeof(NHibernateContext).Namespace) && t.InheritsOrImplements(typeof(IdentityFieldProvider<,>))))
					.IgnoreBase(typeof(IdentityFieldProvider<,>))
					.Conventions.AddAssembly(typeof(AbbreviationAttribute).Assembly)
					.UseOverridesFromAssembly(Assembly.GetExecutingAssembly())))
				.Diagnostics(x => x.Enable().OutputToConsole())
				.ExposeConfiguration(x =>
				{
					new SqliteRemapper().Remap(x);
					_configuration = x;
				})
				.BuildSessionFactory();

			using (var debug = new DiagnosticsTextWriter())
			{
				debug.WriteLine("Start building Sqlite database schema");
				debug.WriteLine("*************************************");
				using (var session = sessionFactory.OpenSession())
					new SchemaExport(_configuration).Execute(false, true, false, session.Connection, debug);
				debug.WriteLine("*************************************");
				debug.WriteLine("Sqlite database schema build complete");
				debug.WriteLine();
			}

			return sessionFactory;
		}

		public void OnAssemblyComplete()
		{
			if (_sessionFactory.IsValueCreated)
				_sessionFactory.Value.Dispose();
		}

		static Configuration _configuration;
		public static Configuration Configuration
		{
			get
			{
				SessionFactory = _sessionFactory.Value;
				return _configuration;
			}
		}

		static ISessionFactory _initializedSessionFactory;
		public static ISessionFactory SessionFactory
		{
			get { return _initializedSessionFactory ?? (_initializedSessionFactory = _sessionFactory.Value); }
			private set { _initializedSessionFactory = value; }
		}
	}
}
