using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Quarks.FluentNHibernate.Conventions.HasMany
{
	/// <summary>
	/// Applies the key field convention to the Many end of HasMany relationships - "RelatedEntityTypeId"
	/// </summary>
	class KeyColumn : IHasManyConvention, IHasManyConventionAcceptance
	{
		public void Apply(IOneToManyCollectionInstance instance)
		{
			var abbreviation = AttributeHelper.GetTypeAttribute<AbbreviationAttribute>(instance.EntityType);
			var prefix = abbreviation == null
				? instance.EntityType.Name
				: abbreviation.Abbreviation;
			instance.Key.Column((prefix + "Id").EscapeColumnName());
		}

		public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
		{
			// Only update properties that haven't been set.
			criteria.Expect(x => x.Key.Columns, Is.Not.Set);
		}
	}
}
