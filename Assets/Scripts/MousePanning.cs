using UnityEngine;

public class MousePanning : MonoBehaviour
{
    // Variables
    public Camera targetCamera;
    public float panSpeed = 1.0f;
    public Vector2 panLimitX = new Vector2(-50f, 50f);
    public Vector2 panLimitY = new Vector2(-50f, 50f);

    private Vector3 lastMousePoistion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checking
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Paining functionality
        HandleMousePanning();
    }

    private void HandleMousePanning()
    {
        // Right Mouse is clicked
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePoistion = Input.mousePosition;
        }   
        // Right Mouse button is pressed
        else if (Input.GetMouseButton(1))
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePoistion;
            lastMousePoistion = Input.mousePosition;

            // Panning the camera
            Vector3 panMovement = new Vector3(-deltaMousePosition.x * panSpeed * Time.deltaTime,
                                               -deltaMousePosition.y * panSpeed * Time.deltaTime, 0f);

            targetCamera.transform.position += panMovement;

            // Clamp the camera's position within the pan limits
            Vector3 clampedPosition = targetCamera.transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, panLimitX.x, panLimitX.y);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, panLimitY.x, panLimitY.y);
            targetCamera.transform.position = clampedPosition;
        }
    }
}
