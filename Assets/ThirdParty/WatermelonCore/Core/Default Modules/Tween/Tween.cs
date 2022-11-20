using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bugs
//Wrong unscaled time if tween called from start

namespace Watermelon
{
    public class Tween : MonoBehaviour
    {
        public delegate void TweenCallback();

        private static Tween instance;

        private bool systemLogs;

        /// <summary>
        ///     Create tween instance.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
#if UNITY_EDITOR
            else
            {
                if (systemLogs)
                    Debug.LogError("[Tween]: Tween already exist!");
            }
#endif
        }

        private void Update()
        {
            UpdateFramesCount++;

            if (!hasActiveUpdateTweens)
                return;

            if (updateRequiresActiveReorganization)
                ReorganizeUpdateActiveTweens();

            var deltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;

            for (var i = 0; i < updateTweensCount; i++)
            {
                var tween = updateTweens[i];
                if (tween != null)
                {
                    if (!tween.Validate())
                    {
                        tween.Kill();
                    }
                    else
                    {
                        if (tween.isActive && !tween.isPaused)
                        {
                            if (!tween.isUnscaled)
                            {
                                if (Time.timeScale == 0)
                                    continue;

                                tween.NextState(deltaTime);
                            }
                            else
                            {
                                tween.NextState(unscaledDeltaTime);
                            }

                            tween.Invoke();

                            if (tween.isCompleted)
                            {
                                tween.DefaultComplete();

                                if (tween.onCompleteCallback != null)
                                    tween.onCompleteCallback.Invoke();

                                tween.Kill();
                            }
                        }
                    }
                }
            }

            var killingTweensCount = updateKillingTweens.Count - 1;
            for (var i = killingTweensCount; i > -1; i--) RemoveActiveTween(updateKillingTweens[i]);
            updateKillingTweens.Clear();
        }

        private void FixedUpdate()
        {
            FixedUpdateFramesCount++;

            if (!hasActiveFixedTweens)
                return;

            if (fixedRequiresActiveReorganization)
                ReorganizeFixedActiveTweens();

            var deltaTime = Time.fixedDeltaTime;
            var unscaledDeltaTime = Time.fixedUnscaledDeltaTime;

            for (var i = 0; i < fixedTweensCount; i++)
            {
                var tween = fixedTweens[i];
                if (tween != null)
                {
                    if (!tween.Validate())
                    {
                        tween.Kill();
                    }
                    else
                    {
                        if (tween.isActive && !tween.isPaused)
                        {
                            if (!tween.isUnscaled)
                            {
                                if (Time.timeScale == 0)
                                    continue;

                                tween.NextState(deltaTime);
                            }
                            else
                            {
                                tween.NextState(unscaledDeltaTime);
                            }

                            tween.Invoke();

                            if (tween.isCompleted)
                            {
                                tween.DefaultComplete();

                                if (tween.onCompleteCallback != null)
                                    tween.onCompleteCallback.Invoke();

                                tween.Kill();
                            }
                        }
                    }
                }
            }

            var killingTweensCount = fixedKillingTweens.Count - 1;
            for (var i = killingTweensCount; i > -1; i--) RemoveActiveTween(fixedKillingTweens[i]);
            fixedKillingTweens.Clear();
        }

        private void LateUpdate()
        {
            LateUpdateFramesCount++;

            if (!hasActiveLateTweens)
                return;

            if (lateRequiresActiveReorganization)
                ReorganizeLateActiveTweens();

            var deltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;

            for (var i = 0; i < lateTweensCount; i++)
            {
                var tween = lateTweens[i];
                if (tween != null)
                {
                    if (!tween.Validate())
                    {
                        tween.Kill();
                    }
                    else
                    {
                        if (tween.isActive && !tween.isPaused)
                        {
                            if (!tween.isUnscaled)
                            {
                                if (Time.timeScale == 0)
                                    continue;

                                tween.NextState(deltaTime);
                            }
                            else
                            {
                                tween.NextState(unscaledDeltaTime);
                            }

                            tween.Invoke();

                            if (tween.isCompleted)
                            {
                                tween.DefaultComplete();

                                if (tween.onCompleteCallback != null)
                                    tween.onCompleteCallback.Invoke();

                                tween.Kill();
                            }
                        }
                    }
                }
            }

            var killingTweensCount = lateKillingTweens.Count - 1;
            for (var i = killingTweensCount; i > -1; i--) RemoveActiveTween(lateKillingTweens[i]);
            lateKillingTweens.Clear();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (systemLogs)
                Debug.Log("[Tween]: Max amount of used tweens at the same time (Update - " + maxUpdateTweensAmount +
                          "; Fixed - " + maxFixedUpdateTweensAmount + "; Late - " + maxLateUpdateTweensAmount + ")");
#endif
        }

