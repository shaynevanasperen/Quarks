﻿using System;
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
		readonly Func<Type, bool> _predicate;

		FilterableTypeSource(Func<Type, bool> predicate)
		{
			_predicate = predicate ?? anyType;
		}

		internal FilterableTypeSource(ITypeSource innerTypeSource, Func<Type, bool> predicate = null) : this(predicate)
		{
			if (innerTypeSource == null) throw new ArgumentNullException("innerTypeSource");

			_innerTypeSource = innerTypeSource;
		}

		internal static ITypeFilter CreateTypeFilter(Func<Type, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");

			return new FilterableTypeSource(predicate);
		}

		public ITypeSource Where(Func<Type, bool> predicate)
		{
			return new FilterableTypeSource(this, predicate);
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

		public IFilterableTypeSource FromAssemblies(params Assembly[] assemblies)
		{
			if (assemblies == null) throw new ArgumentNullException("assemblies");
			return FromTypeSource(new CombinedAssemblyTypeSource(assemblies));
		}

		public IFilterableTypeSource FromTypeSource(ITypeSource typeSource)
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
		IFilterableTypeSource FromAssemblies(params Assembly[] assemblies);
		IFilterableTypeSource FromTypeSource(ITypeSource typeSource);
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
