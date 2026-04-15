using UnityEngine;
using UnityEngine.Events;

public class TwoPlayerTrigger : MonoBehaviour
{

    [SerializeField] private UnityEvent TwoPlayerStay;
    [SerializeField]private int playerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody.CompareTag("Player"))
        {
            playerCount++;
        }

        if (playerCount == 2)
        {
            TwoPlayerStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            playerCount--;
        }
    }  
}
