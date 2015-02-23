using System;

namespace Quarks
{
	static class SystemTime
	{
		static Func<DateTime> _now;
		static Func<DateTime> _utcNow;
		static Func<DateTime> _today;

		static SystemTime()
		{
			Reset();
		}

		internal static DateTime Now
		{
			get { return _now(); }
			set { _now = () => value; }
		}

		internal static DateTime UtcNow
		{
			get { return _utcNow(); }
			set { _utcNow = () => value; }
		}

		internal static DateTime Today
		{
			get { return _today(); }
			set { _today = () => value; }
		}

		internal static void Reset()
		{
			_now = () => DateTime.Now;
			_utcNow = () => DateTime.UtcNow;
			_today = () => DateTime.Today;
		}
	}
}
