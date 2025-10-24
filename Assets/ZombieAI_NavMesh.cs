using UnityEngine;
using UnityEngine.AI;

public class ZombieAI_NavMesh : MonoBehaviour
{
    [Header("Zombie Settings")]
    public Transform target;              // Mục tiêu (người chơi)
    public float detectionRadius = 200f;   // Bán kính tìm người chơi
    public float repathInterval = 2f;   // Thời gian cập nhật đường đi lại
    public float randomWanderRadius = 0f; // Nếu không thấy player → đi lang thang

    private NavMeshAgent agent;
    private float repathTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindTarget();
    }

    void Update()
    {
        if (agent == null) return;

        repathTimer += Time.deltaTime;
        if (repathTimer >= repathInterval)
        {
            repathTimer = 0f;
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        // Nếu tìm thấy player trong phạm vi
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= detectionRadius)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                return;
            }
        }

        // Nếu không thấy player → đi lang thang nhẹ quanh chỗ hiện tại
        WanderRandomly();
    }

    void WanderRandomly()
    {
        Vector3 randomDir = Random.insideUnitSphere * randomWanderRadius;
        randomDir += transform.position;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, randomWanderRadius, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }

    void FindTarget()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    // Khi zombie spawn mới → đảm bảo lập tức tìm player
    void OnEnable()
    {
        FindTarget();
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}