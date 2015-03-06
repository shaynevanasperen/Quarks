using System;
using System.Drawing;
using Machine.Fakes;
using Machine.Specifications;
using Quarks.ImageDiff;

namespace Quarks.Tests.ImageDiff
{
	class Images
	{
		public static Bitmap MonaLisa;
		public static Bitmap MonaLisaDuplicate;
		public static Bitmap MonaLisaWithMoustache;
		public static Bitmap MonaLisaDiff;

		public Images()
		{
			MonaLisa = Properties.Resources.Mona_Lisa;
			MonaLisaDuplicate = (Bitmap)MonaLisa.Clone();
			MonaLisaWithMoustache = Properties.Resources.Mona_Lisa_Moustache;
			MonaLisaDiff = Properties.Resources.Mona_Lisa_Diff;
		}
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_both_images_are_the_same : WithSubject<ImageDiffTool>
	{
		It should_return_true = () =>
			result.ShouldBeTrue();

		Because of = () =>
			result = Subject.Compare(Images.MonaLisa, Images.MonaLisaDuplicate);

		Establish context = () => With(new Images());

		static bool result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_the_images_are_the_different : WithSubject<ImageDiffTool>
	{
		It should_return_false = () =>
			result.ShouldBeFalse();

		Because of = () =>
			result = Subject.Compare(Images.MonaLisa, Images.MonaLisaWithMoustache);

		Establish context = () => With(new Images());

		static bool result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_the_images_are_different_sizes : WithSubject<ImageDiffTool>
	{
		It should_throw_a_format_exception = () =>
			result.ShouldBeOfExactType<FormatException>();

		It should_throw_an_exception_with_message_images_are_not_the_same_size = () =>
			result.Message.ShouldEqual("images are not the same size");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.Compare(new Bitmap(150, 150), new Bitmap(200, 200))));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_the_images_are_large_in_size : WithSubject<ImageDiffTool>
	{
		It should_throw_a_format_exception = () =>
			result.ShouldBeOfExactType<FormatException>();

		It should_throw_an_exception_with_message_one_or_more_image_might_be_too_large = () =>
			result.Message.ShouldEqual("one or more image might be too large");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.Compare(new Bitmap(5000, 5000), new Bitmap(5000, 5000))));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_one_image_is_null : WithSubject<ImageDiffTool>
	{
		It should_throw_a_null_reference_exception = () =>
			result.ShouldBeOfExactType<NullReferenceException>();

		It should_throw_an_exception_with_message_one_or_more_images_may_be_null = () =>
			result.Message.ShouldEqual("one or more images may be null");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.Compare(new Bitmap(200, 200), null)));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_comparing_images_and_both_images_are_null : WithSubject<ImageDiffTool>
	{
		It should_throw_a_null_reference_exception = () =>
			result.ShouldBeOfExactType<NullReferenceException>();

		It should_throw_an_exception_with_message_one_or_more_images_may_be_null = () =>
			result.Message.ShouldEqual("one or more images may be null");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.Compare(null, null)));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_and_both_images_for_comparison_are_null : WithSubject<ImageDiffTool>
	{
		It should_throw_a_null_reference_exception = () =>
			result.ShouldBeOfExactType<NullReferenceException>();

		It should_throw_an_exception_with_message_one_or_more_images_may_be_null = () =>
			result.Message.ShouldEqual("one or more images may be null");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.CreateDifferenceImage(null, null)));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_and_one_image_for_comparison_is_null : WithSubject<ImageDiffTool>
	{
		It should_throw_a_null_reference_exception = () =>
			result.ShouldBeOfExactType<NullReferenceException>();

		It should_throw_an_exception_with_message_one_or_more_images_may_be_null = () =>
			result.Message.ShouldEqual("one or more images may be null");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.CreateDifferenceImage(null, new Bitmap(200, 200))));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_and_the_images_for_comparison_are_different_sizes : WithSubject<ImageDiffTool>
	{
		It should_throw_a_format_exception = () =>
			result.ShouldBeOfExactType<FormatException>();

		It should_throw_an_exception_with_message_images_are_not_the_same_size = () =>
			result.Message.ShouldEqual("images are not the same size");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.CreateDifferenceImage(new Bitmap(150, 150), new Bitmap(200, 200))));

		static Exception result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_with_two_different_images : WithSubject<ImageDiffTool>
	{
		It should_return_a_valid_diff_image = () =>
			result.Compare(Images.MonaLisaDiff).ShouldBeTrue();

		Because of = () =>
			result = Subject.CreateDifferenceImage(Images.MonaLisa, Images.MonaLisaWithMoustache);

		Establish context = () => With(new Images());

		static Bitmap result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_with_images_that_are_exactly_the_same : WithSubject<ImageDiffTool>
	{
		It should_return_a_empty_diff_image = () =>
			result.Compare(new Bitmap(534, 401)).ShouldBeTrue();

		Because of = () =>
			result = Subject.CreateDifferenceImage(Images.MonaLisa, Images.MonaLisaDuplicate);

		Establish context = () => With(new Images());

		static Bitmap result;
	}

	[Subject(typeof(ImageDiffTool))]
	class When_creating_difference_image_and_the_images_for_comparison_are_large_in_size : WithSubject<ImageDiffTool>
	{
		It should_throw_a_format_exception = () =>
			result.ShouldBeOfExactType<FormatException>();

		It should_throw_an_exception_with_message_one_or_more_image_might_be_too_large = () =>
			result.Message.ShouldEqual("one or more image might be too large");

		Because of = () =>
			result = Catch.Exception((Action)(() => Subject.CreateDifferenceImage(new Bitmap(5000, 5000), new Bitmap(5000, 5000))));

		static Exception result;
	}
}
