using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Diagnostics;

namespace Quarks.FluentNHibernate
{
	class FilterableTypeSource : IFilterableTypeSource, ITypeFilter
	{
		ITypeSource _innerTypeSource;
		Func<Type, bool> _predicate;

		FilterableTypeSource() { }

		internal FilterableTypeSource(ITypeSource innerTypeSource)
		{
			if (innerTypeSource == null) throw new ArgumentNullException("innerTypeSource");

			_innerTypeSource = innerTypeSource;
			_predicate = anyType;
		}

		internal static ITypeFilter CreateTypeFilter(Func<Type, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");

			return new FilterableTypeSource { _predicate = predicate };
		}

		public ITypeSource Where(Func<Type, bool> predicate)
		{
			_predicate = predicate;
			return this;
		}

		public string GetIdentifier()
		{
			return _innerTypeSource.GetIdentifier();
		}

		public IEnumerable<Type> GetTypes()
		{
			return _innerTypeSource.GetTypes().Where(_predicate);
		}

		public void LogSource(IDiagnosticLogger logger)
		{
			_innerTypeSource.LogSource(logger);
		}

		static bool anyType(Type type)
		{
			return true;
		}

		public ITypeSource FromAssemblies(params Assembly[] assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException("assemblies");
			return FromTypeSource(new CombinedAssemblyTypeSource(assemblies));
		}

		public ITypeSource FromTypeSource(ITypeSource typeSource)
		{
			if (typeSource == null) throw new ArgumentNullException("typeSource");
			_innerTypeSource = typeSource;
			return this;
		}
	}

	interface IFilterableTypeSource : ITypeSource
	{
		ITypeSource Where(Func<Type, bool> predicate);
	}

	interface ITypeFilter
	{
		ITypeSource FromAssemblies(params Assembly[] assemblies);
		ITypeSource FromTypeSource(ITypeSource typeSource);
	}

	static partial class Types
	{
		internal static IFilterableTypeSource FromAssemblies(params Assembly[] assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException("assemblies");
			return FromTypeSource(new CombinedAssemblyTypeSource(assemblies));
		}

		internal static IFilterableTypeSource FromTypeSource(ITypeSource typeSource)
		{
			if (typeSource == null) throw new ArgumentNullException("typeSource");
			return new FilterableTypeSource(typeSource);
		}
	}
}
