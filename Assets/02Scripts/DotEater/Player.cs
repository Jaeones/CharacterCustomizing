using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public float rotationSpeed = 360f;

    GameObject[] dots;
    CharacterController controller;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (dir.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, dir));
            transform.LookAt(transform.position + forward);
        }
        controller.Move(dir * speed * Time.deltaTime);
        animator.SetFloat("Run", controller.velocity.magnitude);

        if(GameObject.FindGameObjectsWithTag("Dot").Length == 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dot"))
        {
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
