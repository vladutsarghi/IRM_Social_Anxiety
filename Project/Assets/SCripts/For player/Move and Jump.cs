using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public AudioSource groundSound;
    public AudioSource podiumSound;
    private Rigidbody rb;
    private bool isGrounded;
    private bool hasTouchedPodium = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (groundSound == null)
        {
            groundSound = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + move);

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            Debug.Log("sari");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Podium") && !hasTouchedPodium)
        {
            hasTouchedPodium = true;
            Debug.Log("Touched Podium for the first time!");

            if (podiumSound != null)
            {
                podiumSound.Play();
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
