using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode jumpKeyPlayer1;
    [SerializeField] private KeyCode jumpKeyPlayer2;

    [SerializeField] private KeyCode moveLeftKeyPlayer1;
    [SerializeField] private KeyCode moveLeftKeyPlayer2;

    [SerializeField] private KeyCode moveRightKeyPlayer1;
    [SerializeField] private KeyCode moveRightKeyPlayer2;

    [SerializeField] private KeyCode stickKeyPlayer1;
    [SerializeField] private KeyCode stickKeyPlayer2;

    [SerializeField][Range(1,2)] private int playerNumber;


    [SerializeField] private float moveForce;
    [SerializeField] private float airMoveForce;
    [SerializeField] private float jumpForce;

    private Rigidbody rb;

    private bool isGrounded;
    private bool isTouchingSurface;

    private bool wantsToJump;
    private bool movingLeft;
    private bool movingRight;

    private LayerMask layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask("Surface");
    }

    private void CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.55f, layerMask))
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    private void CheckTouchingSurface()
    {
        CheckGrounded();
        if (isGrounded)
        {
            isTouchingSurface = true;
        }
        else
        {
            if (
                Physics.Raycast(transform.position, Vector3.up, 0.55f, layerMask) || 
                Physics.Raycast(transform.position, Vector3.left, 0.55f, layerMask) || 
                Physics.Raycast(transform.position, Vector3.right, 0.55f, layerMask))
            {
                isTouchingSurface = true;
            }
            else isTouchingSurface= false;
        }
    }
    private void FixedUpdate()
    {
        if (wantsToJump)
        {
            Jump();
        }

        if (movingLeft)
        {
            if (isGrounded)
                rb.AddForce(moveForce * Vector3.left, ForceMode.Force);
            else
                rb.AddForce(airMoveForce * Vector3.left, ForceMode.Force);
        }

        if (movingRight)
        {
            if (isGrounded)
                rb.AddForce(moveForce * Vector3.right, ForceMode.Force);
            else
                rb.AddForce(airMoveForce * Vector3.right, ForceMode.Force);
        }

    }

    private void Jump()
    {
        rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        wantsToJump = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckTouchingSurface();

        if (playerNumber == 1)
        {
            //Jumping
            if (isGrounded && Input.GetKeyDown(jumpKeyPlayer1))
            {
                wantsToJump = true;
            }

            //Sticking to Walls
            if (isTouchingSurface && Input.GetKey(stickKeyPlayer1))
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }

            //Movement
            if (Input.GetKey(moveLeftKeyPlayer1))
            {
                movingLeft = true;
            }
            else movingLeft = false;

            if (Input.GetKey(moveRightKeyPlayer1))
            {
                movingRight = true;
            }
            else movingRight = false;
        }
        else
        {
            //Jumping
            if (isGrounded && Input.GetKeyDown(jumpKeyPlayer2))
            {
                wantsToJump = true;
            }

            //Sticking to Walls
            if (isTouchingSurface && Input.GetKey(stickKeyPlayer2))
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.isKinematic = false;
            }

            //Movement
            if (Input.GetKey(moveLeftKeyPlayer2))
            {
                movingLeft = true;
            }
            else movingLeft = false;

            if (Input.GetKey(moveRightKeyPlayer2))
            {
                movingRight = true;
            }
            else movingRight = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
