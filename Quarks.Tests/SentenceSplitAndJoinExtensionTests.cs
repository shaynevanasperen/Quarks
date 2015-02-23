using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(SentenceSplitAndJoinExtension))]
	class When_splitting_sentences
	{
		It should_keep_single_words = () =>
			"word".SplitWordsAndPunctuation().ShouldEqual(new[] { "word" });

		It should_split_words = () =>
			"word1 word2".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "word2" });

		It should_ignore_leading_space = () =>
			"  word1 word2".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "word2" });

		It should_ignore_trailing_space = () =>
			"word1 word2 ".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "word2" });

		It should_ignore_insignificant_space = () =>
			"word1     word2 ".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "word2" });

		It should_return_punctuation = () =>
			"word1 & word2 ".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "&", "word2" });

		It should_return_punctuation_as_suffix_of_previous_word = () =>
			"word1, word2 ".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", ",", "word2" });

		It should_return_trailing_punctuation = () =>
			"word1 and word2.".SplitWordsAndPunctuation().ShouldEqual(new[] { "word1", "and", "word2", "." });
	}
}
