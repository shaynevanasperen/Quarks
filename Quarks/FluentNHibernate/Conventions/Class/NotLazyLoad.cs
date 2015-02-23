using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;

namespace Quarks.FluentNHibernate.Conventions.Class
{
	class NotLazyLoad : IClassConvention, IClassConventionAcceptance
	{
		public void Apply(IClassInstance instance)
		{
			instance.Not.LazyLoad();
		}

		public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
		{
			criteria.Expect(x => !x.LazyLoad && !anyLazyMembers(x));
		}

		static bool anyLazyMembers(IClassInspector inspector)
		{
			if (inspector.Properties.Any(x => x.LazyLoad))
				return true;

			if (inspector.OneToOnes.Any(x => x.LazyLoad.Equals(Laziness.Proxy) || x.LazyLoad.Equals(Laziness.NoProxy)))
				return true;

			if (inspector.References.Any(x => x.LazyLoad.Equals(Laziness.Proxy) || x.LazyLoad.Equals(Laziness.NoProxy)))
				return true;

			return inspector.Collections.Any(x => !x.LazyLoad.Equals(Lazy.False));
		}
	}
}
