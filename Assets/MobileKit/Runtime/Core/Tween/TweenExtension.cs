using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public static class TweenExtensions
    {
        #region Transform
        /// <summary>
        /// Changes rotation angle of object.
        /// </summary>
        public static TweenCase MDORotate(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomRotateAngle(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes quaternion rotation of object.
        /// </summary>
        public static TweenCase MDORotate(this Transform tweenObject, Quaternion resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomRotateQuaternion(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local quaternion rotation of object.
        /// </summary>
        public static TweenCase MDOLocalRotate(this Transform tweenObject, Quaternion resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalRotate(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local angle rotation of object.
        /// </summary>
        public static TweenCase MDOLocalRotate(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalRotateAngle(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes position of object.
        /// </summary>
        public static TweenCase MDOMove(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPosition(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        public static TweenCase MDOPath(this Transform tweenObject,Vector3[] path, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomDoPath(tweenObject, path).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x position of object.
        /// </summary>
        public static TweenCase MDOMoveX(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionX(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y position of object.
        /// </summary>
        public static TweenCase MDOMoveY(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionY(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z position of object.
        /// </summary>
        public static TweenCase MDOMoveZ(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPositionZ(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object.
        /// </summary>
        public static TweenCase MDOScale(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScale(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object.
        /// </summary>
        public static TweenCase MDOScale(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScale(tweenObject, new Vector3(resultValue, resultValue, resultValue)).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object twice.
        /// </summary>
        public static TweenCase MDOPushScale(this Transform tweenObject, Vector3 firstScale, Vector3 secondScale, float firstScaleTime, float secondScaleTime, Ease.Type firstScaleEasing = Ease.Type.Linear, Ease.Type secondScaleEasing = Ease.Type.Linear, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPushScale(tweenObject, firstScale, secondScale, firstScaleTime, secondScaleTime, firstScaleEasing, secondScaleEasing).SetTime(firstScaleTime + secondScaleTime).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local scale of object twice.
        /// </summary>
        public static TweenCase MDOPushScale(this Transform tweenObject, float firstScale, float secondScale, float firstScaleTime, float secondScaleTime, Ease.Type firstScaleEasing = Ease.Type.Linear, Ease.Type secondScaleEasing = Ease.Type.Linear, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomPushScale(tweenObject, firstScale.ToVector3(), secondScale.ToVector3(), firstScaleTime, secondScaleTime, firstScaleEasing, secondScaleEasing).SetTime(firstScaleTime + secondScaleTime).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x scale of object.
        /// </summary>
        public static TweenCase MDOScaleX(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleX(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y scale of object.
        /// </summary>
        public static TweenCase MDOScaleY(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleY(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z scale of object.
        /// </summary>
        public static TweenCase MDOScaleZ(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomScaleZ(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes local position of object.
        /// </summary>
        public static TweenCase MDOLocalMove(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalMove(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes x local position of object.
        /// </summary>
        public static TweenCase MDOLocalMoveX(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionX(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes y local position of object.
        /// </summary>
        public static TweenCase MDOLocalMoveY(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionY(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Changes z local position of object.
        /// </summary>
        public static TweenCase MDOLocalMoveZ(this Transform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLocalPositionZ(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Rotates object face to position.
        /// </summary>
        public static TweenCase MDOLookAt(this Transform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLookAt(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Rotates 2D object face to position.
        /// </summary>
        public static TweenCase MDOLookAt2D(this Transform tweenObject, Vector3 resultValue, TweenCaseTransfomLookAt2D.LookAtType type, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTransfomLookAt2D(tweenObject, resultValue, type).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region RectTransform
        /// <summary>
        /// Change anchored position of rectTransform
        /// </summary>
        public static TweenCase MDOAnchoredPosition(this RectTransform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformAnchoredPosition(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sizeDelta of rectTransform
        /// </summary>
        public static TweenCase MDOSizeScale(this RectTransform tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject, tweenObject.sizeDelta * resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sizeDelta of rectTransform
        /// </summary>
        public static TweenCase MDOSize(this RectTransform tweenObject, Vector3 resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseRectTransformSizeScale(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region SpriteRenderer
        /// <summary>
        /// Change color of sprite renderer
        /// </summary>
        public static TweenCase MDOColor(this SpriteRenderer tweenObject, Color resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseSpriteRendererColor(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change sprite renderer color alpha
        /// </summary>
        public static TweenCase MDOFade(this SpriteRenderer tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseSpriteRendererFade(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Image
        /// <summary>
        /// Change color of image
        /// </summary>
        public static TweenCase MDOColor(this Image tweenObject, Color resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseImageColor(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change image color alpha
        /// </summary>
        public static TweenCase MDOFade(this Image tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseImageFade(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Text
        /// <summary>
        /// Change text font size
        /// </summary>
        public static TweenCase MDOFontSize(this Text tweenObject, int resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextFontSize(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change text color alpha
        /// </summary>
        public static TweenCase MDOFade(this Text tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextFade(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }

        /// <summary>
        /// Change color of text
        /// </summary>
        public static TweenCase MDOColor(this Text tweenObject, Color resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseTextColor(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region CanvasGroup
        /// <summary>
        /// Change alpha value of canvas group
        /// </summary>
        public static TweenCase MDOFade(this CanvasGroup tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseCanvasGroupFade(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region AudioSource
        /// <summary>
        /// Change audio source volume
        /// </summary>
        public static TweenCase MDOVolume(this AudioSource tweenObject, float resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseAudioSourceVolume(tweenObject, resultValue).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion

        #region Other
        public static TweenCase MDOAction<T>(this object tweenObject, System.Action<T, T, float> action, T startValue, T resultValue, float time, bool unscaledTime = false, TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseAction<T>(startValue, resultValue, action).SetTime(time).SetUnscaledMode(unscaledTime).SetType(tweenType).StartTween();
        }
        #endregion
    }
}