using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Settings")]
    public GameObject zombiePrefab;       // Prefab của zombie
    public Transform[] spawnPoints;       // Các điểm spawn (có thể để trống)
    public float spawnInterval = 3f;      // Thời gian giữa các lần spawn
    public int maxZombies = 20;           // Giới hạn số zombie tối đa

    private float timer;
    private int currentZombieCount = 0;   // Đếm số zombie đang tồn tại

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnZombie();
            timer = 0f;
        }
    }

    void TrySpawnZombie()
    {
        // Kiểm tra số lượng zombie hiện tại
        if (currentZombieCount >= maxZombies)
        {
            return; // Dừng không spawn thêm
        }

        SpawnZombie();
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null)
        {
            Debug.LogWarning("⚠ Chưa gán Zombie Prefab cho ZombieSpawner!");
            return;
        }

        Vector3 spawnPos;

        // Nếu có spawn points → chọn ngẫu nhiên
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPos = randomPoint.position;
        }
        else
        {
            // Nếu không có spawn points → spawn tại vị trí của chính spawner
            spawnPos = transform.position;
        }

        GameObject newZombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        currentZombieCount++;

        // Khi zombie bị destroy → giảm số lượng
        ZombieLifeTracker tracker = newZombie.AddComponent<ZombieLifeTracker>();
        tracker.spawner = this;
    }

    // Hàm được gọi khi zombie bị hủy
    public void OnZombieDestroyed()
    {
        currentZombieCount--;
        if (currentZombieCount < 0) currentZombieCount = 0;
    }
}

// Script phụ theo dõi vòng đời của zombie
public class ZombieLifeTracker : MonoBehaviour
{
    public ZombieSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnZombieDestroyed();
        }
    }
}