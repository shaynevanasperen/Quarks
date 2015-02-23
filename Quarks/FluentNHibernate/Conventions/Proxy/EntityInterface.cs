using System;
using System.Linq;
using FluentNHibernate.Conventions;
using Quarks.TypeExtensions;

namespace Quarks.FluentNHibernate.Conventions.Proxy
{
	class EntityInterface : ProxyConvention
	{
		public EntityInterface()
			: base(getEntityInterfaceFromConcreteClass, getConcreteClassFromEntityInterface) { }

		static Type getEntityInterfaceFromConcreteClass(Type concrete)
		{
			// Check the class for interface with the same name, prefixed with "I"
			var iface = concrete
				.GetInterfaces()
				.FirstOrDefault(i => i.Name == "I" + concrete.Name);

			return iface;
		}

		static Type getConcreteClassFromEntityInterface(Type iface)
		{
			// Only support interfaces
			if (!iface.IsInterface)
				return null;
			// Only support interfaces following the IEntityType naming convention
			if (iface.Name[0] != 'I')
				return null;

			// Search all loaded assemblies
			var concrete = AppDomain.CurrentDomain.GetAssemblies()
				// Select all types in the assembly
				.Select(a => a.GetExportedTypes().FirstOrDefault(t => t.IsClass && t.InheritsOrImplements(iface)))
				// Null values returned for assemblies that have no matching class
				.FirstOrDefault(t => t != null);

			return concrete;
		}
	}
}
