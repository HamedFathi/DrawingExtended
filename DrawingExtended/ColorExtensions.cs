using System.Drawing;

namespace DrawingExtended
{
    public static class ColorExtensions
    {
        public static string ToHtmlColor(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        public static int ToOleColor(this Color color)
        {
            return ColorTranslator.ToOle(color);
        }

        public static int ToWin32Color(this Color color)
        {
            return ColorTranslator.ToWin32(color);
        }
    }
}
