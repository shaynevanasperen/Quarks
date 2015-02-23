using System;
using Machine.Fakes;

namespace Quarks.Machine.Fakes
{
	class ConfigForASystemTimeOf
	{
		internal ConfigForASystemTimeOf(DateTime fakeTime)
		{
			_fakeTime = fakeTime;
		}

		internal ConfigForASystemTimeOf(int year, int month, int day)
		{
			_fakeTime = new DateTime(year, month, day);
		}

		OnEstablish context = ctx =>
		{
			SystemTime.Now = _fakeTime;
			SystemTime.UtcNow = _fakeTime;
			SystemTime.Today = _fakeTime;
		};

		OnCleanup after = ctx => SystemTime.Reset();

		static DateTime _fakeTime;
	}
}
