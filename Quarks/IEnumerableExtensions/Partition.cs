using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quarks.IEnumerableExtensions
{
	/// <summary>
	/// Extends the IEnumerable&lt;T&gt; interface to include a Partition method.
	/// </summary>
	static partial class EnumerableExtension
	{
		/// <summary>
		/// Partition extension which provides an easy way to partition a large collection into
		/// manageable chunks.
		/// </summary>
		/// <example>foreach (var chunk in largeCollection.Partition(1000)
		/// {
		///		processChunk(1000);
		/// }</example>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="partitionSize"></param>
		///<returns></returns>
		internal static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> collection, int partitionSize)
		{
			if (partitionSize <= 0) throw new ArgumentOutOfRangeException("partitionSize", "partitionSize must be greater than zero");

			T[] array = null;
			var count = 0;
			foreach (var item in collection)
			{
				if (array == null)
					array = new T[partitionSize];
				array[count++] = item;
				if (count != partitionSize) continue;

				yield return new ReadOnlyCollection<T>(array);
				array = null;
				count = 0;
			}

			if (array != null)
			{
				Array.Resize(ref array, count);
				yield return new ReadOnlyCollection<T>(array);
			}
		}
	}
}