        public void Init(int tweensUpdateCount, int tweensFixedUpdateCount, int tweensLateUpdateCount, bool systemLogs)
        {
            updateTweens = new TweenCase[tweensUpdateCount];
            fixedTweens = new TweenCase[tweensFixedUpdateCount];
            lateTweens = new TweenCase[tweensLateUpdateCount];

            this.systemLogs = systemLogs;
        }

        public static void AddTween(TweenCase tween, TweenType tweenType)
        {
            switch (tweenType)
            {
                case TweenType.Update:
                    if (updateRequiresActiveReorganization)
                        ReorganizeUpdateActiveTweens();

                    tween.isActive = true;
                    tween.activeId = updateMaxActiveLookupID = updateTweensCount;

                    updateTweens[updateTweensCount] = tween;
                    updateTweensCount++;

                    hasActiveUpdateTweens = true;

#if UNITY_EDITOR
                    if (maxUpdateTweensAmount < updateTweensCount)
                        maxUpdateTweensAmount = updateTweensCount;
#endif
                    break;
                case TweenType.FixedUpdate:
                    if (fixedRequiresActiveReorganization)
                        ReorganizeFixedActiveTweens();

                    tween.isActive = true;
                    tween.activeId = fixedMaxActiveLookupID = fixedTweensCount;

                    fixedTweens[fixedTweensCount] = tween;
                    fixedTweensCount++;

                    hasActiveFixedTweens = true;

#if UNITY_EDITOR
                    if (maxFixedUpdateTweensAmount < fixedTweensCount)
                        maxFixedUpdateTweensAmount = fixedTweensCount;
#endif
                    break;
                case TweenType.LateUpdate:
                    if (lateRequiresActiveReorganization)
                        ReorganizeLateActiveTweens();

                    tween.isActive = true;
                    tween.activeId = lateMaxActiveLookupID = lateTweensCount;

                    lateTweens[lateTweensCount] = tween;
                    lateTweensCount++;

                    hasActiveLateTweens = true;

#if UNITY_EDITOR
                    if (maxLateUpdateTweensAmount < lateTweensCount)
                        maxLateUpdateTweensAmount = lateTweensCount;
#endif
                    break;
            }
        }

        public static void Pause(TweenType tweenType)
        {
            switch (tweenType)
            {
                case TweenType.Update:
                    for (var i = 0; i < updateTweensCount; i++)
                    {
                        var tween = updateTweens[i];
                        if (tween != null) tween.Pause();
                    }

                    break;
                case TweenType.FixedUpdate:
                    for (var i = 0; i < fixedTweensCount; i++)
                    {
                        var tween = fixedTweens[i];
                        if (tween != null) tween.Pause();
                    }

                    break;
                case TweenType.LateUpdate:
                    for (var i = 0; i < lateTweensCount; i++)
                    {
                        var tween = lateTweens[i];
                        if (tween != null) tween.Pause();
                    }

                    break;
            }
        }

        public static void PauseAll()
        {
            for (var i = 0; i < updateTweensCount; i++)
            {
                var tween = updateTweens[i];
                if (tween != null) tween.Pause();
            }

            for (var i = 0; i < fixedTweensCount; i++)
            {
                var tween = fixedTweens[i];
                if (tween != null) tween.Pause();
            }

            for (var i = 0; i < lateTweensCount; i++)
            {
                var tween = lateTweens[i];
                if (tween != null) tween.Pause();
            }
        }

