using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    private float cameraZoom = 8;
    [SerializeField] private float cameraZoomSensitivity;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

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

        if(Input.GetMouseButtonDown(2).Equals(true))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                tempAnchor = new GameObject();
                tempAnchor.transform.position = hit.point;
                transform.parent = tempAnchor.transform;
            }

               
        }

        if(Input.GetMouseButton(2).Equals(true))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                tempAnchor.transform.position = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(2).Equals(true))
        {
            transform.parent = null;
            Destroy(tempAnchor);
        }
        
    }

    void CameraZoom()
    {
        cameraZoom -= Input.GetAxis("Mouse ScrollWheel") * cameraZoomSensitivity;

        cameraZoom = Mathf.Clamp(cameraZoom, minSize, maxSize);

        cam.orthographicSize = cameraZoom;
    }
}
