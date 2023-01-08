using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    private float cameraZoom = 15;
    [SerializeField] private float cameraZoomSensitivity;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] float cameraMoveSpeed;

    private GameObject tempAnchor;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * -1;
        mouseY = Input.GetAxis("Mouse Y") * -1;

        CameraZoom();

        if(Input.GetMouseButton(2).Equals(true))
        {
            transform.position += new Vector3(mouseX, 0, mouseY);
        }

        if (Input.GetMouseButtonUp(2).Equals(true))
        {
            transform.parent = null;
            Destroy(tempAnchor);
        }
        
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, cameraMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -cameraMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(cameraMoveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-cameraMoveSpeed * Time.deltaTime, 0, 0);
        }

    }

    void CameraZoom()
    {
        cameraZoom -= Input.GetAxis("Mouse ScrollWheel") * cameraZoomSensitivity;

        cameraZoom = Mathf.Clamp(cameraZoom, minSize, maxSize);

        cam.orthographicSize = cameraZoom;
    }
}
