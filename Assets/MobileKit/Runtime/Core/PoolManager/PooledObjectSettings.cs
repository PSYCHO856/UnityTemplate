using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class PooledObjectSettings
    {
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

        public PooledObjectSettings(bool activate = true, bool useActiveOnHierarchy = false)
        {
            this.Activate = activate;
            this.UseActiveOnHierarchy = useActiveOnHierarchy;

            ApplyPosition = false;
            ApplyEulerRotation = false;
            ApplyLocalScale = false;
            ApplyParrent = false;
        }

        public PooledObjectSettings SetActivate(bool activate)
        {
            this.Activate = activate;
            return this;
        }

        public PooledObjectSettings SetPosition(Vector3 position)
        {
            this.Position = position;
            ApplyPosition = true;
            return this;
        }

        public PooledObjectSettings SetLocalPosition(Vector3 localPosition)
        {
            this.LocalPosition = localPosition;
            ApplyLocalPosition = true;
            return this;
        }

        public PooledObjectSettings SetEulerRotation(Vector3 eulerRotation)
        {
            this.EulerRotation = eulerRotation;
            ApplyEulerRotation = true;
            return this;
        }

        public PooledObjectSettings SetLocalScale(Vector3 localScale)
        {
            this.LocalScale = localScale;
            ApplyLocalScale = true;
            return this;
        }

        public PooledObjectSettings SetParrent(Transform parrent)
        {
            this.Parrent = parrent;
            ApplyParrent = true;
            return this;
        }
    }
}