        public static void Resume(TweenType tweenType)
        {
            switch (tweenType)
            {
                case TweenType.Update:
                    for (var i = 0; i < updateTweensCount; i++)
                    {
                        var tween = updateTweens[i];
                        if (tween != null) tween.Resume();
                    }

                    break;
                case TweenType.FixedUpdate:
                    for (var i = 0; i < fixedTweensCount; i++)
                    {
                        var tween = fixedTweens[i];
                        if (tween != null) tween.Resume();
                    }

                    break;
                case TweenType.LateUpdate:
                    for (var i = 0; i < lateTweensCount; i++)
                    {
                        var tween = lateTweens[i];
                        if (tween != null) tween.Resume();
                    }

                    break;
            }
        }

        public static void ResumeAll()
        {
            for (var i = 0; i < updateTweensCount; i++)
            {
                var tween = updateTweens[i];
                if (tween != null) tween.Resume();
            }

            for (var i = 0; i < fixedTweensCount; i++)
            {
                var tween = fixedTweens[i];
                if (tween != null) tween.Resume();
            }

            for (var i = 0; i < lateTweensCount; i++)
            {
                var tween = lateTweens[i];
                if (tween != null) tween.Resume();
            }
        }

        public static void Remove(TweenType tweenType)
        {
            switch (tweenType)
            {
                case TweenType.Update:
                    for (var i = 0; i < updateTweensCount; i++)
                    {
                        var tween = updateTweens[i];
                        if (tween != null) tween.Kill();
                    }

                    break;
                case TweenType.FixedUpdate:
                    for (var i = 0; i < fixedTweensCount; i++)
                    {
                        var tween = fixedTweens[i];
                        if (tween != null) tween.Kill();
                    }

                    break;
                case TweenType.LateUpdate:
                    for (var i = 0; i < lateTweensCount; i++)
                    {
                        var tween = lateTweens[i];
                        if (tween != null) tween.Kill();
                    }

                    break;
            }
        }

        public static void RemoveAll()
        {
            for (var i = 0; i < updateTweensCount; i++)
            {
                var tween = updateTweens[i];
                if (tween != null) tween.Kill();
            }

            for (var i = 0; i < fixedTweensCount; i++)
            {
                var tween = fixedTweens[i];
                if (tween != null) tween.Kill();
            }

            for (var i = 0; i < lateTweensCount; i++)
            {
                var tween = lateTweens[i];
                if (tween != null) tween.Kill();
            }
        }

        private static void ReorganizeUpdateActiveTweens()
        {
            if (updateTweensCount <= 0)
            {
                updateMaxActiveLookupID = -1;
                updateReorganizeFromID = -1;
                updateRequiresActiveReorganization = false;

                return;
            }

            if (updateReorganizeFromID == updateMaxActiveLookupID)
            {
                updateMaxActiveLookupID--;
                updateReorganizeFromID = -1;
                updateRequiresActiveReorganization = false;

                return;
            }

            var defaultOffset = 1;
            var tweensTempCount = updateMaxActiveLookupID + 1;

            updateMaxActiveLookupID = updateReorganizeFromID - 1;

            for (var i = updateReorganizeFromID + 1; i < tweensTempCount; i++)
            {
                var tween = updateTweens[i];
                if (tween != null)
                {
                    tween.activeId = updateMaxActiveLookupID = i - defaultOffset;

                    updateTweens[i - defaultOffset] = tween;
                    updateTweens[i] = null;
                }
                else
                {
                    defaultOffset++;
                }

                //Debug.Log("MaxActiveLookupID: " + maxActiveLookupID + "; ReorganizeFromID: " + reorganizeFromID + "; Offset: " + defaultOffset + ";");
            }

            updateRequiresActiveReorganization = false;
            updateReorganizeFromID = -1;
        }

