using System;
using Random = UnityEngine.Random;

namespace Watermelon
{
    [Serializable]
    public struct SafeFloat
    {
        private float offset;
        private float value;

        public SafeFloat(float value)
        {
            offset = Random.Range(-9999, 9999);

            this.value = value - offset;
        }

        public float Value => value + offset;

        public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
        {
            return new(f1.Value + f2.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}