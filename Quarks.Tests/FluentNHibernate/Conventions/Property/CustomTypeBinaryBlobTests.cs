using System.Drawing;
using System.Linq;
using Machine.Specifications;
using NHibernate.Mapping;
using NHibernate.Type;
using Quarks.FluentNHibernate.Conventions.Property;
using Quarks.Tests.Properties;

namespace Quarks.Tests.FluentNHibernate.Conventions.Property
{
	[Subject(typeof(CustomTypeBinaryBlob))]
	class When_using_byte_array_property_with_binary_blob_attribute
	{
		It should_load_a_persisted_entity_in_the_same_state_it_was_persisted = () =>
		{
			retrieved.Id.ShouldEqual(persisted.Id);
			retrieved.DefaultBinaryBlob.ShouldEqual(persisted.DefaultBinaryBlob);
			retrieved.LazyBinaryBlob.ShouldEqual(persisted.LazyBinaryBlob);
			retrieved.NotLazyBinaryBlob.ShouldEqual(persisted.NotLazyBinaryBlob);
		};

		It should_map_property_as_nullable = () =>
		{
			defaultBinaryBlobProperty.IsNullable.ShouldBeTrue();
			lazyBinaryBlobProperty.IsNullable.ShouldBeTrue();
			notLazyBinaryBlobProperty.IsNullable.ShouldBeTrue();
		};

		It should_map_property_type_as_binary_blob_type = () =>
		{
			defaultBinaryBlobProperty.Type.ShouldBeOfExactType<BinaryBlobType>();
			lazyBinaryBlobProperty.Type.ShouldBeOfExactType<BinaryBlobType>();
			notLazyBinaryBlobProperty.Type.ShouldBeOfExactType<BinaryBlobType>();
		};

		It should_map_property_column_with_length_set_to_maximum_int_value = () =>
		{
			// ReSharper disable PossibleNullReferenceException
			(defaultBinaryBlobProperty.ColumnIterator.Single() as global::NHibernate.Mapping.Column).Length.ShouldEqual(int.MaxValue);
			(lazyBinaryBlobProperty.ColumnIterator.Single() as global::NHibernate.Mapping.Column).Length.ShouldEqual(int.MaxValue);
			(notLazyBinaryBlobProperty.ColumnIterator.Single() as global::NHibernate.Mapping.Column).Length.ShouldEqual(int.MaxValue);
			// ReSharper restore PossibleNullReferenceException
		};

		It should_map_property_laziness_as_specified = () =>
		{
			defaultBinaryBlobProperty.IsLazy.ShouldBeFalse();
			lazyBinaryBlobProperty.IsLazy.ShouldBeTrue();
			notLazyBinaryBlobProperty.IsLazy.ShouldBeFalse();
		};

		Because of = () =>
		{
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				retrieved = session.Get<BinaryBlobEntity>(persisted.Id);
				transaction.Commit();
			}
		};

		Establish context = () =>
		{
			persistentClass = NHibernateContext.Configuration.GetClassMapping(typeof(BinaryBlobEntity));
			defaultBinaryBlobProperty = persistentClass.GetProperty(Type<BinaryBlobEntity>.MemberName(x => x.DefaultBinaryBlob));
			lazyBinaryBlobProperty = persistentClass.GetProperty(Type<BinaryBlobEntity>.MemberName(x => x.LazyBinaryBlob));
			notLazyBinaryBlobProperty = persistentClass.GetProperty(Type<BinaryBlobEntity>.MemberName(x => x.NotLazyBinaryBlob));

			var imageConverter = new ImageConverter();
			var moreThan8000Bytes = (byte[])imageConverter.ConvertTo(Resources.MoreThan8000Bytes, typeof(byte[]));
			var lessThan8000Bytes = (byte[])imageConverter.ConvertTo(Resources.LessThan8000Bytes, typeof(byte[]));

			persisted = new BinaryBlobEntity
			{
				DefaultBinaryBlob = moreThan8000Bytes,
				LazyBinaryBlob = moreThan8000Bytes,
				NotLazyBinaryBlob = lessThan8000Bytes
			};
			using (var session = NHibernateContext.SessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(persisted);
				transaction.Commit();
			}
		};

		static BinaryBlobEntity persisted, retrieved;
		static PersistentClass persistentClass;
		static global::NHibernate.Mapping.Property defaultBinaryBlobProperty, lazyBinaryBlobProperty, notLazyBinaryBlobProperty;
	}

	public class BinaryBlobEntity : IdentityFieldProvider<BinaryBlobEntity, int>
	{
		[BinaryBlob]
		public virtual byte[] DefaultBinaryBlob { get; set; }

		[BinaryBlob(LazyLoad = true)]
		public virtual byte[] LazyBinaryBlob { get; set; }

		[BinaryBlob(LazyLoad = false)]
		public virtual byte[] NotLazyBinaryBlob { get; set; }
	}
}
