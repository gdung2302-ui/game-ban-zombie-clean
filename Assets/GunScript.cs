using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float raycastDistance = 200f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);

                ZombieH z = hit.collider.GetComponentInParent<ZombieH>();
                if (z != null)
                {
                    Debug.Log("Zombie HIT -> Kill");
                    z.TakeDamage(9999);
                }
            }
        }
    }
}
