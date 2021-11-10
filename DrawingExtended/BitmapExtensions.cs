using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DrawingExtended
{
    public static class BitmapExtensions
    {
        public static Bitmap FromBytes(this byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                return new Bitmap(ms);
            }
        }

        public static byte[] GetBytes(this Bitmap value, ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                value.Save(ms, imageFormat);
                return ms.GetBuffer();
            }
        }

        public static bool IsNullOrEmpty(this Bitmap value)
        {
            if (value == null)
                return true;
            return value.Size.IsEmpty;
        }

        public static Bitmap ScaleProportional(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleProportional(size.Width, size.Height);
        }

        public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }

        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }
            return scaledBitmap;
        }

        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSizeProportional(Color.White, size);
        }

        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, Size size)
        {
            return bitmap.ScaleToSizeProportional(backgroundColor, size.Width, size.Height);
        }

        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, int width, int height)
        {
            return bitmap.ScaleToSizeProportional(Color.White, width, height);
        }

        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                var proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m), (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }
    }
}
