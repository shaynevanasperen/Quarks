using System;
using Machine.Specifications;
using Machine.Specifications.Model;

namespace Quarks.Tests
{
	[Subject(typeof(AttributeHelper))]
	class When_getting_member_attribute
	{
		It should_find_an_attribute_on_a_method = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0)).ShouldNotBeNull();

		It should_find_an_attribute_matching_predicate_on_a_method = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0), x => x.Name == "Stevie").ShouldNotBeNull();

		It should_not_find_an_attribute_on_a_method_without_it = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithoutAttribute(0)).ShouldBeNull();

		It should_not_find_an_attribute_mismatching_predicate_on_a_method = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0), x => x.Name == "X").ShouldBeNull();


		It should_find_an_attribute_on_a_property = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute).ShouldNotBeNull();

		It should_find_an_attribute_matching_predicate_on_a_property = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute, x => x.Name == "Bob").ShouldNotBeNull();

		It should_not_find_an_attribute_on_a_property_without_it = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithoutAttribute).ShouldBeNull();

		It should_not_find_an_attribute_mismatching_predicate_on_a_property = () =>
			AttributeHelper.GetMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute, x => x.Name == "X").ShouldBeNull();
	}

	[Subject(typeof(AttributeHelper))]
	class When_getting_member_attributes
	{
		It should_find_all_attributes_on_a_method = () =>
			AttributeHelper.GetMemberAttributes<Attributed>(x => x.MethodWithAttributes(0)).Count.ShouldEqual(2);

		It should_find_all_attributes_of_type_on_a_method = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.MethodWithAttributes(0)).Count.ShouldEqual(1);

		It should_find_no_attributes_on_a_method_having_none = () =>
			AttributeHelper.GetMemberAttributes<Attributed>(x => x.MethodWithoutAttribute(0)).Count.ShouldEqual(0);


		It should_find_all_attributes_on_a_property = () =>
			AttributeHelper.GetMemberAttributes<Attributed>(x => x.PropertyWithAttributes).Count.ShouldEqual(2);

		It should_find_all_attributes_of_type_on_a_property = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.PropertyWithAttributes).Count.ShouldEqual(1);

		It should_find_no_attributes_on_a_property_having_none = () =>
			AttributeHelper.GetMemberAttributes<Attributed>(x => x.PropertyWithoutAttribute).Count.ShouldEqual(0);


		It should_find_all_attributes_of_type_matching_predicate_on_a_method = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.MethodWithAttributes(0), x => x.Name == "Gary").Count.ShouldEqual(1);

		It should_find_no_attributes_of_type_mismatching_predicate_on_a_method = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.MethodWithAttributes(0), x => x.Name == "X").Count.ShouldEqual(0);


		It should_find_all_attributes_of_type_matching_predicate_on_a_property = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.PropertyWithAttributes, x => x.Name == "James").Count.ShouldEqual(1);

		It should_find_no_attributes_of_type_mismatching_predicate_on_a_property = () =>
			AttributeHelper.GetMemberAttributes<Attributed, MyAttribute>(x => x.PropertyWithAttributes, x => x.Name == "X").Count.ShouldEqual(0);
	}

	[Subject(typeof(AttributeHelper))]
	class When_getting_type_attribute
	{
		It should_find_an_attribute_of_type_on_a_type = () =>
			AttributeHelper.GetTypeAttribute<Attributed, MyAttribute>().ShouldNotBeNull();

		It should_not_find_an_attribute_of_type_on_a_type_without_it = () =>
			AttributeHelper.GetTypeAttribute<Attributed, Subject>().ShouldBeNull();


		It should_find_an_attribute_of_type_matching_predicate_on_a_type = () =>
			AttributeHelper.GetTypeAttribute<Attributed, MyAttribute>(x => x.Name == "Steve").ShouldNotBeNull();

		It should_not_find_an_attribute_of_type_mismatching_predicate_on_a_type_without_it = () =>
			AttributeHelper.GetTypeAttribute<Attributed, MyAttribute>(x => x.Name == "X").ShouldBeNull();
	}

	[Subject(typeof(AttributeHelper))]
	class When_getting_type_attributes
	{
		It should_find_all_attributes_on_a_type = () =>
			AttributeHelper.GetTypeAttributes<OtherAttributed>().Count.ShouldEqual(2);

		It should_find_all_attributes_of_type_on_a_type = () =>
			AttributeHelper.GetTypeAttributes<OtherAttributed, MyAttribute>().Count.ShouldEqual(1);

		It should_find_no_attributes_on_a_type_having_none = () =>
			AttributeHelper.GetTypeAttributes<NotAttributed>().Count.ShouldEqual(0);


		It should_find_all_attributes_of_type_matching_predicate_on_a_type = () =>
			AttributeHelper.GetTypeAttributes<OtherAttributed, MyAttribute>(x => x.Name == "Alex").Count.ShouldEqual(1);

		It should_find_no_attributes_of_type_mismatching_predicate_on_a_type = () =>
			AttributeHelper.GetTypeAttributes<OtherAttributed, MyAttribute>(x => x.Name == "X").Count.ShouldEqual(0);
	}

	[Subject(typeof(AttributeHelper))]
	class When_checking_for_existence_of_member_attribute
	{
		It should_report_true_if_method_has_attribute = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0)).ShouldBeTrue();

		It should_report_false_if_method_has_no_attribute = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithoutAttribute(0)).ShouldBeFalse();

		It should_report_true_if_method_has_attribute_matching_predicate = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0), x => x.Name == "Stevie").ShouldBeTrue();

		It should_report_false_if_method_has_attribute_mismatching_predicate = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.MethodWithAttribute(0), x => x.Name == "X").ShouldBeFalse();


		It should_report_true_if_property_has_attribute = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute).ShouldBeTrue();

		It should_report_false_if_property_has_no_attribute = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithoutAttribute).ShouldBeFalse();

		It should_report_true_if_property_has_attribute_matching_predicate = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute, x => x.Name == "Bob").ShouldBeTrue();

		It should_report_false_if_property_has_attribute_mismatching_predicate = () =>
			AttributeHelper.HasMemberAttribute<Attributed, MyAttribute>(x => x.PropertyWithAttribute, x => x.Name == "X").ShouldBeFalse();
	}

	[Subject(typeof(AttributeHelper))]
	class When_checking_for_existence_of_type_attribute
	{
		It should_report_true_if_type_has_attribute = () =>
			AttributeHelper.HasTypeAttribute<Attributed, MyAttribute>().ShouldBeTrue();

		It should_report_false_if_type_has_no_attribute = () =>
			AttributeHelper.HasTypeAttribute<Attributed, Subject>().ShouldBeFalse();

		It should_report_true_if_type_has_attribute_matching_predicate = () =>
			AttributeHelper.HasTypeAttribute<Attributed, MyAttribute>(x => x.Name == "Steve").ShouldBeTrue();

		It should_report_false_if_type_has_attribute_mismatching_predicate = () =>
			AttributeHelper.HasTypeAttribute<Attributed, MyAttribute>(x => x.Name == "X").ShouldBeFalse();
	}

	[My(Name = "Steve")]
	class Attributed
	{
		[My(Name = "Bob")]
		public bool PropertyWithAttribute { get; set; }

		[My(Name = "James"), Other]
		public bool PropertyWithAttributes { get; set; }

		public bool PropertyWithoutAttribute { get; set; }

		[My(Name = "Stevie")]
		public bool MethodWithAttribute(int x)
		{
			return true;
		}

		[My(Name = "Gary"), Other]
		public bool MethodWithAttributes(int x)
		{
			return true;
		}

		public bool MethodWithoutAttribute(int x)
		{
			return true;
		}
	}

	[My(Name = "Alex"), Other]
	class OtherAttributed { }

	class NotAttributed { }

	class MyAttribute : Attribute
	{
		public string Name { get; set; }
	}

	class OtherAttribute : Attribute { }
}