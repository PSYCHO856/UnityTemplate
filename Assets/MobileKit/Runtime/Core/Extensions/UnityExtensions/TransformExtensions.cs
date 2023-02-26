using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Flip object x scale
        /// </summary>
        public static void FlipX(this Transform transform, bool flip)
        {
            transform.localScale =
                new Vector3(flip ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x),
                    transform.localScale.y, transform.localScale.z);
        }

        /// <summary>
        /// Flip object y scale
        /// </summary>
        public static void FlipY(this Transform transform, bool flip)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                flip ? -Mathf.Abs(transform.localScale.y) : Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }

        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.x = newValue;
            transform.position = v;
        }

        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.y = newValue;
            transform.position = v;
        }

        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.z = newValue;
            transform.position = v;
        }

        public static void AddPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.x += deltaValue;
            transform.position = v;
        }

        public static void AddPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.y += deltaValue;
            transform.position = v;
        }

        public static void AddPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.z += deltaValue;
            transform.position = v;
        }

        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }

        public static void AddLocalPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.x += deltaValue;
            transform.localPosition = v;
        }

        public static void AddLocalPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.y += deltaValue;
            transform.localPosition = v;
        }

        public static void AddLocalPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.z += deltaValue;
            transform.localPosition = v;
        }

        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.x = newValue;
            transform.localScale = v;
        }

        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.y = newValue;
            transform.localScale = v;
        }

        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.z = newValue;
            transform.localScale = v;
        }

        public static void AddLocalScaleX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.x += deltaValue;
            transform.localScale = v;
        }

        public static void AddLocalScaleY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.y += deltaValue;
            transform.localScale = v;
        }

        public static void AddLocalScaleZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.z += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 二维空间下使 <see cref="Transform" /> 指向目标点的算法，使用世界坐标。
        /// </summary>
        /// <param name="transform"><see cref="Transform" /> 对象。</param>
        /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
        /// <remarks>假定其 forward 向量为 <see cref="Vector3.up" />。</remarks>
        public static void LookAt2D(this Transform transform, Vector2 lookAtPoint2D)
        {
            Vector3 vector = lookAtPoint2D.ToVector3() - transform.position;
            vector.y = 0f;
            if (vector.magnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
            }
        }


        /// <summary>
        /// Try to get child
        /// </summary>
        /// <returns>child or null if child is not exists</returns>
        public static Transform TryGetChild(this Transform transform, int index)
        {
            return transform.childCount < index ? transform.GetChild(index) : null;
        }


        /// <summary>
        /// Get component if it exists or add new one
        /// </summary>
        public static T GetOrAddComponent<T>(this Transform transform) where T : Component
        {
            Component component = transform.GetComponent(typeof(T));

            if (component != null)
                return (T) component;

            return (T) transform.gameObject.AddComponent(typeof(T));
        }
        
        public static bool HasComponent<T>(this Component source) where T : Component
        {
             return source.GetComponent(typeof(T)) != null;
        }
        
        /// <summary>
        /// 判断是否面向目标
        /// </summary>
        public static bool IsFacingTarget(this Transform transform, Transform target, float dotThreshold = 0.5f)
        {
             var vectorToTarget = target.position - transform.position;
             vectorToTarget.Normalize();
             float dot = Vector3.Dot(transform.forward, vectorToTarget);
             return dot >= dotThreshold;
        }
        
        public static T[] GetComponentsOnlyInChildren<T>(this Transform transform) where T : class
        {
            var group = new List<T>();

            //collect only if its an interface or a Component
            if (typeof(T).IsInterface
                || typeof(T).IsSubclassOf(typeof(Component))
                || typeof(T) == typeof(Component))
            {
                foreach (Transform child in transform)
                {
                    if (child == transform) continue;
                    group.Add(child.GetComponent<T>());
                }
            }
            return group.ToArray();
        }
        
        
        public static T[] GetComponentsOnlyInChildrenRecursive<T>(this Transform transform) where T : class
        {
            var group = new List<T>();

            //collect only if its an interface or a Component
            if (typeof(T).IsInterface
                || typeof(T).IsSubclassOf(typeof(Component))
                || typeof(T) == typeof(Component))
            {
                foreach (Transform child in transform)
                {
                    if (child == transform) continue;
                    group.AddRange(child.GetComponentsInChildren<T>());
                }
            }
            return group.ToArray();
        }
        
        

    }
}