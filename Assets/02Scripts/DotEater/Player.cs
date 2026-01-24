using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("이동")]
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;

    [Header("카메라")]
    public Transform cameraTransform;

    GameObject[] dots;
    CharacterController controller;
    Animator animator;
    public Text scoreText;

    float verticalVelocity = 0f;
    float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        if (cameraTransform == null)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam != null) cameraTransform = cam.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스 룩
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        if (cameraTransform != null)
        {
            cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
        }

        transform.Rotate(Vector3.up * mouseX);

        // 이동 (카메라 기준이 아닌 플레이어의 회전 기준으로 동작)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ);
        Vector3 horizontalMove = Vector3.ProjectOnPlane(move, Vector3.up).normalized;

        // 점프 및 중력
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f) verticalVelocity = -2f; // 작은 음수로 지면 붙이기

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontalMove * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // 애니메이터 (이동 속도 기반)
        if (animator != null)
        {
            float runAmount = new Vector2(moveX, moveZ).magnitude;
            animator.SetFloat("Run", runAmount * speed);
        }

        // 코인 체크 및 UI
        int remaining = GameObject.FindGameObjectsWithTag("Dot").Length;
        if (remaining == 0)
        {
            SceneManager.LoadScene("Clear");
        }
        if (scoreText != null)
        {
            scoreText.text = "남은 코인 수: " + remaining;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dot"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
