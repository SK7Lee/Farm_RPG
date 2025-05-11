
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetZ = 6f;
    public float smoothing = 8f;
    Transform playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //first position
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);   
    }

}


