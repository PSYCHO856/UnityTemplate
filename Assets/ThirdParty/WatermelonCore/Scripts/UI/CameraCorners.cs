using UnityEngine;

namespace Watermelon
{
    public class CameraCorners
    {
        public CameraCorners(Camera camera)
        {
            var leftPoint = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var rightPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            Left = leftPoint.x;
#if UNITY_EDITOR
            Top = Camera.main.orthographicSize;
#else
            top = leftPoint.y;
#endif

#if UNITY_EDITOR
            Right = Camera.main.orthographicSize * 2 * Camera.main.aspect * 0.5f;
#else
            right = rightPoint.x;
#endif
            Bottom = rightPoint.y;
        }

        public float Left { get; }

        public float Right { get; }

        public float Top { get; }

        public float Bottom { get; }

        public float Width => Right - Left;

        public float Height => Top - Bottom;
    }
}