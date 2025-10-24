using UnityEngine;

public class ZombieHealth : MonoBehaviour
{

    public void TakeHit()
    {
        Destroy(gameObject);
    }
}
