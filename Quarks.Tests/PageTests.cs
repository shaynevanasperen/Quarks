using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace Quarks.Tests
{
	static class PageConstants
	{
		internal const int TestPageSize = 11;
		internal const int TestNumberOfItems = 100;
	}

	class PageItem : IEquatable<PageItem>
	{
		public string Name { get; set; }

		public bool Equals(PageItem other)
		{
			return other.Name == Name;
		}
	}

	[Behaviors]
	class ReturnsCorrectItemsFromList
	{
		// To avoid compiler warnings
		public ReturnsCorrectItemsFromList()
		{
			Subject = null;
			Source = null;
			PageNumber = 0;
			PageSize = 0;
		}

		It should_return_correct_items_from_the_list = () =>
		{
			var items = Source.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
			items.Count.ShouldEqual(Subject.Count());
			var enumerator = Subject.GetEnumerator();
			foreach (var item in items)
			{
				enumerator.MoveNext();
				item.ShouldEqual(enumerator.Current);
			}
		};

		protected static Page<PageItem> Subject;
		protected static IQueryable<PageItem> Source;
		protected static int PageNumber;
		protected static int PageSize;
	}

	abstract class PageTest
	{
		protected static int PageNumber;
		protected static int PageSize;
		protected static IQueryable<PageItem> Source;
		protected static Page<PageItem> Subject;

		Because of = () =>
		{
			Source = Enumerable.Range(1, PageConstants.TestNumberOfItems).Select(x => new PageItem { Name = "PageItem" + x }).AsQueryable();
			Subject = new Page<PageItem>(Source, PageNumber, PageSize);
		};
	}

	#pragma warning disable 0169 // For MSpec behaviour fields
	[Subject(typeof(Page<>))]
	class When_page_created_with_page_1 : PageTest
	{
		Behaves_like<ReturnsCorrectItemsFromList> returns_correct_items_from_list;

		It should_return_the_correct_total_count = () =>
			Subject.TotalCount.ShouldEqual(Source.Count());

		It should_return_total_pages_as_total_count_divided_by_page_size = () =>
			Subject.TotalPages.ShouldEqual((int)Math.Ceiling(Source.Count() / (double)PageSize));

		It should_not_have_a_previous_page = () =>
			Subject.HasPreviousPage.ShouldBeFalse();

		Establish context = () =>
		{
			PageNumber = 1;
			PageSize = PageConstants.TestPageSize;
		};
	}

	[Subject(typeof(Page<>))]
	class When_page_created_with_middle_page : PageTest
	{
		Behaves_like<ReturnsCorrectItemsFromList> returns_correct_items_from_list;

		It should_have_a_previous_page = () =>
			Subject.HasPreviousPage.ShouldBeTrue();

		It should_have_a_next_page = () =>
			Subject.HasNextPage.ShouldBeTrue();

		Establish context = () =>
		{
			PageNumber = 2;
			PageSize = PageConstants.TestPageSize;
		};
	}

	[Subject(typeof(Page<>))]
	class When_page_created_with_last_page : PageTest
	{
		Behaves_like<ReturnsCorrectItemsFromList> returns_correct_items_from_list;

		It should_have_a_previous_page = () =>
			Subject.HasPreviousPage.ShouldBeTrue();

		It should_not_have_a_next_page = () =>
			Subject.HasNextPage.ShouldBeFalse();

		Establish context = () =>
		{
			PageSize = PageConstants.TestPageSize;
			PageNumber = (int)Math.Ceiling(PageConstants.TestNumberOfItems / (double)PageSize);
		};
	}
	#pragma warning restore 0169

	[Subject(typeof(Page<>))]
	class When_ilist_used_and_total_count_is_less_than_source
	{
		It should_throw_an_out_of_range_exception = () =>
			Catch.Exception(() =>
			{
				var list = new Page<PageItem>(source, 1, 10, source.Count - 1);
			}).ShouldBeOfExactType<ArgumentOutOfRangeException>();

		Because of = () =>
			source = new List<PageItem> { new PageItem(), new PageItem() };

		static List<PageItem> source;
	}
}