        private static void ReorganizeFixedActiveTweens()
        {
            if (fixedTweensCount <= 0)
            {
                fixedMaxActiveLookupID = -1;
                fixedReorganizeFromID = -1;
                fixedRequiresActiveReorganization = false;

                return;
            }

            if (fixedReorganizeFromID == fixedMaxActiveLookupID)
            {
                fixedMaxActiveLookupID--;
                fixedReorganizeFromID = -1;
                fixedRequiresActiveReorganization = false;

                return;
            }

            var defaultOffset = 1;
            var tweensTempCount = fixedMaxActiveLookupID + 1;

            fixedMaxActiveLookupID = fixedReorganizeFromID - 1;

            for (var i = fixedReorganizeFromID + 1; i < tweensTempCount; i++)
            {
                var tween = fixedTweens[i];
                if (tween != null)
                {
                    tween.activeId = fixedMaxActiveLookupID = i - defaultOffset;

                    fixedTweens[i - defaultOffset] = tween;
                    fixedTweens[i] = null;
                }
                else
                {
                    defaultOffset++;
                }
            }

            fixedRequiresActiveReorganization = false;
            fixedReorganizeFromID = -1;
        }

        private static void ReorganizeLateActiveTweens()
        {
            if (lateTweensCount <= 0)
            {
                lateMaxActiveLookupID = -1;
                lateReorganizeFromID = -1;
                lateRequiresActiveReorganization = false;

                return;
            }

            if (lateReorganizeFromID == lateMaxActiveLookupID)
            {
                lateMaxActiveLookupID--;
                lateReorganizeFromID = -1;
                lateRequiresActiveReorganization = false;

                return;
            }

            var defaultOffset = 1;
            var tweensTempCount = lateMaxActiveLookupID + 1;

            lateMaxActiveLookupID = lateReorganizeFromID - 1;

            for (var i = lateReorganizeFromID + 1; i < tweensTempCount; i++)
            {
                var tween = lateTweens[i];
                if (tween != null)
                {
                    tween.activeId = lateMaxActiveLookupID = i - defaultOffset;

                    lateTweens[i - defaultOffset] = tween;
                    lateTweens[i] = null;
                }
                else
                {
                    defaultOffset++;
                }
            }

            lateRequiresActiveReorganization = false;
            lateReorganizeFromID = -1;
        }

        public static void MarkForKilling(TweenCase tween)
        {
            switch (tween.tweenType)
            {
                case TweenType.Update:
                    updateKillingTweens.Add(tween);
                    break;
                case TweenType.FixedUpdate:
                    fixedKillingTweens.Add(tween);
                    break;
                case TweenType.LateUpdate:
                    lateKillingTweens.Add(tween);
                    break;
            }
        }

        private void RemoveActiveTween(TweenCase tween)
        {
            var activeId = tween.activeId;
            tween.activeId = -1;

            switch (tween.tweenType)
            {
                case TweenType.Update:
                    updateRequiresActiveReorganization = true;

                    if (updateReorganizeFromID == -1 || updateReorganizeFromID > activeId)
                        updateReorganizeFromID = activeId;

                    updateTweens[activeId] = null;

                    updateTweensCount--;
                    hasActiveUpdateTweens = updateTweensCount > 0;
                    break;
                case TweenType.FixedUpdate:
                    fixedRequiresActiveReorganization = true;

                    if (fixedReorganizeFromID == -1 || fixedReorganizeFromID > activeId)
                        fixedReorganizeFromID = activeId;

                    fixedTweens[activeId] = null;

                    fixedTweensCount--;
                    hasActiveFixedTweens = fixedTweensCount > 0;
                    break;
                case TweenType.LateUpdate:
                    lateRequiresActiveReorganization = true;

                    if (lateReorganizeFromID == -1 || lateReorganizeFromID > activeId) lateReorganizeFromID = activeId;

                    lateTweens[activeId] = null;

                    lateTweensCount--;
                    hasActiveLateTweens = lateTweensCount > 0;
                    break;
            }
        }

        #region Update Tween

        public static int UpdateFramesCount { get; private set; }

        private static TweenCase[] updateTweens;

        public TweenCase[] UpdateTweens => updateTweens;

        private static int updateTweensCount;

        private static bool hasActiveUpdateTweens;

