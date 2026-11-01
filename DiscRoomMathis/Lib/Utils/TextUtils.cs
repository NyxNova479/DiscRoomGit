using System;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    public static class TextUtils
    {
        /// <summary>
        /// Draws a text horizontally centered on centerX
        /// </summary>
        public static void DrawTextCentered(string text, float centerX, float y, int fontSize, Color color)
        {
            int textWidth = MeasureText(text, fontSize);

            DrawText(text, (int)(centerX - textWidth * 0.5f), (int)y, fontSize, color);
        }
    }
}
