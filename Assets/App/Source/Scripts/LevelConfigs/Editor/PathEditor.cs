using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Collect Points"))
        {
            var path = (Path)target;
            var points = path.GetComponentsInChildren<Waypoint>();
            var viewpoint = path.GetComponentInChildren<Viewpoint>();
            var type = path.GetType();
            var waypointsField = type.GetField("waypoints", BindingFlags.Instance | BindingFlags.NonPublic);
            waypointsField.SetValue(path, points);
            var viewpointField = type.GetField("viewpoint", BindingFlags.Instance | BindingFlags.NonPublic);
            viewpointField.SetValue(path, viewpoint);
            EditorUtility.SetDirty(target);
        }
    }
}
