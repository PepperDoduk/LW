using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;

    public float cameraZoomSize;
    private float per;

    public float zoomSpeed = 6.0f;

    public Camera mainCamera;

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            MoveCameraWithMouse();
        }

        Zoom();
    }

    private void MoveCameraWithMouse()
    {
        per = cameraZoomSize * 10;
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 difference = lastMousePosition - currentMousePosition;

        Vector3 newPosition = transform.position + new Vector3(difference.x * 0.025f, difference.y * 0.025f, 0) * cameraZoomSize;
        float clampedX = Mathf.Clamp(newPosition.x, minCameraBoundary.x, maxCameraBoundary.x);
        float clampedY = Mathf.Clamp(newPosition.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        lastMousePosition = currentMousePosition;
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        cameraZoomSize = mainCamera.orthographicSize/10;

        if (distance != 0)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize + distance, 1.3f, 30.0f);
        }
    }
}
