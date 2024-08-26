using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float speed = 5;
    [SerializeField, Range(0, 1000)] private float iteractForce = 5;
    [SerializeField, Range(0, 100)] private float lifeTime;
    [SerializeField, Range(0, 100)] private float damage;
    [SerializeField] private Rigidbody rb;
    private float deathTime;
    private void OnEnable()
    {
        deathTime = Time.time + lifeTime;
    }
    private void Update()
    {
        if (deathTime < Time.time)
            DisableProjectile();
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PartBody partBody))
        {
            var direction = (collision.gameObject.transform.position - transform.position).normalized;

            partBody.IteractWithProjectile(direction * iteractForce, damage);
        }
        DisableProjectile();
    }

    private void DisableProjectile()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
