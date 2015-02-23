# region license
// Functional switch statement based on code by Bart De Smet:
// http://bartdesmet.net/blogs/bart/archive/2008/03/30/a-functional-c-type-switch.aspx
# endregion

using System;

namespace Quarks
{
	static class Switch
	{
		internal static Switch<T> On<T>(T testValue)
		{
			return new Switch<T>(testValue);
		}
	}

	static class SwitchExtensions
	{
		internal static Switch<T> Case<T>(this Switch<T> @switch, T compareTo, Action<T> action, bool fallThrough = false)
		{
			return Case(@switch, x => Equals(x, compareTo), action, fallThrough);
		}

		internal static Switch<T> Case<T>(this Switch<T> @switch, Func<T, bool> c, Action<T> action, bool fallThrough = false)
		{
			if (@switch == null) return null;

			if (c(@switch.Test))
			{
				action(@switch.Test);
				return fallThrough ? @switch : null;
			}
			return @switch;
		}

		internal static void Default<T>(this Switch<T> @switch, Action<T> action)
		{
			if (@switch != null)
				action(@switch.Test);
		}
	}

	/// <summary>
	/// Functional switch statement.
	/// </summary>
	/// <typeparam name="T">Type of test object/expression</typeparam>
	class Switch<T>
	{
		internal Switch(T test)
		{
			Test = test;
		}

		internal T Test { get; private set; }
	}
}
