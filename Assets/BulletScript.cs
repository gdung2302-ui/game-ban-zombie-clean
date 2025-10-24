using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 100;

    private void OnCollisionEnter(Collision collision)
    {
        // nếu bạn vẫn muốn bullet gây damage thay vì raycast:
        ZombieH z = collision.collider.GetComponentInParent<ZombieH>();
        if (z != null)
        {
            z.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
