using System;
using Machine.Specifications;

namespace Quarks.Tests
{
	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_references_to_the_same_entity
	{
		It should_consider_them_equal_with_equality_operator = () =>
			(original == secondReference).ShouldBeTrue();

		It should_consider_them_equal_with_equality_operator_reversed = () =>
			(secondReference == original).ShouldBeTrue();

		It should_return_false_for_inequality_operator = () =>
			(original != secondReference).ShouldBeFalse();

		It should_return_false_for_inequality_operator_reversed = () =>
			(secondReference != original).ShouldBeFalse();

		It should_return_ture_for_equals_method = () =>
			(original.Equals(secondReference)).ShouldBeTrue();

		It should_return_the_same_hashcodes = () =>
			original.GetHashCode().ShouldEqual(secondReference.GetHashCode());

		Establish context = () =>
			original = new TestEntityTypeBase(new Random().Next());

		Because of = () =>
			secondReference = original;

		static IEntity original;
		static IEntity secondReference;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_entities_of_same_the_type_with_the_same_id
	{
		It should_return_true_for_the_equality_operator = () =>
			(original == other).ShouldBeTrue();

		It should_return_true_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeTrue();

		It should_return_false_for_the_inequality_operator = () =>
			(original != other).ShouldBeFalse();

		It should_return_false_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeFalse();

		It should_return_true_for_the_equals_method = () =>
			original.Equals(other).ShouldBeTrue();

		It should_return_the_same_hashcodes = () =>
			original.GetHashCode().ShouldEqual(other.GetHashCode());

		Establish context = () =>
			original = new TestEntityTypeBase(new Random().Next());

		Because of = () =>
			other = new TestEntityTypeBase(original.Id);

		static TestEntityTypeBase original;
		static TestEntityTypeBase other;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_entities_of_same_the_type_with_a_different_id
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == other).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != other).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			original.Equals(other).ShouldBeFalse();

		It should_return_the_different_hashcodes = () =>
			original.GetHashCode().ShouldNotEqual(other.GetHashCode());

		Establish context = () =>
			original = new TestEntityTypeBase(new Random().Next());

		Because of = () =>
			other = new TestEntityTypeBase(original.Id + 1);

