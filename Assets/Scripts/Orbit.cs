using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float radius;


    [SerializeField] private KeyCode holdKey;



    [SerializeField] private float angle;


    [SerializeField] private float moveSpeed;


    [SerializeField] private Transform otherPlayer;


    private void Update()
    {

        if (Input.GetKeyDown(holdKey))
        {
            float tangle = Vector3.Angle(Vector3.right, otherPlayer.position - transform.position);
            Debug.Log(tangle);

            angle = tangle;
            //if (IsKeyPressed(KeyCode.J)) tangle += 10;

            Debug.Log(tangle);

            // pos = (cos(z-rotation) * 3, sin(z-rotation) * 3, 0)

            Vector3 rotatedPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            Debug.Log(rotatedPosition);
            otherPlayer.position = rotatedPosition + transform.position;

            Debug.Log(otherPlayer.position);




        }

        if (IsKeyPressed(KeyCode.K))
        {
            float tangle = Vector3.SignedAngle(Vector3.right, otherPlayer.position - transform.position, Vector3.forward);



            angle = tangle;

            angle += moveSpeed * Time.deltaTime;

            Vector3 rotatedPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            
            Vector3 newPosition = rotatedPosition + transform.position;

            Vector3 difference = newPosition - otherPlayer.position;

            otherPlayer.Translate(difference);

            //otherPlayer.position = rotatedPosition + transform.position;
        }

    }


    private bool IsKeyPressed(KeyCode pKey)
    {
        return Input.GetKeyDown(pKey) || Input.GetKey(pKey);
    }

}
