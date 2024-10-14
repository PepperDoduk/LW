using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;

    public float zoomSpeed = 10.0f;

    public Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0f).normalized;
        MoveFreeCamera(moveDirection);
        Zoom();
       
    }

    private void MoveFreeCamera(Vector3 moveDirection)
    {
        Vector3 newPosition = transform.position + moveDirection * 10 * Time.deltaTime;

        float clampedX = Mathf.Clamp(newPosition.x, minCameraBoundary.x, maxCameraBoundary.x);
        float clampedY = Mathf.Clamp(newPosition.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView + distance, 15.0f, 90.0f);
        }
    }
}
