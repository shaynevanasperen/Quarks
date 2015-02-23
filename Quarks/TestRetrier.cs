using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quarks
{
	class TestRetrier : Retrier
	{
		internal class RetryInvocation
		{
			internal RetryInvocation(Delegate @delegate, TimeSpan retryInterval, int retryCount)
			{
				Delegate = @delegate;
				RetryInterval = retryInterval;
				RetryCount = retryCount;
			}

			internal Delegate Delegate { get; private set; }
			internal TimeSpan RetryInterval { get; private set; }
			internal int RetryCount { get; private set; }
		}

		readonly IList<RetryInvocation> _syncVoidDoInvocations = new List<RetryInvocation>();
		internal IEnumerable<RetryInvocation> SyncVoidDoInvocations
		{
			get { return _syncVoidDoInvocations.AsEnumerable(); }
		}

		readonly IList<RetryInvocation> _asyncVoidDoInvocations = new List<RetryInvocation>();
		internal IEnumerable<RetryInvocation> AsyncVoidDoInvocations
		{
			get { return _asyncVoidDoInvocations.AsEnumerable(); }
		}

		readonly IList<RetryInvocation> _syncNonvoidDoInvocations = new List<RetryInvocation>();
		internal IEnumerable<RetryInvocation> SyncNonvoidDoInvocations
		{
			get { return _syncNonvoidDoInvocations.AsEnumerable(); }
		}

		readonly IList<RetryInvocation> _asyncNonvoidDoInvocations = new List<RetryInvocation>();
		internal IEnumerable<RetryInvocation> AsyncNonvoidDoInvocations
		{
			get { return _asyncNonvoidDoInvocations.AsEnumerable(); }
		}

		public override void Do(Action action, TimeSpan retryInterval, int retryCount = 3)
		{
			_syncVoidDoInvocations.Add(new RetryInvocation(action, retryInterval, retryCount));
			base.Do(action, TimeSpan.FromSeconds(0), retryCount);
		}

		public override Task DoAsync(Func<Task> action, TimeSpan retryInterval, int retryCount = 3)
		{
			_asyncVoidDoInvocations.Add(new RetryInvocation(action, retryInterval, retryCount));
			return base.DoAsync(action, TimeSpan.FromSeconds(0), retryCount);
		}

		public override T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
		{
			_syncNonvoidDoInvocations.Add(new RetryInvocation(action, retryInterval, retryCount));
			return base.Do(action, TimeSpan.FromSeconds(0), retryCount);
		}

		public override Task<T> DoAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int retryCount = 3)
		{
			_asyncNonvoidDoInvocations.Add(new RetryInvocation(action, retryInterval, retryCount));
			return base.DoAsync(action, TimeSpan.FromSeconds(0), retryCount);
		}
	}
}