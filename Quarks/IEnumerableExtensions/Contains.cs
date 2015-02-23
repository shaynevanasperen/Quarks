using System.Collections;

namespace Quarks.IEnumerableExtensions
{
	/// <summary>
	/// Extends the IEnumerable interface to include a Contains method.
	/// </summary>
	static partial class EnumerableExtension
	{
		internal static bool Contains(this IEnumerable collection, object item)
		{
			foreach (var element in collection)
				if (element.Equals(item))
					return true;

			return false;
		}
	}
}
