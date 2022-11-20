using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowAssetPreviewAttribute : DrawerAttribute
    {
        public ShowAssetPreviewAttribute(int width = 64, int height = 64)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }
    }
}