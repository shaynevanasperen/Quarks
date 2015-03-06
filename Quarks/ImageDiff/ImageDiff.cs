using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Quarks.ImageDiff
{
	public unsafe class ImageDiffTool
	{
		public bool Compare(Bitmap firstImage, Bitmap secondImage)
		{
			if (firstImage == null || secondImage == null) throw new NullReferenceException("one or more images may be null");
			if (firstImage.Height != secondImage.Height || firstImage.Width != secondImage.Width) throw new FormatException("images are not the same size");
			if (firstImage.Height > 2000 && firstImage.Width > 2000) throw new FormatException("one or more image might be too large");

			bool equals = true;

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

			return @equals;
		}

		public Bitmap CreateDifferenceImage(Bitmap firstImage, Bitmap secondImage)
		{
			if (firstImage == null || secondImage == null) throw new NullReferenceException("one or more images may be null");
			if (firstImage.Height != secondImage.Height || firstImage.Width != secondImage.Width) throw new FormatException("images are not the same size");
			if (firstImage.Height > 2000 && firstImage.Width > 2000) throw new FormatException("one or more image might be too large");

			var matchColor = Color.Red;

			var diffImage = secondImage.Clone() as Bitmap;

			int height = firstImage.Height;
			int width = firstImage.Width;

			BitmapData data1 = firstImage.LockBits(new Rectangle(0, 0, width, height),
				ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			BitmapData data2 = secondImage.LockBits(new Rectangle(0, 0, width, height),
				ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			if (diffImage != null)
			{
				BitmapData diffData = diffImage.LockBits(new Rectangle(0, 0, width, height),
					ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

				byte* data1Ptr = (byte*)data1.Scan0;
				byte* data2Ptr = (byte*)data2.Scan0;
				byte* diffPtr = (byte*)diffData.Scan0;

				byte[] swapColor = new byte[3];
				swapColor[0] = matchColor.B;
				swapColor[1] = matchColor.G;
				swapColor[2] = matchColor.R;

				int rowPadding = data1.Stride - (firstImage.Width * 3);

				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						int same = 0;

						byte[] tmp = new byte[3];

						for (int x = 0; x < 3; x++)
						{
							tmp[x] = data2Ptr[0];
							if (data1Ptr[0] == data2Ptr[0])
							{
								same++;
							}
							data1Ptr++;
							data2Ptr++; 
						}
						for (int x = 0; x < 3; x++)
						{
							diffPtr[0] = (same == 3) ? swapColor[x] : tmp[x];
							diffPtr++;
						}
					}

					if (rowPadding > 0)
					{
						data1Ptr += rowPadding;
						data2Ptr += rowPadding;
						diffPtr += rowPadding;
					}
				}

				firstImage.UnlockBits(data1);
				secondImage.UnlockBits(data2);
				diffImage.UnlockBits(diffData);
			}

			if (diffImage != null)
				diffImage.MakeTransparent(Color.Red);

			return diffImage;
		}
	}
}
