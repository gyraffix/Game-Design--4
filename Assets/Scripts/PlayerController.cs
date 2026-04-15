using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode moveLeftKey;
    [SerializeField] private KeyCode moveRightKey;
    [SerializeField] private KeyCode stickKey;


    [SerializeField][Range(1, 2)] private int playerNumber;


    [SerializeField] private float moveForce;
    [SerializeField] private float airMoveForce;
    [SerializeField] private float jumpForce;

    [SerializeField] private GameObject orbit;

    [SerializeField] private float radius;


    [SerializeField] private KeyCode holdKey;



    [SerializeField] private float angle;


    [SerializeField] private float moveSpeed;


    [SerializeField] private Transform otherPlayer;

    private Rigidbody rb;

    private bool isGrounded;
    private bool isTouchingSurface;
    public bool isFrozen;
    public bool inOrbit;
    public bool orbitClose;

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
            else isTouchingSurface = false;
        }
    }
    private void FixedUpdate()
    {
        if (wantsToJump)
        {
            Jump();
        }

        if (movingLeft && !inOrbit)
        {
  
            if (isGrounded)
                rb.AddForce(moveForce * Vector3.left, ForceMode.Force);
            else
                rb.AddForce(airMoveForce * Vector3.left, ForceMode.Force);
        }

        if (movingRight && !inOrbit)
        {
            if (isGrounded)
                rb.AddForce(moveForce * Vector3.right, ForceMode.Force);
            else
                rb.AddForce(airMoveForce * Vector3.right, ForceMode.Force);
        }

        if (movingLeft && inOrbit)
        {
            MoveInOrbit(-moveSpeed);

        }

        if (movingRight && inOrbit)
        {
            MoveInOrbit(moveSpeed);
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


        

        //Sticking to Walls
        if (isTouchingSurface && Input.GetKey(stickKey))
        {
            rb.isKinematic = true;
            inOrbit = false;
            orbit.SetActive(true);
            isFrozen = true;
        }
        else
        {
            rb.isKinematic = false;
            orbit.SetActive(false);
            otherPlayer.GetComponent<PlayerController>().orbitClose = false;
            isFrozen = false;
        }
        if (isFrozen) return;
        //Orbit
        if (Input.GetKey(holdKey) && orbitClose)
        {
            if (!inOrbit)
            {
                MoveInOrbit(0);
                rb.linearVelocity = Vector3.zero;
            }

            inOrbit = true;
            rb.useGravity = false;
            
        }

        else
        {
            inOrbit = false;
            rb.useGravity = true;
        }

        

        //Jumping
        if (isGrounded && Input.GetKeyDown(jumpKey) && !inOrbit)
        {
            wantsToJump = true;
        }

        //Movement
        if (Input.GetKey(moveLeftKey))
        {
            if (isFrozen) return;
        
            else
                movingLeft = true;
        }
        else movingLeft = false;

        if (Input.GetKey(moveRightKey))
        {
            if (isFrozen) return;
            
            else
                movingRight = true;
        }
        else movingRight = false;

    }

    private void MoveInOrbit(float rotationAngle)
    {
        float tangle = Vector3.SignedAngle(Vector3.right, transform.position - otherPlayer.position, Vector3.forward);

        angle = tangle;

        angle += rotationAngle * Time.fixedDeltaTime;

        Vector3 rotatedPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;

        Vector3 newPosition = rotatedPosition + otherPlayer.position;

        Vector3 difference = newPosition - transform.position;

        Debug.Log(difference);

        transform.Translate(difference, Space.World);
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard") && !isFrozen)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (other.CompareTag("Orbit"))
        {
            orbitClose = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Orbit"))
        {
            orbitClose = false;
        }
    }

}
