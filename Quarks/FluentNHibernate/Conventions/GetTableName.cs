using System;

namespace Quarks.FluentNHibernate.Conventions
{
	static partial class TypeExtension
	{
		internal static string GetTableName(this Type entityType)
		{
			// Remove "I" prefix from Interface names
			var entity = (entityType.IsInterface && entityType.Name.StartsWith("I"))
				? entityType.Name.Substring(1)
				: entityType.Name;

			return entity;
		}
	}
}
