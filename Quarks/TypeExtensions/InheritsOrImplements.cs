using System;
using System.Linq;

namespace Quarks.TypeExtensions
{
	static partial class TypeExtension
	{
		/// <summary>
		/// Returns <c>true</c> if the type either derives from the given type (if the given type is a class or struct),
		/// or implements the given interface, whether it be directly or indirectly through its inheritance hierarchy.
		/// </summary>
		/// <param name="child">The type being evaluated for the result.</param>
		/// <typeparam name="TParent">The type to compare to.</typeparam>
		internal static bool InheritsOrImplements<TParent>(this Type child)
		{
			return InheritsOrImplements(child, typeof(TParent));
		}

		/// <summary>
		/// Returns <c>true</c> if the type either derives from the given type (if the given type is a class or struct),
		/// or implements the given interface, whether it be directly or indirectly through its inheritance hierarchy.
		/// </summary>
		/// <param name="child">The type being evaluated for the result.</param>
		/// <param name="parent">The type to compare to.</param>
		internal static bool InheritsOrImplements(this Type child, Type parent)
		{
			if (child == null || parent == null)
				return false;

			parent = resolveGenericTypeDefinition(parent);

			if (parent.IsAssignableFrom(child))
				return true;

			var currentChild = child.IsGenericType
				? child.GetGenericTypeDefinition()
				: child;

			while (currentChild != typeof(object))
			{
				if (parent == currentChild || hasAnyInterfaces(parent, currentChild))
					return true;

				currentChild = currentChild.BaseType != null && currentChild.BaseType.IsGenericType
					? currentChild.BaseType.GetGenericTypeDefinition()
					: currentChild.BaseType;

				if (currentChild == null)
					return false;
			}

			return false;
		}

		static bool hasAnyInterfaces(Type parent, Type child)
		{
			return child.GetInterfaces().Any(childInterface =>
			{
				var currentInterface = childInterface.IsGenericType
					? childInterface.GetGenericTypeDefinition()
					: childInterface;

				return currentInterface == parent;
			});
		}

		static Type resolveGenericTypeDefinition(Type type)
		{
			var shouldUseGenericType = !(type.IsGenericType && type.GetGenericTypeDefinition() != type);
			if (type.IsGenericType && shouldUseGenericType)
				type = type.GetGenericTypeDefinition();
			return type;
		}
	}
}
