using System;

namespace Quarks.StringExtensions
{
	static partial class StringExtension
	{
		internal static bool Contains(this string input, string value, bool caseSensitive)
		{
			if (caseSensitive)
				return input.Contains(value);
			
			return input.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
		}
	}
}