using UnityEngine;

public class ZombieH : MonoBehaviour
{

    public int health = 100;

    void Start()
    {
        // debug: kiểm tra script hoạt động
        // Debug.Log("ZombieH Start on " + gameObject.name);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        // bạn có thể thay bằng animation/disable navmesh agent...
        Destroy(gameObject);
    }
}
