using System.Collections.Generic;
using System.Data;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using Quarks.NHibernate.UserTypes;

namespace Quarks.Tests.NHibernate.UserTypes
{
	[Subject(typeof(EnumerableOfStringType))]
	class When_encoding_a_string_for_persistence : WithSubject<EnumerableOfStringType>
	{
		It should_join_the_strings_using_cr_lf_delimiter = () =>
		{
			Subject.NullSafeSet(dbCommand, new[] { "line1", "line2" }, 0);
			commandParameter.Value.ShouldEqual("line1\r\nline2");
		};

		It should_include_blank_lines = () =>
		{
			Subject.NullSafeSet(dbCommand, new[] { "line1", "", "line2" }, 0);
			commandParameter.Value.ShouldEqual("line1\r\n\r\nline2");
		};

		It should_use_blank_for_zero_length_array = () =>
		{
			Subject.NullSafeSet(dbCommand, new string[0], 0);
			commandParameter.Value.ShouldEqual("");
		};

		Establish context = () =>
		{
			dbCommand = An<IDbCommand>();
			commandParameter = new FakeDataParameter();
			var parameters = An<IDataParameterCollection>();
			parameters.WhenToldTo(x => x[0]).Return(commandParameter);
			dbCommand.WhenToldTo(x => x.Parameters).Return(parameters);
		};

		static IDbCommand dbCommand;
		static IDataParameter commandParameter;
	}

	[Subject(typeof(EnumerableOfStringType))]
	class When_decoding_a_string_from_persistence : WithSubject<EnumerableOfStringType>
	{
		It should_split_the_strings_at_cr_lf_delimiter = () =>
		{
			testValue = "line1\r\nline2";
			var value = Subject.NullSafeGet(dataReader, new[] { "column" }, null);

			value.ShouldNotBeNull();
			value.ShouldBeAssignableTo<IEnumerable<string>>();
			var strings = value as IEnumerable<string>;
			// ReSharper disable PossibleMultipleEnumeration
			// ReSharper disable AssignNullToNotNullAttribute
			strings.Count().ShouldEqual(2);
			strings.First().ShouldEqual("line1");
			strings.ElementAt(1).ShouldEqual("line2");
			// ReSharper restore PossibleMultipleEnumeration
			// ReSharper restore AssignNullToNotNullAttribute
		};

		It should_include_blank_lines = () =>
		{
			testValue = "line1\r\nline2\r\n\r\n";
			var value = Subject.NullSafeGet(dataReader, new[] { "column" }, null);

			value.ShouldNotBeNull();
			value.ShouldBeAssignableTo<IEnumerable<string>>();
			var strings = value as IEnumerable<string>;
			// ReSharper disable PossibleMultipleEnumeration
			// ReSharper disable AssignNullToNotNullAttribute
			strings.Count().ShouldEqual(4);
			strings.First().ShouldEqual("line1");
			strings.ElementAt(1).ShouldEqual("line2");
			strings.ElementAt(2).ShouldBeEmpty();
			strings.ElementAt(3).ShouldBeEmpty();
			// ReSharper restore PossibleMultipleEnumeration
			// ReSharper restore AssignNullToNotNullAttribute
		};

		It should_return_an_empty_enumerable_for_null = () =>
		{
			testValue = null;
			var value = Subject.NullSafeGet(dataReader, new[] { "column" }, null);

			value.ShouldNotBeNull();
			value.ShouldBeAssignableTo<IEnumerable<string>>();
			var strings = value as IEnumerable<string>;
			// ReSharper disable PossibleMultipleEnumeration
			// ReSharper disable AssignNullToNotNullAttribute
			strings.Any().ShouldBeFalse();
			// ReSharper restore PossibleMultipleEnumeration
			// ReSharper restore AssignNullToNotNullAttribute
		};

		Establish context = () =>
		{
			dataReader = An<IDataReader>();
			// Requirements for NHibernateUtil.String.NullSafeGet(rs, column)
			dataReader.WhenToldTo(x => x.GetOrdinal(Param.IsAny<string>())).Return(0);
			dataReader.WhenToldTo(x => x.IsDBNull(0)).Return(() => testValue == null);
			dataReader.WhenToldTo(x => x[0]).Return(() => testValue);
		};

		static IDataReader dataReader;
		static string testValue;
	}

	class FakeDataParameter : IDataParameter
	{
		public DbType DbType { get; set; }
		public ParameterDirection Direction { get; set; }
		public bool IsNullable { get; set; }
		public string ParameterName { get; set; }
		public string SourceColumn { get; set; }
		public DataRowVersion SourceVersion { get; set; }
		public object Value { get; set; }
	}
}
