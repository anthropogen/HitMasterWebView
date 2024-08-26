using System.Collections;
using UnityEngine;
public class GameFactory : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    public ObjectPool<Projectile> BulletPool { get; private set; }
    private void Awake()
    {
        BulletPool = new ObjectPool<Projectile>(projectile, 10, transform);
    }
}
