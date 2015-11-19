using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Quarks
{
	class Retrier : IRetrier
	{
		public virtual void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
		{
			Do<object>(() =>
			{
				action();
				return null;
			}, retryInterval, retryCount);
		}

		public virtual async Task DoAsync(Func<Task> action, TimeSpan retryInterval, int retryCount = 3)
		{
			await DoAsync<object>(async () =>
			{
				await action().ConfigureAwait(false);
				return null;
			}, retryInterval, retryCount).ConfigureAwait(false);
		}

		public virtual T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
		{
			var exceptions = new List<Exception>();

			for (var count = 1; count <= retryCount; count++)
			{
				try
				{
					return action();
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
					if (count < retryCount)
						Thread.Sleep(retryInterval);
				}
			}

			throw new AggregateException(exceptions);
		}

		public virtual async Task<T> DoAsync<T>(Func<Task<T>> func, TimeSpan retryInterval, int retryCount = 3)
		{
			var exceptions = new List<Exception>();

			for (var count = 1; count <= retryCount; count++)
			{
				try
				{
					return await func().ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
					if (count < retryCount)
						Thread.Sleep(retryInterval);
				}
			}

			throw new AggregateException(exceptions);
		}
	}

	internal interface IRetrier
	{
		void Do(Action action, TimeSpan retryInterval, int retryCount = 3);
		Task DoAsync(Func<Task> action, TimeSpan retryInterval, int retryCount = 3);
		T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3);
		Task<T> DoAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int retryCount = 3);
	}
}
