using UnityEngine;

public class WorldUI : MonoBehaviour
{
    Transform cameraPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraPos = FindFirstObjectByType<CameraController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Look at camera
        transform.rotation = cameraPos.rotation;
    }
}
