using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandController : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;


    }



}
