using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandController : MonoBehaviour
{
    [Header("Own values")]
    [SerializeField] private Rigidbody rb;

    [SerializeField] private HingeJoint hj;

    public Transform Hand;


    [Header("Other obj")]
    [SerializeField] private Transform otherObj;
    [SerializeField] private Transform otherHandObj;
    [SerializeField] private Rigidbody otherRigidBody;
    
    
    [Header("Other")]
    [SerializeField] public Rigidbody parentRB;

    [SerializeField] private KeyCode holdHandsKey;
    [SerializeField] private KeyCode rotateClockwise;
    [SerializeField] private KeyCode rotateAntiClockwise;
    [SerializeField] private float rotateForce;

    [SerializeField] private int rotateDirection;


    [SerializeField] private bool isHoldingHands;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (parentRB == null) Debug.LogError("Parent RB is not defined");
    }

    // Update is called once per frame
    void Update()
    {
        isHoldingHands = IsKeyPressed(holdHandsKey); // Input.GetKeyDown(holdHandsKey) || Input.GetKey(holdHandsKey);
        if (IsKeyPressed(rotateAntiClockwise)) rotateDirection = 1;
        else if (IsKeyPressed(rotateClockwise)) rotateDirection = -1;
        else rotateDirection = 0;
    }

    private bool IsKeyPressed(KeyCode pKey)
    {
        return Input.GetKeyDown(pKey) || Input.GetKey(pKey);
    }

    private void FixedUpdate()
    {
        if (rotateDirection != 0)
        {
            //if (otherObj == null || otherObj.GetComponent<Rigidbody>().isKinematic)
            //if (parentRB.isKinematic)
            if (otherObj == null || parentRB.isKinematic)
            {
                transform.Rotate(Vector3.forward * rotateDirection * rotateForce * Time.fixedDeltaTime);
            }
            else
            {
                parentRB.AddTorque(Vector3.forward * rotateDirection * rotateForce * Time.fixedDeltaTime);
            }
        }
        if (otherHandObj != null)
        {
            if (!isHoldingHands)
            {
                DisconnectHands();
            }
        }

        if (!parentRB.isKinematic && otherObj != null && !otherRigidBody.isKinematic)
        {
            DisconnectHands();
        }
    }

    private void DisconnectHands()
    {
        hj.connectedBody = null;
        otherObj = null;
        otherHandObj = null;
        otherRigidBody = null;
    }

    private void AttemptHandholding(Collider other)
    {
        // If the other doesn't have a rigidbody, it's not a player
        if (other.attachedRigidbody == null) return;

        // If I'm not holding hands, I don't attach
        if (!isHoldingHands) return;

        // Get references to the other obj
        HandController otherHand = other.attachedRigidbody.transform.GetComponentInChildren<HandController>();
        Rigidbody otherRB = otherHand.parentRB;

        // Don't wanna connect with its own parent
        if (otherRB == parentRB)
        {
            Debug.Log("Preventing parental stuff");
            return;
        }


        // Set global references
        if (otherHandObj == null) otherHandObj = otherHand.Hand;
        if (otherObj == null)
        {
            otherRigidBody = otherRB;
            otherObj = otherRB.transform;
            hj.connectedAnchor = otherHandObj.position - otherObj.position;

            if (otherRB.isKinematic || !parentRB.isKinematic)
            {
                // The other one is sticking to a wall
                return;
            }

            otherHand.transform.Rotate(Vector3.forward, otherObj.eulerAngles.z, Space.Self);
            Debug.Log("here");
            // Rotate the other hand to match parent rotation
        }


        // The other player should connect with me
        if (otherRB.isKinematic || !parentRB.isKinematic)
        {
            // The other one is sticking to a wall
            return;
        }

        // If I'm attached, I connect to the other person
        if (parentRB.isKinematic)
        {
            Debug.Log("problem");
            hj.connectedBody = otherRB;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AttemptHandholding(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (otherObj == null)
        {
            AttemptHandholding(other);
        }
    }


}
