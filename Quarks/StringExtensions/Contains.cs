using System;

namespace Quarks.StringExtensions
{
	static partial class StringExtension
	{
		internal static bool Contains(this string input, string value, bool caseSensitive)
		{
			if (caseSensitive)
				return input.Contains(value);
			
			return input.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
		}
        internal static bool Contains(this string str, string substring, StringComparison comp)
        {
            //copy from https://docs.microsoft.com/zh-cn/dotnet/api/system.string.contains?view=netframework-4.7.1
            if (substring == null)
                throw new ArgumentNullException(nameof(substring));
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            nameof(comp));

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}