using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Waypoint[] waypoints;
    [SerializeField] private Viewpoint viewpoint;
    public int Lenght => waypoints.Length;
    public Waypoint this[int i]
    { get { return waypoints[i]; } }

    private void OnDrawGizmos()
    {
        if (waypoints == null) return;
        if (waypoints.Length < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 1; i < waypoints.Length; i++)
        {
            Handles.color = Color.black;
            Gizmos.DrawLine(waypoints[i - 1].Point, waypoints[i].Point);
            Handles.Label(GetLabelPos(waypoints[i - 1].Point), $"{i - 1}");
        }
        Handles.Label(GetLabelPos(waypoints[waypoints.Length - 1].Point), $"{waypoints.Length - 1}");
    }
    public static Vector3 GetLabelPos(Vector3 pos)
        => new Vector3(pos.x, pos.y + .5f, pos.z);

    public Vector3 GetLookDirection()
        => viewpoint.Point - waypoints[waypoints.Length - 1].Point;
}
