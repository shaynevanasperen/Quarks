using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quarks
{
	public class Page<T> : IPage<T>
	{
		readonly List<T> _internalList;

		public Page(IQueryable<T> source, int pageNumber, int pageSize)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (pageNumber <= 0) throw new ArgumentOutOfRangeException("pageNumber", "pageNumber must be greater than zero");
			if (pageSize <= 0) throw new ArgumentOutOfRangeException("pageNumber", "pageSize must be greater than zero");

			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalCount = source.Count();

			_internalList = new List<T>();
			_internalList.AddRange(source.Skip((PageNumber - 1) * PageSize).Take(PageSize));
		}

		public Page(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (pageNumber <= 0) throw new ArgumentOutOfRangeException("pageNumber", "pageNumber must be greater than zero");
			if (pageSize <= 0) throw new ArgumentOutOfRangeException("pageNumber", "pageSize must be greater than zero");

			_internalList = source.ToList();
			if (totalCount < _internalList.Count) throw new ArgumentOutOfRangeException("totalCount", "totalCount must be greater than or equal to source.Count");

			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalCount = totalCount;
		}

		public int PageNumber { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public int TotalPages
		{
			get { return (int)Math.Ceiling(TotalCount / (double)PageSize); }
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 1; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages; }
		}

		public int FirstItemIndex
		{
			get { return ((PageNumber - 1) * PageSize) + 1; }
		}

		public int LastItemIndex
		{
			get { return FirstItemIndex + _internalList.Count - 1; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public int Count
		{
			get { return _internalList.Count; }
		}

		public T this[int index]
		{
			get { return _internalList[index]; }
		}
	}

	public static class GetPageExtension
	{
		public static IPage<T> GetPage<T>(this IQueryable<T> source, int pageNumber, int pageSize)
		{
			return new Page<T>(source, pageNumber, pageSize);
		}

		public static IPage<T> GetPage<T>(this IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
		{
			return new Page<T>(source, pageNumber, pageSize, totalCount);
		}
	}

	public interface IPage<out T> : IReadOnlyList<T>, IPagination { }

	public interface IPagination
	{
		int PageNumber { get; }
		int PageSize { get; }
		int TotalCount { get; }
		int TotalPages { get; }
		int FirstItemIndex { get; }
		int LastItemIndex { get; }
		bool HasPreviousPage { get; }
		bool HasNextPage { get; }
	}
}
