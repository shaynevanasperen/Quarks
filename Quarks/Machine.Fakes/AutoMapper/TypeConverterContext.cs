using AutoMapper;
using Machine.Fakes;
using Machine.Specifications;

namespace Quarks.Machine.Fakes.AutoMapper
{
	public abstract class TypeConverterContext<TConverter, TSource, TResult> : WithSubject<TConverter>
		where TConverter : TypeConverter<TSource, TResult>
	{
		Because of = () =>
			Result = Subject.Convert(ResolutionContext.New(Source));

		protected static TSource Source;
		protected static TResult Result;
	}
}
