using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Collect Platforms"))
        {
            var levelConfig = (LevelConfig)target;
            var type = levelConfig.GetType();
            var platfomrProp = type.GetProperty("Platforms");
            var platforms = levelConfig.GetComponentsInChildren<Platform>();
            platfomrProp.SetValue(levelConfig, platforms);
            EditorUtility.SetDirty(target);
        }
    }
}
