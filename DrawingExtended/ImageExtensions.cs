using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DrawingExtended
{
    public static class ImageExtensions
    {
        public static Image ConvertBase64ToImage(this string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0,
                imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            return Image.FromStream(ms, true);
        }

        public static string ConvertImageToBase64(this Image image, ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                var imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public static Image ResizeAndFit(this Image @this, Size newSize)
        {
            var sourceIsLandscape = @this.Width > @this.Height;
            var targetIsLandscape = newSize.Width > newSize.Height;

            var ratioWidth = newSize.Width / (double)@this.Width;
            var ratioHeight = newSize.Height / (double)@this.Height;

            double ratio;

            if (ratioWidth > ratioHeight && sourceIsLandscape == targetIsLandscape)
                ratio = ratioWidth;
            else
                ratio = ratioHeight;

            var targetWidth = (int)(@this.Width * ratio);
            var targetHeight = (int)(@this.Height * ratio);

            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var offsetX = (double)(newSize.Width - targetWidth) / 2;
            var offsetY = (double)(newSize.Height - targetHeight) / 2;

            graphics.DrawImage(@this, (int)offsetX, (int)offsetY, targetWidth, targetHeight);
            graphics.Dispose();

            return bitmap;
        }

        public static Image ScaleImage(this Image @this, int height, int width)
        {
            if (@this == null || height <= 0 || width <= 0)
            {
                return null;
            }
            var newWidth = @this.Width * height / @this.Height;
            var newHeight = @this.Height * width / @this.Width;
            int x, y;

            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            // use this when debugging.
            //g.FillRectangle(Brushes.Aqua, 0, 0, bmp.Width - 1, bmp.Height - 1);
            if (newWidth > width)
            {
                // use new height
                x = (bmp.Width - width) / 2;
                y = (bmp.Height - newHeight) / 2;
                g.DrawImage(@this, x, y, width, newHeight);
            }
            else
            {
                // use new width
                x = bmp.Width / 2 - newWidth / 2;
                y = bmp.Height / 2 - height / 2;
                g.DrawImage(@this, x, y, newWidth, height);
            }
            // use this when debugging.
            //g.DrawRectangle(new Pen(Color.Red, 1), 0, 0, bmp.Width - 1, bmp.Height - 1);
            return bmp;
        }

        public static byte[] ToByteArray(this Image @this, ImageFormat imageFormat)
        {
            var ms = new MemoryStream();
            @this.Save(ms, imageFormat);
            return ms.ToArray();
        }

        public static Image ToImage(this byte[] byteArray)
        {
            var ms = new MemoryStream(byteArray);
            return Image.FromStream(ms);
        }
    }
}
