using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Collections;

namespace Quarks.FluentNHibernate.Conventions.HasMany
{
	class NotKeyNullable : IHasManyConvention, IHasManyConventionAcceptance
	{
		public void Apply(IOneToManyCollectionInstance instance)
		{
			instance.Not.KeyNullable();
		}

		public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
		{
			criteria.Expect(x => x.LazyLoad == Lazy.False);
		}
	}
}
