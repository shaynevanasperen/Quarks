using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Quarks
{
	/// <summary>
	/// https://github.com/srkirkland/Inflector
	/// </summary>
	static class Inflector
	{
		static Inflector()
		{
			#region Default Rules

			addPlural("$", "s");
			addPlural("s$", "s");
			addPlural("(ax|test)is$", "$1es");
			addPlural("(octop|vir|alumn|fung)us$", "$1i");
			addPlural("(alias|status)$", "$1es");
			addPlural("(bu)s$", "$1ses");
			addPlural("(buffal|tomat|volcan)o$", "$1oes");
			addPlural("([ti])um$", "$1a");
			addPlural("sis$", "ses");
			addPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
			addPlural("(hive)$", "$1s");
			addPlural("([^aeiouy]|qu)y$", "$1ies");
			addPlural("(x|ch|ss|sh)$", "$1es");
			addPlural("(matr|vert|ind)ix|ex$", "$1ices");
			addPlural("([m|l])ouse$", "$1ice");
			addPlural("^(ox)$", "$1en");
			addPlural("(quiz)$", "$1zes");

			addSingular("s$", "");
			addSingular("(n)ews$", "$1ews");
			addSingular("([ti])a$", "$1um");
			addSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
			addSingular("(^analy)ses$", "$1sis");
			addSingular("([^f])ves$", "$1fe");
			addSingular("(hive)s$", "$1");
			addSingular("(tive)s$", "$1");
			addSingular("([lr])ves$", "$1f");
			addSingular("([^aeiouy]|qu)ies$", "$1y");
			addSingular("(s)eries$", "$1eries");
			addSingular("(m)ovies$", "$1ovie");
			addSingular("(x|ch|ss|sh)es$", "$1");
			addSingular("([m|l])ice$", "$1ouse");
			addSingular("(bus)es$", "$1");
			addSingular("(o)es$", "$1");
			addSingular("(shoe)s$", "$1");
			addSingular("(cris|ax|test)es$", "$1is");
			addSingular("(octop|vir|alumn|fung)i$", "$1us");
			addSingular("(alias|status)es$", "$1");
			addSingular("^(ox)en", "$1");
			addSingular("(vert|ind)ices$", "$1ex");
			addSingular("(matr)ices$", "$1ix");
			addSingular("(quiz)zes$", "$1");

			addIrregular("person", "people");
			addIrregular("man", "men");
			addIrregular("child", "children");
			addIrregular("sex", "sexes");
			addIrregular("move", "moves");
			addIrregular("goose", "geese");
			addIrregular("alumna", "alumnae");

			addUncountable("equipment");
			addUncountable("information");
			addUncountable("rice");
			addUncountable("money");
			addUncountable("species");
			addUncountable("series");
			addUncountable("fish");
			addUncountable("sheep");
			addUncountable("deer");
			addUncountable("aircraft");

			addUncountable("and");
			addUncountable("&amp;");

			#endregion
		}

		private sealed class Rule
		{
			readonly Regex _regex;
			readonly string _replacement;

			public Rule(string pattern, string replacement)
			{
				_regex = new Regex(pattern, RegexOptions.IgnoreCase);
				_replacement = replacement;
			}

			public string Apply(string word)
			{
				return !_regex.IsMatch(word)
					? null
					: _regex.Replace(word, _replacement);
			}
		}

		static void addIrregular(string singular, string plural)
		{
			addPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
			addSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
		}

		static void addUncountable(string word)
		{
			_uncountables.Add(word.ToLower());
		}

		static void addPlural(string rule, string replacement)
		{
			_plurals.Add(new Rule(rule, replacement));
		}

		static void addSingular(string rule, string replacement)
		{
			_singulars.Add(new Rule(rule, replacement));
		}

		/// <summary>
		/// Phrase should be treated as word for pluralise/singularise operations.
		/// </summary>
		static void addPhraseExclusion(string phrase)
		{
			_phraseExclusions.Add(phrase.ToLower());
		}

		static readonly List<Rule> _plurals = new List<Rule>();
		static readonly List<Rule> _singulars = new List<Rule>();
		static readonly List<string> _uncountables = new List<string>();
		static readonly List<string> _phraseExclusions = new List<string>();

		internal static string Pluralize(this string word)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");

			return isPhrase(word)
				? applyRulesToPhrase(_plurals, word)
				: applyRulesToWord(_plurals, word);
		}

		static string applyRulesToWord(IReadOnlyList<Rule> rules, string word)
		{
			return word.Length == 1
				? word
				: applyRules(rules, word);
		}

		static string applyRulesToPhrase(IReadOnlyList<Rule> rules, string phrase)
		{
			return phrase
				.SplitWordsAndPunctuation()
				.Select(word => applyRulesToWord(rules, word))
				.JoinWordsAndPunctuation();
		}

		static bool isPhrase(string word)
		{
			var wordCompare = word.Trim().ToLower();
			return wordCompare.Contains(" ")
				&& !_phraseExclusions.Contains(wordCompare);
		}

		internal static string Singularize(this string word)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");

			return isPhrase(word)
				? applyRulesToPhrase(_singulars, word)
				: applyRulesToWord(_singulars, word);
		}

		static string applyRules(IReadOnlyList<Rule> rules, string word)
		{
			if (_uncountables.Contains(word.ToLower()))
				return word;

			var result = word;
			for (var i = rules.Count - 1; i >= 0; i--)
				if ((result = rules[i].Apply(word)) != null)
					break;

			// No change if no rules were applied
			return result ?? word;
		}

		internal static string Titleize(this string word)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");

			return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
								 match => match.Captures[0].Value.ToUpper());
		}

		internal static string Humanize(this string lowercaseAndUnderscoredWord)
		{
			if (lowercaseAndUnderscoredWord == null) throw new ArgumentNullException("lowercaseAndUnderscoredWord", "cannot be null");

			return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
		}

		internal static string Pascalize(this string lowercaseAndUnderscoredWord)
		{
			if (lowercaseAndUnderscoredWord == null) throw new ArgumentNullException("lowercaseAndUnderscoredWord", "cannot be null");

			return Regex.Replace(lowercaseAndUnderscoredWord, "(?:^|_)(.)",
								 match => match.Groups[1].Value.ToUpper());
		}

		internal static string Camelize(this string lowercaseAndUnderscoredWord)
		{
			if (lowercaseAndUnderscoredWord == null) throw new ArgumentNullException("lowercaseAndUnderscoredWord", "cannot be null");

			return Uncapitalize(Pascalize(lowercaseAndUnderscoredWord));
		}

		internal static string Underscore(this string pascalCasedWord)
		{
			if (pascalCasedWord == null) throw new ArgumentNullException("pascalCasedWord", "cannot be null");

			return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
		}

		internal static string Capitalize(this string word)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");

			return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
		}

		internal static string Uncapitalize(this string word)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");

			return word.Substring(0, 1).ToLower() + word.Substring(1);
		}

		internal static string Ordinalize(this string numberString)
		{
			if (numberString == null) throw new ArgumentNullException("numberString", "cannot be null");

			return ordanize(int.Parse(numberString), numberString);
		}

		internal static string Ordinalize(this int number)
		{
			return ordanize(number, number.ToString(CultureInfo.InvariantCulture));
		}

		static string ordanize(int number, string numberString)
		{
			var nMod100 = number % 100;

			if (nMod100 >= 11 && nMod100 <= 13)
				return numberString + "th";

			switch (number % 10)
			{
				case 1:
					return numberString + "st";
				case 2:
					return numberString + "nd";
				case 3:
					return numberString + "rd";
				default:
					return numberString + "th";
			}
		}

		internal static string Dasherize(this string underscoredWord)
		{
			if (underscoredWord == null) throw new ArgumentNullException("underscoredWord", "cannot be null");

			return underscoredWord.Replace('_', '-');
		}

		internal static string PluralizeForCount(this string word, int count)
		{
			if (word == null) throw new ArgumentNullException("word", "cannot be null");
			if (count < 0) throw new ArgumentNullException("count", "cannot be negative");

			return word.PluralizeForCount((ulong)count);
		}

		internal static string PluralizeForCount(this string word, ulong count)
		{
			return count == 1
				? word
				: Pluralize(word);
		}
	}
}