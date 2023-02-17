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

    private Vector3 tempAnchor;
    private Camera cam;
    private Plane movePlane;

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

        CameraMove();
        
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

    void CameraMove()
    {
        if(Input.GetMouseButtonDown(2).Equals(true))
        {
            movePlane = new Plane(Vector3.up, Vector3.up);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            //Initialise the enter variable
            float enter = 0.0f;

            if (movePlane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);

                //Move your cube GameObject to the point where you clicked
                tempAnchor = hitPoint;
            }
        }

        if (Input.GetMouseButton(2).Equals(true))
        {
            Vector3 offset = new();

            movePlane = new Plane(Vector3.up, Vector3.up);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Initialise the enter variable
            float enter = 0.0f;

            if (movePlane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);

                //Move your cube GameObject to the point where you clicked
                offset = tempAnchor - hitPoint;
            }

            transform.position += offset;
        }

        if (Input.GetMouseButtonUp(2).Equals(true))
        {
         
        }
    }

    void CameraZoom()
    {
        cameraZoom -= Input.GetAxis("Mouse ScrollWheel") * cameraZoomSensitivity;

        cameraZoom = Mathf.Clamp(cameraZoom, minSize, maxSize);

        cam.orthographicSize = cameraZoom;
    }
}
