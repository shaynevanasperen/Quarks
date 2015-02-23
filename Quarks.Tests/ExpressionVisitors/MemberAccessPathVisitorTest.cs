using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Machine.Specifications;
using Quarks.ExpressionVisitors;

namespace Quarks.Tests.ExpressionVisitors
{
	[Subject(typeof(MemberAccessPathVisitor))]
	class When_using_a_member_access_path_visitor
	{
		It should_return_the_member_name_when_accessing_a_top_level_member = () =>
		{
			Expression<Func<Customer, object>> expression = x => x.Orders;
			var visitor = new MemberAccessPathVisitor();
			visitor.Visit(expression);
			visitor.Path.ShouldEqual("Orders");
		};

		It should_return_the_full_path_when_accessing_a_top_nested_member = () =>
		{
			Expression<Func<SalesPerson, object>> expression = x => x.PrimaryCustomer.Orders;
			var visitor = new MemberAccessPathVisitor();
			visitor.Visit(expression);
			visitor.Path.ShouldEqual("PrimaryCustomer.Orders");
		};

		It should_thow_a_not_supported_exception_when_expresion_contains_a_method_call = () =>
		{
			Expression<Func<SalesPerson, object>> expression = x => x.MethodAccess();
			var visitor = new MemberAccessPathVisitor();
			Catch.Exception(() => visitor.Visit(expression)).ShouldBeOfExactType<NotSupportedException>();
		};

		class SalesPerson
		{
			public Customer PrimaryCustomer { get; set; }

			public object MethodAccess() { return null; }
		}

		class Customer
		{
			public Customer()
			{
				Orders = new List<Order>();
			}

			public IList<Order> Orders;
		}

		class Order { }
	}
}