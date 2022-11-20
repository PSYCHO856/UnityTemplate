using UnityEngine;

//Pool module v 1.6.3
namespace Watermelon
{
    public class PooledObjectSettings
    {
        public PooledObjectSettings(bool activate = true, bool useActiveOnHierarchy = false)
        {
            Activate = activate;
            UseActiveOnHierarchy = useActiveOnHierarchy;

            ApplyPosition = false;
            ApplyEulerRotation = false;
            ApplyLocalScale = false;
            ApplyParrent = false;
        }

        public bool Activate { get; private set; }

        public bool UseActiveOnHierarchy { get; }

        public Vector3 Position { get; private set; }

        public bool ApplyPosition { get; private set; }

        public Vector3 LocalPosition { get; private set; }

        public bool ApplyLocalPosition { get; private set; }

        public Vector3 EulerRotation { get; private set; }

        public bool ApplyEulerRotation { get; private set; }

        public Vector3 LocalScale { get; private set; }

        public bool ApplyLocalScale { get; private set; }

        public Transform Parrent { get; private set; }

        public bool ApplyParrent { get; private set; }

        public PooledObjectSettings SetActivate(bool activate)
        {
            Activate = activate;
            return this;
        }

        public PooledObjectSettings SetPosition(Vector3 position)
        {
            Position = position;
            ApplyPosition = true;
            return this;
        }

        public PooledObjectSettings SetLocalPosition(Vector3 localPosition)
        {
            LocalPosition = localPosition;
            ApplyLocalPosition = true;
            return this;
        }

        public PooledObjectSettings SetEulerRotation(Vector3 eulerRotation)
        {
            EulerRotation = eulerRotation;
            ApplyEulerRotation = true;
            return this;
        }

        public PooledObjectSettings SetLocalScale(Vector3 localScale)
        {
            LocalScale = localScale;
            ApplyLocalScale = true;
            return this;
        }

        public PooledObjectSettings SetParrent(Transform parrent)
        {
            Parrent = parrent;
            ApplyParrent = true;
            return this;
        }
    }
}