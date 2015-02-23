using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Seatwave.Infrastructure.Utils
{
	static partial class NameValueCollectionExtension
	{
		public static IDictionary<string, object> ToDictionary(this NameValueCollection source)
		{
			return source.Cast<string>()
				.Select(s => new { Key = s, Value = source[s] })
				.ToDictionary(p => p.Key, p => (object)p.Value);
		}
	}
}