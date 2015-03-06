using System.Drawing;
using System.Drawing.Imaging;

namespace Quarks.Tests.ImageDiff
{
	internal static class ImageExtensions
	{
		public static unsafe bool Compare(this Bitmap firstImage, Bitmap secondImage)
		{
			bool equals = true;
			if (firstImage != null && secondImage != null)
			{
				if (firstImage.Height == secondImage.Height && firstImage.Width == secondImage.Width)
				{
					Rectangle rect = new Rectangle(0, 0, firstImage.Width, firstImage.Height);
					BitmapData bmpData1 = firstImage.LockBits(rect, ImageLockMode.ReadOnly, firstImage.PixelFormat);
					BitmapData bmpData2 = secondImage.LockBits(rect, ImageLockMode.ReadOnly, secondImage.PixelFormat);

					byte* ptr1 = (byte*)bmpData1.Scan0.ToPointer();
					byte* ptr2 = (byte*)bmpData2.Scan0.ToPointer();
					int width = rect.Width * 3; // for 24bpp pixel data
					for (int y = 0; @equals && y < rect.Height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							if (*ptr1 != *ptr2)
							{
								@equals = false;
								break;
							}
							ptr1++;
							ptr2++;
						}
						ptr1 += bmpData1.Stride - width;
						ptr2 += bmpData2.Stride - width;
					}
					firstImage.UnlockBits(bmpData1);
					secondImage.UnlockBits(bmpData2);

					return equals;
				}
			}
			return false;
		}
	}
}
