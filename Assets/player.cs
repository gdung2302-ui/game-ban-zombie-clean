using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 720f;
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    [Header("Shooting Settings")]
    public Transform shootPoint;       // Nơi viên đạn được tạo ra
    public GameObject bulletPrefab;    // Prefab viên đạn
    public float bulletForce = 20f;    // Lực bắn
    public float fireRate = 0.3f;      // Thời gian giữa 2 phát bắn

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private Animator animator;
    private bool canShoot = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleShooting();
    }

    // ------------------- DI CHUYỂN -------------------
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                                + cameraTransform.eulerAngles.y;

            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle,
                turnSpeed * Time.deltaTime / 360f);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Cập nhật animation chạy
        if (animator)
            animator.SetFloat("Speed", inputDir.magnitude);
    }

    // ------------------- NHẢY -------------------
    void HandleJump()
    {
        if (IsGrounded() && velocity.y < 0)
            velocity.y = -2f;

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            velocity.y = jumpForce;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ------------------- BẮN -------------------
    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        // Bật animation bắn
        if (animator)
            animator.SetBool("isShooting", true);

        // Đợi một chút để animation ra đạn đúng lúc
        yield return new WaitForSeconds(0.1f);

        // Tạo viên đạn
        if (bulletPrefab && shootPoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(shootPoint.forward * bulletForce, ForceMode.Impulse);
        }

        // Đợi animation bắn kết thúc (tùy theo độ dài clip)
        yield return new WaitForSeconds(0.3f);

        // Tắt animation bắn
        if (animator)
            animator.SetBool("isShooting", false);

        // Cho phép bắn lại sau fireRate
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    // ------------------- KIỂM TRA CHẠM ĐẤT -------------------
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
