using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 camPos;
    public float zoomValueX;
    private float zoomValueY;

    public float zoomScalar;
    public float standardZoom;

    GameObject[] players = null;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        
        SetPosition();
        
    }

    private void SetPosition()
    {
        zoomValueX = Mathf.Abs(players[0].transform.position.x - players[1].transform.position.x);
        zoomValueY = Mathf.Abs(players[0].transform.position.y - players[1].transform.position.y);

        camPos = (players[0].transform.position + players[1].transform.position) / 2;
        camPos.z = Mathf.Min(-1 * Mathf.Max(zoomValueY, zoomValueX), standardZoom);
        transform.position = camPos;
    }

    private void Update()
    {
        SetPosition();
    }

}
