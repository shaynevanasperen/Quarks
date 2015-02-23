using AutoMapper;
using Machine.Fakes;
using Machine.Specifications;

namespace Quarks.Machine.Fakes.AutoMapper
{
	public abstract class ValueResolverContext<TResolver, TSource, TResult> : WithSubject<TResolver>
		where TResolver : ValueResolver<TSource, TResult>
	{
		Because of = () =>
			Result = (TResult)(Subject.Resolve(new ResolutionResult(ResolutionContext.New(Source))).Value);

		protected static TSource Source;
		protected static TResult Result;
	}
}
