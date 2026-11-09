using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;
    private float verticalVelocity;
    private bool isGrounded;
    private AABBCollider playerCollider;

    void Start()
    {
        playerCollider = GetComponent<AABBCollider>();
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        float horizontal = Input.GetAxisRaw("Horizontal");

        transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && wasGrounded)
        {
            verticalVelocity = jumpForce;
        }

        verticalVelocity -= gravity * Time.deltaTime;
        transform.position += new Vector3(0f, verticalVelocity * Time.deltaTime, 0f);
    }

    public void HandleCollision(AABBCollider other)
    {
        if (other.collisionType == CollisionType.Ground)
        {
            if (verticalVelocity <= 0)
            {
                float halfHeight = (playerCollider.max.y - playerCollider.min.y) / 2f;

                transform.position = new Vector3(transform.position.x, other.max.y + halfHeight, transform.position.z);

                verticalVelocity = 0f;
                isGrounded = true;
                playerCollider.UpdateBounds();
            }
        }

        if (other.collisionType == CollisionType.Obstacle)
        {
            Debug.Log("Obstacle");
        }

        if (other.collisionType == CollisionType.FallCollider)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
