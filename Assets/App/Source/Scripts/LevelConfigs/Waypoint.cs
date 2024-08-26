using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 Point => transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Point, 0.2f);
    }
}
