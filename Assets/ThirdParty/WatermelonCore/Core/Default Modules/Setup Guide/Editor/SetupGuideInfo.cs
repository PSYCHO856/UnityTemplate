using UnityEngine;

namespace Watermelon
{
    [SetupTab("Info", texture = "icon_info", priority = -1)]
    [CreateAssetMenu(fileName = "Setup Guide Info", menuName = "Settings/Editor/Setup Guide Info")]
    public class SetupGuideInfo : ScriptableObject
    {
        public string gameName = "ProjectName";
        public string documentationURL = "#";

        public SetupButtonWindow[] windowButtons;
        public SetupButtonFolder[] folderButtons;
        public SetupButtonFile[] fileButtons;
    }
}