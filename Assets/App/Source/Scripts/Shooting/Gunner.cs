using UnityEngine;

public class Gunner : MonoBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private GameFactory factory;
    [SerializeField, Range(0, 100)] float pointDistance;
    [SerializeField, Range(0, 180)] private float maxShootAngle;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 25, Color.red);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point;
            Vector3 direction;
            if (Physics.Raycast(ray, out hit))
            {
                point = hit.point;
            }
            else
            {
                point = ray.GetPoint(pointDistance);
            }
            var angle = Vector3.SignedAngle(point - transform.position, transform.forward, Vector3.up);
            if ((Mathf.Abs(angle)) > maxShootAngle)
                return;
            direction = point - gun.ShootPos;
            Quaternion rotation = Quaternion.LookRotation(direction);
            factory.BulletPool.GetAt(gun.ShootPos, rotation);

        }
    }
}
