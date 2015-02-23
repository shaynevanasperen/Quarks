using System.Collections.Generic;
using Machine.Specifications;
using Quarks.IEnumerableExtensions;

namespace Quarks.Tests
{
	[Subject(typeof(Inflector))]
	class When_using_singularize
	{
		It should_handle_these_cases = () =>
			TestData.Pluralized().ForEach(x => x.Value.Singularize().ShouldEqual(x.Key));

		It should_handle_words_that_are_already_singular = () =>
			"vacuum".Singularize().ShouldEqual("vacuum");
	}

	[Subject(typeof(Inflector))]
	class When_using_pluralize
	{
		It should_handle_these_cases = () =>
			TestData.Pluralized().ForEach(x => x.Key.Pluralize().ShouldEqual(x.Value));

		It should_handle_words_that_are_already_plural = () =>
			"microwaves".Pluralize().ShouldEqual("microwaves");
	}

	partial class TestData
	{
		public static IDictionary<string, string> Pluralized()
		{
			return new Dictionary<string, string>
			{
				{ "search", "searches" },
				{ "switch", "switches" },
				{ "fix", "fixes" },
				{ "box", "boxes" },
				{ "process", "processes" },
				{ "address", "addresses" },
				{ "case", "cases" },
				{ "stack", "stacks" },
				{ "wish", "wishes" },
				{ "fish", "fish" },

				{ "category", "categories" },
				{ "query", "queries" },
				{ "ability", "abilities" },
				{ "agency", "agencies" },
				{ "movie", "movies" },

				{ "archive", "archives" },

				{ "index", "indices" },

				{ "wife", "wives" },
				{ "safe", "saves" },
				{ "half", "halves" },

				{ "move", "moves" },

				{ "salesperson", "salespeople" },
				{ "person", "people" },

				{ "spokesman", "spokesmen" },
				{ "man", "men" },
				{ "woman", "women" },

				{ "basis", "bases" },
				{ "diagnosis", "diagnoses" },

				{ "datum", "data" },
				{ "medium", "media" },
				{ "analysis", "analyses" },

				{ "node_child", "node_children" },
				{ "child", "children" },

				{ "experience", "experiences" },
				{ "day", "days" },

				{ "comment", "comments" },
				{ "foobar", "foobars" },
				{ "newsletter", "newsletters" },

				{ "old_news", "old_news" },
				{ "news", "news" },

				{ "series", "series" },
				{ "species", "species" },

				{ "quiz", "quizzes" },

				{ "perspective", "perspectives" },

				{ "ox", "oxen" },
				{ "photo", "photos" },
				{ "buffalo", "buffaloes" },
				{ "tomato", "tomatoes" },
				{ "dwarf", "dwarves" },
				{ "elf", "elves" },
				{ "information", "information" },
				{ "equipment", "equipment" },
				{ "bus", "buses" },
				{ "status", "statuses" },
				{ "status_code", "status_codes" },
				{ "mouse", "mice" },

				{ "louse", "lice" },
				{ "house", "houses" },
				{ "octopus", "octopi" },
				{ "virus", "viri" },
				{ "alias", "aliases" },
				{ "portfolio", "portfolios" },

				{ "vertex", "vertices" },
				{ "matrix", "matrices" },

				{ "axis", "axes" },
				{ "testis", "testes" },
				{ "crisis", "crises" },

				{ "rice", "rice" },
				{ "shoe", "shoes" },

				{ "horse", "horses" },
				{ "prize", "prizes" },
				{ "edge", "edges" },

				/* Tests added by Bas Jansen */
				{ "goose", "geese" },
				{ "deer", "deer" },
				{ "sheep", "sheep" },
				{ "wolf", "wolves" },
				{ "volcano", "volcanoes" },
				{ "aircraft", "aircraft" },
				{ "alumna", "alumnae" },
				{ "alumnus", "alumni" },
				{ "fungus", "fungi" },

				/* Multi-word pluralise/singularise */
				{ "cooker & hob", "cookers & hobs" },
				{ "boy and girl", "boys and girls" },
				{ "man, woman and child", "men, women and children" }
			};
		}
	}
}
