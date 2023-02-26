using System.Linq;
using Beebyte.Obfuscator;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using TMPro;
using UnityEditor;
// using UnityEngine.Rendering.Universal;

namespace MobileKit.Editor
{
    public class GameConfigWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/MobileKit/游戏设置窗口 #%S")]        
        private static void OpenWindow()
        {
            var window = GetWindow<GameConfigWindow>("游戏设置");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            
            var customMenuStyle = new OdinMenuStyle
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f,
            };
            tree.DefaultMenuStyle = customMenuStyle;
            tree.Config.DrawSearchToolbar = true;
            tree.Config.UseCachedExpandedStates = true;
            
            // Adds the custom menu style to the tree, so that you can play around with it.
            // Once you are happy, you can press Copy C# Snippet copy its settings and paste it in code.
            // And remove the "Menu Style" menu item from the tree.
            //tree.AddObjectAtPath("Menu Style", customMenuStyle);
            
            // tree.Add("切换平台", new SwitchPlatform());
            tree.Add("Build Window", Resources.FindObjectsOfTypeAll<BuildWindow>().FirstOrDefault());
            tree.AddAllAssetsAtPath("Games", "Resources", typeof(ScriptableObject), true);
            tree.AddAllAssetsAtPath("MobileKit", "MobileKit/Resources", typeof(ScriptableObject), true);
            tree.Add("MobileKit/Player Setting", Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault());
            tree.Add("MobileKit/ObfuscatorOptions", Resources.FindObjectsOfTypeAll<Options>().FirstOrDefault());
            tree.AddAllAssetsAtPath("Fonts", "MobileKit/Fonts", typeof(TextAsset), true);
            tree.AddAllAssetsAtPath("Fonts", "MobileKit/Fonts", typeof(TMP_FontAsset), true);
            // tree.Add("URPSettings", Resources.FindObjectsOfTypeAll<UniversalRenderPipelineAsset>());

            // tree.Add("Symbols", new SymbolData());
            
            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();
            return tree;
        }
    }
}
