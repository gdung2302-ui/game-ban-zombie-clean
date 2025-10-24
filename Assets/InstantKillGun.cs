using UnityEngine;

public class InstantKillGun : MonoBehaviour
{
    public string zombieTag = "Zombie";
    public float range = 100f;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag(zombieTag))
            {
                Destroy(hit.collider.gameObject); // giết zombie
            }
        }
    }
}