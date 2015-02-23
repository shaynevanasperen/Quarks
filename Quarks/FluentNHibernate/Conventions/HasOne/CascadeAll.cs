using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasOne
{
	/// <remarks>
	/// For info on Cascade see http://ayende.com/Blog/archive/2006/12/02/nhibernatecascadesthedifferentbetweenallalldeleteorphansandsaveupdate.aspx
	/// </remarks>
	class CascadeAll<TParentEntity> : IHasOneConvention, IHasOneConventionAcceptance
	{
		public void Apply(IOneToOneInstance instance)
		{
			// Cascade All from Parent to child
			instance.Cascade.All();
		}

		public void Accept(IAcceptanceCriteria<IOneToOneInspector> criteria)
		{
			// Apply to ParentEntity entities
			criteria.Expect(c => c.EntityType == typeof(TParentEntity));
			// Ignore properties that have already been set
			criteria.Expect(c => string.IsNullOrEmpty(c.PropertyRef));
		}
	}
}
