using System;

namespace Watermelon
{
    public interface ISavable
    {
        SavableObject SaveObject();
        void LoadObject(SavableObject savableObject);
    }

    [Serializable]
    public class SavableObject
    {
    }
}