using System;
using AutoMapper;
using Castle.Windsor;
using Machine.Fakes;
using Machine.Specifications;

namespace Quarks.Machine.Fakes.AutoMapper
{
	public abstract class MappingContext<TMappingProfile, TSource, TDestination> : WithSubject<TMappingProfile>
		where TMappingProfile : Profile, new()
	{
		Establish context = () =>
		{
			Container = new WindsorContainer();
			Configure(Container);

			// Ensure Mapper is empty on start
			Mapper.Reset();
			// Obtain all services from the auto mocking container
			Mapper.Configuration.ConstructServicesUsing(x =>
			{
				if (Container.Kernel.HasComponent(x))
					return Container.Resolve(x);

				var defaultContructor = x.GetConstructor(new Type[0]);
				return defaultContructor != null
					? defaultContructor.Invoke(new object[0])
					: Container.Resolve(x);
			});
			MappingProfile = Subject;
			// Make the Profile available via The<TMappingProfile>
			Configure(MappingProfile);
		};

		Cleanup stuff = () =>
		{
			Container.Dispose();
			Container = null;
			Mapper.Reset();
			Destination = default(TDestination);
			Source = default(TSource);
		};

		protected static void AddMappingProfile()
		{
			Mapper.AddProfile(MappingProfile);
		}

		// ReSharper disable once StaticFieldInGenericType
		protected static IWindsorContainer Container;
		protected static TSource Source;
		protected static TDestination Destination;
		protected static TMappingProfile MappingProfile;
	}

	/// <summary>
	/// Provides a base context which loads an AutoMapper profile and maps from a source to a new destination.
	/// </summary>
	public abstract class NewDestinationMappingContext<TMappingProfile, TSource, TDestination> : MappingContext<TMappingProfile, TSource, TDestination>
		where TMappingProfile : Profile, new()
		where TSource : class
	{
		Because of = () =>
		{
			// Initialize Mapper later to allow consumers to provide dependencies for the profile
			AddMappingProfile();

			// Allow consumers to populate Source using Configure<TSource>() or The<TSource>
			if (Source == default(TSource))
				Source = The<TSource>();

			Destination = Mapper.Map<TDestination>(Source);
		};
	}

	/// <summary>
	/// Provides a base context which loads an AutoMapper profile and maps from a source to an existing destination.
	/// </summary>
	public abstract class ExistingDestinationMappingContext<TMappingProfile, TSource, TDestination> : MappingContext<TMappingProfile, TSource, TDestination>
		where TMappingProfile : Profile, new()
		where TSource : class
		where TDestination : class
	{
		Because of = () =>
		{
			// Initialize Mapper later to allow consumers to provide dependencies for the profile
			AddMappingProfile();

			// Allow consumers to populate Destination using Configure<TDestination>();
			if (Destination == default(TDestination))
				Destination = The<TDestination>();
			// Allow consumers to populate Source using Configure<TSource>() or The<TSource>
			if (Source == default(TSource))
				Source = The<TSource>();

			if (Destination == default(TDestination))
				throw new InvalidOperationException(string.Format(
					"Destination of '{0}' must be set for ExistingDestinationMappingContext",
					typeof(TDestination).Name));
			if (Destination.GetType().Namespace == "Castle.Proxies")
				throw new InvalidOperationException(string.Format(
					"Destination of '{0}' cannot be a dynamic proxy for ExistingDestinationMappingContext, have you set Destination or configured The<{0}>?",
					typeof(TDestination).Name));

			Destination = Mapper.Map(Source, Destination);
		};
	}

	public abstract class MappingProfileContext<T> where T : Profile, new()
	{
		protected static void AssertValidConfiguration()
		{
			exception.ShouldBeNull();
		}

		Establish context = () =>
			Mapper.Initialize(x => x.AddProfile<T>());

		Because of = () =>
			exception = Catch.Exception(Mapper.AssertConfigurationIsValid);

		Cleanup after = () =>
			Mapper.Reset();

		// ReSharper disable once StaticFieldInGenericType
		static Exception exception;
	}
}
