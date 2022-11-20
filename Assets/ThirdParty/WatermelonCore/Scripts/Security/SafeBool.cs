using System;
using Random = UnityEngine.Random;

namespace Watermelon
{
    [Serializable]
    public struct SafeBool
    {
        private int trueValue;
        private int value;

        public SafeBool(bool value)
        {
            trueValue = Random.Range(-9999, 9999);

            if (value)
                this.value = trueValue;
            else
                this.value = Random.Range(-9999, 9999);
        }

        public bool Value
        {
            get
            {
                if (value == trueValue)
                    return true;

                return false;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}