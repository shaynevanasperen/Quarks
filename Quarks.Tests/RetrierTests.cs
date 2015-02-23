using System;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_void_method_invocation_which_succeeds : WithSubject<Retrier>
	{
		It should_not_retry = () =>
			callCount.ShouldEqual(1);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		Because of = () =>
			exception = Catch.Exception(() => Subject.Do(testMethod, TimeSpan.FromSeconds(1)));

		static void testMethod()
		{
			callCount++;
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_void_method_invocation_which_fails_transiently : WithSubject<Retrier>
	{
		It should_retry_as_many_times_as_there_were_errors = () =>
			(callCount - 1).ShouldEqual(2);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		Because of = () =>
			exception = Catch.Exception(() => Subject.Do(testMethod, TimeSpan.FromMilliseconds(1), 5));

		static void testMethod()
		{
			callCount++;
			if (callCount < 3)
				throw new Exception();
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_void_method_invocation_which_fails_persistently : WithSubject<Retrier>
	{
		It should_retry_the_specified_number_of_times = () =>
			callCount.ShouldEqual(4);

		It should_throw_an_aggregate_exception_containing_each_failure_exception = () =>
		{
			var aggregateException = exception as AggregateException;
			aggregateException.ShouldNotBeNull();
			aggregateException.InnerExceptions.Count.ShouldEqual(4);
		};

		Because of = () =>
			exception = Catch.Exception(() => Subject.Do(testMethod, TimeSpan.FromMilliseconds(1), 4));

		static void testMethod()
		{
			callCount++;
			throw new Exception();
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_non_void_method_invocation_which_succeeds : WithSubject<Retrier>
	{
		It should_not_retry = () =>
			callCount.ShouldEqual(1);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		It should_return_the_result = () =>
			result.ShouldEqual(1);

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.Do<int>(testMethod, TimeSpan.FromSeconds(1)));

		static int testMethod()
		{
			callCount++;
			return 1;
		}

		static int result;
		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_non_void_method_invocation_which_fails_transiently : WithSubject<Retrier>
	{
		It should_retry_as_many_times_as_there_were_errors = () =>
			(callCount - 1).ShouldEqual(2);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		It should_return_the_result = () =>
			result.ShouldEqual(1);

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.Do<int>(testMethod, TimeSpan.FromMilliseconds(1), 5));

		static int testMethod()
		{
			callCount++;
			if (callCount < 3)
				throw new Exception();
			return 1;
		}

		static int result;
		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_a_synchronous_non_void_method_invocation_which_fails_persistently : WithSubject<Retrier>
	{
		It should_retry_the_specified_number_of_times = () =>
			callCount.ShouldEqual(4);

		It should_throw_an_aggregate_exception_containing_each_failure_exception = () =>
		{
			var aggregateException = exception as AggregateException;
			aggregateException.ShouldNotBeNull();
			aggregateException.InnerExceptions.Count.ShouldEqual(4);
		};

		It should_not_return_the_result = () =>
			result.ShouldEqual(0);

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.Do<int>(testMethod, TimeSpan.FromMilliseconds(1), 4));

		static int testMethod()
		{
			callCount++;
			throw new Exception();
		}

		static int result;
		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_void_method_invocation_which_succeeds : WithSubject<Retrier>
	{
		It should_not_retry = () =>
			callCount.ShouldEqual(1);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		Because of = () =>
			exception = Catch.Exception(() => Subject.DoAsync(testMethod, TimeSpan.FromSeconds(1)).Await());

		static async Task testMethod()
		{
			await Task.Run(() => callCount++);
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_void_method_invocation_which_fails_transiently : WithSubject<Retrier>
	{
		It should_retry_as_many_times_as_there_were_errors = () =>
			(callCount - 1).ShouldEqual(2);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		Because of = () =>
			exception = Catch.Exception(() => Subject.DoAsync(testMethod, TimeSpan.FromMilliseconds(1), 5).Wait());

		static async Task testMethod()
		{
			await Task.Run(() =>
			{
				callCount++;
				if (callCount < 3)
					throw new Exception();
			});
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_void_method_invocation_which_fails_persistently : WithSubject<Retrier>
	{
		It should_retry_the_specified_number_of_times_and_then_throw_an_aggregate_exception_containing_each_failure_exception = () =>
		{
			callCount.ShouldEqual(4);
			var aggregateException = exception as AggregateException;
			aggregateException.ShouldNotBeNull();
			aggregateException.InnerExceptions.Count.ShouldEqual(4);
		};

		Because of = () =>
			exception = Catch.Exception(() => Subject.DoAsync(testMethod, TimeSpan.FromMilliseconds(1), 4).Await());

		static async Task testMethod()
		{
			await Task.Run(() =>
			{
				callCount++;
				throw new Exception();
			});
		}

		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_non_void_method_invocation_which_succeeds : WithSubject<Retrier>
	{
		It should_not_retry = () =>
			callCount.ShouldEqual(1);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		It should_return_the_result = () =>
			result.ShouldEqual(1);

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.DoAsync(testMethod, TimeSpan.FromSeconds(1)).Result);

		static async Task<int> testMethod()
		{
			return await Task.Run(() =>
			{
				callCount++;
				return 1;
			});
		}

		static int result;
		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_non_void_method_invocation_which_fails_transiently : WithSubject<Retrier>
	{
		It should_retry_as_many_times_as_there_were_errors = () =>
			(callCount - 1).ShouldEqual(2);

		It should_not_throw_any_exceptions = () =>
			exception.ShouldBeNull();

		It should_return_the_result = () =>
			result.ShouldEqual(1);

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.DoAsync(testMethod, TimeSpan.FromMilliseconds(1), 5).Result);

		static async Task<int> testMethod()
		{
			return await Task.Run(() =>
			{
				callCount++;
				if (callCount < 3)
					throw new Exception();
				return 1;
			});
		}

		static int result;
		static int callCount;
		static Exception exception;
	}

	[Subject(typeof(Retrier))]
	class When_retrying_an_asynchronous_non_void_method_invocation_which_fails_persistently : WithSubject<Retrier>
	{
		It should_retry_the_specified_number_of_times_and_then_throw_an_aggregate_exception_containing_each_failure_exception = () =>
		{
			callCount.ShouldEqual(4);
			var aggregateException = exception as AggregateException;
			aggregateException.ShouldNotBeNull();
			aggregateException.InnerExceptions.Count.ShouldEqual(4);
			result.ShouldEqual(0);
		};

		Because of = () =>
			exception = Catch.Exception(() => result = Subject.DoAsync(testMethod, TimeSpan.FromMilliseconds(1), 4).Await());

		static async Task<int> testMethod()
		{
			return await Task.Run(() =>
			{
				callCount++;
				throw new Exception();
				// ReSharper disable once CSharpWarnings::CS0162
				//return value required otherwise implicit conversion causes compile error
				return 1;
			});
		}

		static int result;
		static int callCount;
		static Exception exception;
	}
}