        private static bool updateRequiresActiveReorganization;
        private static int updateReorganizeFromID = -1;
        private static int updateMaxActiveLookupID = -1;

        private static readonly List<TweenCase> updateKillingTweens = new();

#if UNITY_EDITOR
        private static int maxUpdateTweensAmount;
#endif

        #endregion

        #region Fixed Tween

        public static int FixedUpdateFramesCount { get; private set; }

        private static TweenCase[] fixedTweens;

        public TweenCase[] FixedTweens => fixedTweens;

        private static int fixedTweensCount;

        private static bool hasActiveFixedTweens;

        private static bool fixedRequiresActiveReorganization;
        private static int fixedReorganizeFromID = -1;
        private static int fixedMaxActiveLookupID = -1;

        private static readonly List<TweenCase> fixedKillingTweens = new();

#if UNITY_EDITOR
        private static int maxFixedUpdateTweensAmount;
#endif

        #endregion

        #region Late Tween

        public static int LateUpdateFramesCount { get; private set; }

        private static TweenCase[] lateTweens;

        public TweenCase[] LateTweens => lateTweens;

        private static int lateTweensCount;

        private static bool hasActiveLateTweens;

        private static bool lateRequiresActiveReorganization;
        private static int lateReorganizeFromID = -1;
        private static int lateMaxActiveLookupID = -1;

        private static readonly List<TweenCase> lateKillingTweens = new();

#if UNITY_EDITOR
        private static int maxLateUpdateTweensAmount;
#endif

        #endregion

        #region Custom Tweens

        /// <summary>
        ///     Delayed call of delegate.
        /// </summary>
        /// <param name="callback">Callback to call.</param>
        /// <param name="delay">Delay in seconds.</param>
        public static TweenCase DelayedCall(float delay, TweenCallback callback, bool unscaledTime = false,
            TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseDefault().SetTime(delay).SetUnscaledMode(unscaledTime).OnComplete(callback)
                .SetType(tweenType).StartTween();
        }

        /// <summary>
        ///     Interpolate float value
        /// </summary>
        public static TweenCase DoColor(Color startValue, Color resultValue, float time,
            TweenCaseColor.TweenColorCallback callback, bool unscaledTime = false,
            TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseColor(startValue, resultValue, callback).SetTime(time).SetUnscaledMode(unscaledTime)
                .SetType(tweenType).StartTween();
        }

        /// <summary>
        ///     Interpolate float value
        /// </summary>
        public static TweenCase DoFloat(float startValue, float resultValue, float time,
            TweenCaseFloat.TweenFloatCallback callback, bool unscaledTime = false,
            TweenType tweenType = TweenType.Update)
        {
            return new TweenCaseFloat(startValue, resultValue, callback).SetTime(time).SetUnscaledMode(unscaledTime)
                .SetType(tweenType).StartTween();
        }

        /// <summary>
        ///     Call function in next frame
        /// </summary>
        public static TweenCase NextFrame(TweenCallback callback, int framesOffset = 1, bool unscaledTime = false,
            TweenType updateMethod = TweenType.Update)
        {
            switch (updateMethod)
            {
                case TweenType.Update:
                    return new TweenCaseUpdateNextFrame(callback, framesOffset).SetTime(float.MaxValue)
                        .SetUnscaledMode(unscaledTime).SetType(updateMethod).StartTween();
                case TweenType.FixedUpdate:
                    return new TweenCaseFixedUpdateNextFrame(callback, framesOffset).SetTime(float.MaxValue)
                        .SetUnscaledMode(unscaledTime).SetType(updateMethod).StartTween();
                case TweenType.LateUpdate:
                    return new TweenCaseLateUpdateNextFrame(callback, framesOffset).SetTime(float.MaxValue)
                        .SetUnscaledMode(unscaledTime).SetType(updateMethod).StartTween();
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Invoke coroutine from non-monobehavior script
        /// </summary>
        public static Coroutine InvokeCoroutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        /// <summary>
        ///     Stop custom coroutine
        /// </summary>
        public static void StopCustomCoroutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }

        #endregion
    }

    public enum TweenType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
}