using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSUnitControllerScript: MonoBehaviour
{
    public NavMeshAgent _agent;

    public bool inControlGroup;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            AddToControlGroup();
        }
        if (Input.GetMouseButtonDown(1))
        {
            CastRay();
        }
    }

    private void AddToControlGroup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.transform.CompareTag("Unit").Equals(false))
            {
                inControlGroup = false;
            }
        }
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.transform.CompareTag("Ground").Equals(false))
            {
                GoToTarget(CheckHit(hit));
            }
            else
            {
                GoToTarget(hit.point);
            }
        }
    }
    public virtual void GoToTarget(Vector3 target)
    {
        _agent.destination = target;

    }

    //Check tag of targeted gameobject
    public virtual Vector3 CheckHit(RaycastHit hit)
    {
        return hit.point;
    }
}