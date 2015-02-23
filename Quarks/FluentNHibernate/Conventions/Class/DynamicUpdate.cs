using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.Class
{
	class DynamicUpdate : IClassConvention
	{
		public void Apply(IClassInstance instance)
		{
			instance.DynamicUpdate();
		}
	}
}
