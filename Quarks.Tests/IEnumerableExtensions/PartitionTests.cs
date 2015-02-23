using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests.IEnumerableExtensions
{
	[Subject(typeof(EnumerableExtension))]
	class When_partitioning_large_collection
	{
		It should_provide_the_correct_number_of_chunks = () =>
			chunks.Count().ShouldEqual(CollectionSize / ChunkSize);

		It should_partition_chunks_correctly = () =>
		{
			var count = 0;
			foreach (var chunk in chunks.Select(x => x.ToArray()))
			{
				chunk.Count().ShouldEqual(ChunkSize);
				foreach (var item in chunk) item.ShouldEqual(count++);
			}
		};

		Because of = () =>
			chunks = largeCollection.Partition(ChunkSize);

		Establish context = () =>
			largeCollection = Enumerable.Range(0, CollectionSize);

		const int CollectionSize = 100000;
		const int ChunkSize = 1000;

		static IEnumerable<int> largeCollection;
		static IEnumerable<IEnumerable<int>> chunks;
	}
}