		static TestEntityTypeBase original;
		static TestEntityTypeBase other;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_entities_of_different_type_with_a_different_id
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == other).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != other).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			original.Equals(other).ShouldBeFalse();

		Establish context = () =>
		{
			id = new Random().Next();
			original = new TestEntityTypeBase(id);
		};

		Because of = () =>
			other = new TestEntityTypeNew(id + 1);

		static IEntity original;
		static IEntity other;
		static int id;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_entities_of_different_type_with_the_same_id
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == other).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != other).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			original.Equals(other).ShouldBeFalse();

		Establish context = () =>
		{
			id = new Random().Next();
			original = new TestEntityTypeBase(id);
		};

		Because of = () =>
			other = new TestEntityTypeNew(id);

		static IEntity original;
		static IEntity other;
		static int id;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_entities_of_derived_type_with_the_a_different_id
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == derived).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(derived == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != derived).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(derived != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method_of_the_parent_type = () =>
			original.Equals(derived).ShouldBeFalse();

		It should_return_false_for_the_equals_method_of_the_derived_type = () =>
			(derived.Equals(original)).ShouldBeFalse();

		It should_return_different_hashcodes = () =>
			original.GetHashCode().ShouldNotEqual(derived.GetHashCode());

		Establish context = () =>
		{
			id = new Random().Next();
			original = new TestEntityTypeBase(id);
		};

		Because of = () =>
			derived = new TestEntityTypeDerived(id + 1);

		static IEntity original;
		static IEntity derived;
		static int id;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_entities_of_derived_type_with_the_same_id
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == derived).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(derived == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != derived).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(derived != original).ShouldBeTrue();

		It should_return_true_for_the_equals_method_of_the_parent_type = () =>
			(original.Equals(derived)).ShouldBeTrue();

		It should_return_true_for_the_equals_method_of_the_derived_type = () =>
			(derived.Equals(original)).ShouldBeTrue();

		It should_return_the_same_hashcodes = () =>
			original.GetHashCode().ShouldEqual(derived.GetHashCode());

		Establish context = () =>
		{
			id = new Random().Next();
			original = new TestEntityTypeBase(id);
		};

		Because of = () =>
			derived = new TestEntityTypeDerived(id);

		static IEntity original;
		static IEntity derived;
		static int id;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_references_to_the_same_transient_entity_instance
	{
		It should_return_true_for_the_equality_operator = () =>
			(original == secondReference).ShouldBeTrue();

		It should_return_true_for_the_equality_operator_reversed = () =>
			(secondReference == original).ShouldBeTrue();

		It should_return_false_for_the_inequality_operator = () =>
			(original != secondReference).ShouldBeFalse();

		It should_return_false_for_the_inequality_operator_reversed = () =>
			(secondReference != original).ShouldBeFalse();

		It should_return_true_for_the_equals_method = () =>
			original.Equals(secondReference).ShouldBeTrue();

		It should_return_the_same_hashcodes = () =>
			original.GetHashCode().ShouldEqual(secondReference.GetHashCode());

		Establish context = () =>
			original = new TestEntityTypeBase();

		Because of = () =>
			secondReference = original;

		static IEntity original;
		static IEntity secondReference;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_transient_entities_of_the_same_type
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == other).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != other).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			(original.Equals(other)).ShouldBeFalse();

		It should_return_different_hashcodes = () =>
			original.GetHashCode().ShouldNotEqual(other.GetHashCode());

		Because of = () =>
		{
			original = new TestEntityTypeBase();
			other = new TestEntityTypeBase();
		};

		static IEntity original;
		static IEntity other;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_transient_entities_of_different_types
	{
		It should_return_false_for_the_equality_operator = () =>
			(original == other).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(other == original).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(original != other).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(other != original).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			(original.Equals(other)).ShouldBeFalse();

		Because of = () =>
		{
			original = new TestEntityTypeBase();
			other = new TestEntityTypeDerived();
		};

		static IEntity original;
		static IEntity other;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_a_transient_entity_to_a_non_transient
	{
		It should_return_false_for_the_equality_operator = () =>
			(nonTransient == transient).ShouldBeFalse();

		It should_return_false_for_the_equality_operator_reversed = () =>
			(transient == nonTransient).ShouldBeFalse();

		It should_return_true_for_the_inequality_operator = () =>
			(nonTransient != transient).ShouldBeTrue();

		It should_return_true_for_the_inequality_operator_reversed = () =>
			(transient != nonTransient).ShouldBeTrue();

		It should_return_false_for_the_equals_method = () =>
			(nonTransient.Equals(transient)).ShouldBeFalse();

		It should_have_different_hashcodes = () =>
			nonTransient.GetHashCode().ShouldNotEqual(transient.GetHashCode());

		Because of = () =>
		{
			nonTransient = new TestEntityTypeBase(new Random().Next());
			transient = new TestEntityTypeDerived();
		};

		static IEntity nonTransient;
		static IEntity transient;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_a_transient_instance_becomes_persistent
	{
		It should_keep_the_same_hashcode = () =>
			transientHashCode.ShouldEqual(persistentHashCode);

		Establish context = () =>
			Object = new TestEntityTypeBase();

		Because of = () =>
		{
			Object.Id.ShouldEqual(default(int));
			transientHashCode = Object.GetHashCode();
			Object.Id = 89;
			persistentHashCode = Object.GetHashCode();
		};

		static int transientHashCode;
		static int persistentHashCode;
		static TestEntityTypeBase Object;
	}

	[Subject(typeof(IdentityFieldProvider<,>))]
	class When_comparing_two_null_objects_of_the_same_type
	{
		It should_return_true_for_the_equality_operator = () =>
			(obj1 == obj2).ShouldBeTrue();

		It should_return_false_for_the_inequality_operator = () =>
			(obj1 != obj2).ShouldBeFalse();

		Because of = () =>
		{
			obj1 = null;
			obj2 = null;
		};

		static TestEntityTypeBase obj1;
		static TestEntityTypeBase obj2;
	}

	class TestEntityTypeBase : IdentityFieldProvider<TestEntityTypeBase, int>
	{
		readonly int _id;

		public TestEntityTypeBase(int id)
		{
			_id = id;
		}

		public TestEntityTypeBase()
		{
		}

		public override int Id
		{
			get { return _id; }
		}
	}

	class TestEntityTypeNew : IdentityFieldProvider<TestEntityTypeNew, int>
	{
		readonly int _id;

		public TestEntityTypeNew(int id)
		{
			_id = id;
		}

		public TestEntityTypeNew()
		{
		}

		public override int Id
		{
			get { return _id; }
		}
	}

	class TestEntityTypeDerived : TestEntityTypeBase
	{
		public TestEntityTypeDerived(int id)
			: base(id)
		{
		}

		public TestEntityTypeDerived() { }
	}
}
