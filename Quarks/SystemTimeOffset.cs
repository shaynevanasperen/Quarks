using System;

namespace Quarks
{
	static class SystemTimeOffset
	{
		static Func<DateTimeOffset> _now;
		static Func<DateTimeOffset> _utcNow;

		static SystemTimeOffset()
		{
			Reset();
		}

		internal static DateTimeOffset Now
		{
			get { return _now(); }
			set { _now = () => value; }
		}

		internal static DateTimeOffset UtcNow
		{
			get { return _utcNow(); }
			set { _utcNow = () => value; }
		}

		internal static void Reset()
		{
			_now = () => DateTimeOffset.Now;
			_utcNow = () => DateTimeOffset.UtcNow;
		}
	}
}
