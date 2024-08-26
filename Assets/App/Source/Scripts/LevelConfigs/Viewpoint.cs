using UnityEditor;
using UnityEngine;

public class Viewpoint : MonoBehaviour
{
    public Vector3 Point => transform.position;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
        Handles.Label(Path.GetLabelPos(transform.position), "LookAt");
    }
}
