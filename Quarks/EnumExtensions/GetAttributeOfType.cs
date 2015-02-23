using System;

namespace Quarks.EnumExtensions
{
	static partial class EnumExtension
	{
		/// <summary>
		/// Gets an attribute on an enum field value
		/// </summary>
		/// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
		/// <param name="enumVal">The enum value</param>
		/// <returns>The attribute of type T that exists on the enum value or null if it does not exist</returns>
		internal static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
		{
			var type = enumVal.GetType();
			var memberInfo = type.GetMember(enumVal.ToString());
			var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
			if (attributes.Length == 0)
				return null;

			return (T)attributes[0];
		}
	}
}
