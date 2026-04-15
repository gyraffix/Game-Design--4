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
            if (parentRB.isKinematic)
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
                hj.connectedBody = null;
                otherObj = null;

            }
            //hj.connectedAnchor = Hand.position - transform.position;
            //hj.connectedAnchor = otherHandObj.position - otherObj.position;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        if (!isHoldingHands) return;

        HandController otherHand = other.attachedRigidbody.transform.GetComponentInChildren<HandController>();
        Rigidbody otherRB = otherHand.parentRB;

        if (otherRB == parentRB)
        {
            Debug.Log("Preventing parental stuff");
            return;
        }

        if (otherRB.isKinematic)
        {
            // The other one is sticking to a wall
            return;
        }

        if (otherHandObj == null) otherHandObj = otherHand.Hand;
        if (otherObj == null)
        {
            otherObj = otherRB.transform;
            hj.connectedAnchor = otherHandObj.position - otherObj.position;
            Debug.Log("here");
            otherHand.transform.Rotate(Vector3.forward, otherObj.eulerAngles.z, Space.Self);
        }
        //if (other.attachedRigidbody.GetComponentInChildren<HingeJoint>().connectedBody == rb) return;
        //if (other.attachedRigidbody.GetComponentInChildren<HingeJoint>().connectedBody == parentRB) return;
        //if (isHoldingHands) 


        if (parentRB.isKinematic)
        {
            hj.connectedBody = otherRB;

        }


        }


    }
