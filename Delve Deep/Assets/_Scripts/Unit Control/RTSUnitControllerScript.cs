using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSUnitControllerScript: MonoBehaviour
{
    public NavMeshAgent agent;

    public bool inControlGroup;

    public Color baseColor;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        baseColor = GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    public virtual void GoToTarget(Vector3 target)
    {
        agent.destination = target;

    }

    //Check tag of targeted gameobject
    public virtual Vector3 CheckHit(RaycastHit hit)
    {
        return hit.point;
    }
}