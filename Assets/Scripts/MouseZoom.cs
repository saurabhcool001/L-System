using UnityEngine;

public class MouseZoom : MonoBehaviour
{
    // Variables
    // The camera to apply zoom
    public Camera targetCamera;
    // Speed of zoom
    public float zoomSpeed = 10f;
    // Minimum/Maximum zoom level
    public float minZoom = -20f; 
    public float maxZoom = 20f; 

    void Start()
    {
        // If no camera is assigned, use the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Update()
    {
        // Get the scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            if (targetCamera.orthographic)
            {
                // Adjust cameras
                targetCamera.orthographicSize -= scrollInput * zoomSpeed;
                targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                // Adjust field of view for perspective cameras
                targetCamera.fieldOfView -= scrollInput * zoomSpeed;
                targetCamera.fieldOfView = Mathf.Clamp(targetCamera.fieldOfView, minZoom, maxZoom);
            }
        }
    }
}
