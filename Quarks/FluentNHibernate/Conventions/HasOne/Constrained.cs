using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasOne
{
	/// <remarks>
	/// For info on Cascade see http://ayende.com/Blog/archive/2006/12/02/nhibernatecascadesthedifferentbetweenallalldeleteorphansandsaveupdate.aspx
	/// </remarks>
	class Constrained<TParentEntity> : IHasOneConvention, IHasOneConventionAcceptance
	{
		public void Apply(IOneToOneInstance instance)
		{
			// Child is constrained. Parent record must exist
			instance.Constrained();
		}

		public void Accept(IAcceptanceCriteria<IOneToOneInspector> criteria)
		{
			// Apply to children of ParentEntity
			criteria.Expect(c => c.Class == typeof(TParentEntity));
			// Ignore properties that have already been set
			criteria.Expect(c => string.IsNullOrEmpty(c.PropertyRef));
		}
	}
}
