using System;
using System.ComponentModel;
using System.Linq;
using Quarks.TypeExtensions;

namespace Quarks.ObjectExtensions
{
	static partial class ObjectExtension
	{
		internal static T ChangeTypeTo<T>(this object value)
		{
			if (value == DBNull.Value || value == null)
				return default(T);
			return (T)value.ChangeTypeTo(typeof(T));
		}

		internal static object ChangeTypeTo(this object value, Type destinationType)
		{
			if (value == DBNull.Value || value == null)
				return null;

			var sourceType = value.GetType();
			if (destinationType == sourceType) return value;

			// Attempt to use destination type's TypeConverter
			var destConverter = TypeDescriptor.GetConverter(destinationType);
			if (destConverter.CanConvertFrom(value.GetType()))
				return destConverter.ConvertFrom(value);

			// Attempt to use source type's type converter
			var sourceConverter = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter.CanConvertTo(destinationType))
				return sourceConverter.ConvertTo(value, destinationType);

			// Enum requires some extra work.
			if (destinationType.IsEnum)
				return Enum.Parse(destinationType, value.ToString());

			// Nullable<t> types can only be cast from their non-nullable equivalent
			if (destinationType.InheritsOrImplements(typeof(Nullable<>)))
				destinationType = destinationType.GetGenericArguments().First();
			var convertedValue = Convert.ChangeType(value, destinationType);

			// Cast as desired type
			return convertedValue;
		}
	}
}
