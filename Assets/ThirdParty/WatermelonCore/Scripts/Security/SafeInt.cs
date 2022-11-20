using System;
using Random = UnityEngine.Random;

namespace Watermelon
{
    [Serializable]
    public struct SafeInt
    {
        private int offset;
        private int value;

        public SafeInt(int value)
        {
            offset = Random.Range(-9999, 9999);

            this.value = value - offset;
        }

        public int Value => value + offset;

        public static SafeInt operator +(SafeInt f1, SafeInt f2)
        {
            return new(f1.Value + f2.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}