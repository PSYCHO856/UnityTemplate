using UnityEngine;

namespace MobileKit
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Set color alpha
        /// </summary>
        public static Color SetAlpha(this Color color, byte aValue)
        {
            color.a = aValue;

            return color;
        }

        /// <summary>
        /// Set color alpha
        /// </summary>
        public static Color SetAlpha(this Color color, float aValue)
        {
            color.a = aValue;

            return color;
        }

        /// <summary>
        /// Set color alpha (0-255)
        /// </summary>
        public static Color SetAlpha(this Color color, int aValue)
        {
            color.a = (float)aValue / 255;

            return color;
        }

        /// <summary>
        /// Convert to HEX
        /// </summary>
        public static string ToHex(this Color color)
        {
            return
                $"#{(byte) (Mathf.Clamp01(color.r) * 255):X2}{(byte) (Mathf.Clamp01(color.g) * 255):X2}{(byte) (Mathf.Clamp01(color.b) * 255):X2}";
        }
        
        /// <summary>
        /// Convert from CMYK colorspace
        /// 颜色值从CMYK颜色空间转换到RGB颜色空间
        /// </summary>
        public static Color FromCMYK(this Color color, float c, float m, float y, float k)
        {
            color.r = ( 1.0f - c ) * ( 1.0f - k );
            color.g = ( 1.0f - m ) * ( 1.0f - k );
            color.b = ( 1.0f - y ) * ( 1.0f - k );
            return color;
        }

        /// <summary>
        /// Convert to CMYK colorspace
        /// </summary>
        public static Vector4 ToCMYK(this Color color)
        {
            float rgbMax = Mathf.Max( color.r, Mathf.Max( color.g, color.b ) );
            float c, m, y;
            float k = 1.0f - rgbMax;
            if( rgbMax == 0.0f ) {
                c = m = y = 0.0f;
            }
            else
            {
                c = ( 1.0f - color.r - k ) / ( 1.0f - k );
                m = ( 1.0f - color.g - k ) / ( 1.0f - k );
                y = ( 1.0f - color.b - k ) / ( 1.0f - k );
            }
            return new Vector4(c, m, y, k);
        }
        
        
    }
}