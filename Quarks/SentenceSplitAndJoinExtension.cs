using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Quarks
{
	static class SentenceSplitAndJoinExtension
	{
		const string SplitWordAndPunctuationPattern = @"(\s|,|\.)";
		static readonly string[] punctuationWithoutSpacePrefix = { ",", ".", "!", "?" };

		/// <summary>
		/// Split a string into words and punctuation characters so it can be rebuilt. Whitespace is lost.
		/// </summary>
		internal static IEnumerable<string> SplitWordsAndPunctuation(this string source)
		{
			return Regex.Split(source, SplitWordAndPunctuationPattern)
				.Where(word => !string.IsNullOrWhiteSpace(word))
				.ToList();
		}

		internal static string JoinWordsAndPunctuation(this IEnumerable<string> source)
		{
			var sentence = new StringBuilder();

			foreach (var word in source.Where(word => !string.IsNullOrWhiteSpace(word))
				.Select(word => word.Trim()))
				appendWordToSentence(sentence, word);

			return sentence.ToString();
		}

		static void appendWordToSentence(StringBuilder sentence, string word)
		{
			if (sentence.Length > 0 && !punctuationWithoutSpacePrefix.Contains(word))
				sentence.Append(' ');
			sentence.Append(word);
		}
	}
}
