using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TRavljen.UnitFormation;
using TRavljen.UnitFormation.Formations;



public class UnitSelectionManager: MonoBehaviour
{
    [SerializeField] List<GameObject> selectedUnits = new List<GameObject>();
    [SerializeField] Collider[] hitColliders;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private RectTransform selectionAreaTransform;
    [SerializeField] private float unitSpacing;

    private Vector3 mouseStartWorldPos;
    private Vector3 mouseEndWorldPos;
    private Vector3 mouseStartScreenPos;
    private Vector3 mouseEndScreenPos;

    bool m_Started;


    private void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
        selectionAreaTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            ControlSpecificUnit();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectionAreaTransform.gameObject.SetActive(true);

            foreach (GameObject unit in selectedUnits.ToArray())
            {
                unit.GetComponent<RTSUnitControllerScript>().inControlGroup = false;
                unit.GetComponent<Renderer>().material.SetColor("_BaseColor", unit.GetComponent<RTSUnitControllerScript>().baseColor);
                selectedUnits.Remove(unit);
            }

            ControlSingleUnit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                mouseStartWorldPos = hit.point;
                mouseStartScreenPos = Input.mousePosition;
            }
        }   
        if (Input.GetMouseButtonDown(1))
        {
            CastRay();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                mouseEndWorldPos = hit.point;
                mouseEndScreenPos = Input.mousePosition;

            }

            selectionAreaTransform.position = new Vector3((mouseStartScreenPos.x + mouseEndScreenPos.x) / 2, (mouseStartScreenPos.y + mouseEndScreenPos.y) / 2, (mouseStartScreenPos.z + mouseEndScreenPos.z) / 2);
            selectionAreaTransform.localScale = mouseStartScreenPos - mouseEndScreenPos;

            transform.position = new Vector3((mouseStartWorldPos.x + mouseEndWorldPos.x) / 2, (mouseStartWorldPos.y + mouseEndWorldPos.y) / 2, (mouseStartWorldPos.z + mouseEndWorldPos.z) / 2);
            transform.localScale = mouseStartWorldPos - (mouseEndWorldPos + new Vector3(0, 1, 0));
            MyCollisions();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);
            Array.Clear(hitColliders, 0 , hitColliders.Length);
            return;
        }
    }

    private void ControlSingleUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        { 
            //If player clicks on a unit, add to selectedUnits list and set inControlGroup flag to true, then return;
            if (hit.collider.transform.CompareTag("Unit").Equals(true))
            {
                GameObject selectedUnit = hit.collider.gameObject;

                if(selectedUnits.Contains(selectedUnit))
                {
                    return;
                }

                hit.collider.transform.GetComponent<RTSUnitControllerScript>().inControlGroup = true;
                selectedUnits.Add(hit.collider.gameObject);

                hit.collider.transform.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
                return;
            }
        }
    }

    private void ControlSpecificUnit()
    {
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //If player clicks on a unit, add to selectedUnits list and set inControlGroup flag to true, then return;
            if (hit.collider.transform.CompareTag("Unit").Equals(true))
            {
                GameObject selectedUnit = hit.collider.gameObject;

                if (selectedUnits.Contains(selectedUnit))
                {
                    return;
                }

                hit.collider.transform.GetComponent<RTSUnitControllerScript>().inControlGroup = true;
                selectedUnits.Add(selectedUnit);

                hit.collider.transform.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
                return;
            }
        }
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            QueueGoToTarget(hit);
        }
    }

    private void QueueGoToTarget(RaycastHit hit)
    {
        List<Vector3> currentPositions = new();

        foreach (GameObject unit in selectedUnits.ToArray())
        {
            currentPositions.Add(unit.transform.position);
        }



        RectangleFormation formation = new RectangleFormation((int)Math.Ceiling(Math.Sqrt(selectedUnits.Count)), unitSpacing, true); ;

        UnitsFormationPositions calculatedPositions = FormationPositioner.GetPositions(currentPositions, formation, hit.point);

        int i = 0;
        foreach (GameObject unit in selectedUnits.ToArray())
        {
            if(unit.GetComponent<MinerMiningController>() is not null)
            {
                MinerUnitController minerController = unit.GetComponent<MinerUnitController>();
                if (hit.collider.transform.CompareTag("Ground").Equals(false))
                {
                    minerController.GoToTarget(minerController.CheckHit(hit));
                }
                else
                {
                    minerController.targetNodes.Clear();
                    minerController.destinationIsMiningNode = false;
                    minerController.GoToTarget(calculatedPositions.UnitPositions[i]);
                }
                i++;
                continue;
            }

            RTSUnitControllerScript rtsController = unit.GetComponent<RTSUnitControllerScript>();

            if (hit.collider.transform.CompareTag("Ground").Equals(false))
            {
                rtsController.GoToTarget(rtsController.CheckHit(hit));
            }
            else
            {
                rtsController.GoToTarget(calculatedPositions.UnitPositions[i]);
            }
            i++;
            //yield return null;
        }
    }

    void MyCollisions()
    {
        foreach (GameObject unit in selectedUnits.ToArray())
        {
            unit.GetComponent<RTSUnitControllerScript>().inControlGroup = false;
            unit.GetComponent<Renderer>().material.SetColor("_BaseColor", unit.GetComponent<RTSUnitControllerScript>().baseColor);
            selectedUnits.Remove(unit);
        }

        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        hitColliders = Physics.OverlapBox(transform.position, new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)) / 2, Quaternion.identity, m_LayerMask);
        int i = 0;

        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].CompareTag("Unit").Equals(true))
            {
                GameObject selectedUnit = hitColliders[i].gameObject;
                if(selectedUnits.Contains(hitColliders[i].gameObject))
                {
                    i++;
                    continue;
                }

                hitColliders[i].gameObject.GetComponent<RTSUnitControllerScript>().inControlGroup = true;
                selectedUnits.Add(selectedUnit);

                hitColliders[i].gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
            }
            i++;
        }

        
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}
