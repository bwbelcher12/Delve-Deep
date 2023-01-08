using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MinerUnitController : RTSUnitControllerScript
{
    public List<GameObject> targetNodes;
    public bool destinationIsMiningNode;
    private Transform closestSpot;

    private MinerMiningController miningController;

    private void Start()
    {
        miningController = GetComponent<MinerMiningController>();
    }


    public void SetCurrentTarget(GameObject node)
    {
        if(targetNodes.Contains(node))
        {
            return;
        }

        targetNodes.Clear();
        node.GetComponent<MiningNode>().miners.Remove(gameObject);


        targetNodes.Add(node);
        node.GetComponent<MiningNode>().miners.Add(gameObject);
    }

    public void RemoveNode(GameObject node)
    {
        targetNodes.Remove(node);
    }

    public override void GoToTarget(Vector3 target)
    {
        agent.destination = target;
        
        if (destinationIsMiningNode.Equals(false))
        {
            targetNodes.Clear();
            closestSpot = transform;
            miningController.StopAllCoroutines();
            miningController.progressText.enabled = false;
        }

    }

    public override Vector3 CheckHit(RaycastHit hit)
    {

        Vector3 returnSpot = transform.position;

        if (targetNodes.Contains(hit.collider.transform.gameObject).Equals(true))
        {
            return returnSpot;
        }


        destinationIsMiningNode = true;
        SetCurrentTarget(hit.collider.transform.gameObject);

        float distance = Mathf.Infinity;
        closestSpot = hit.collider.transform.GetComponent<MiningNode>().minerPositions.transform;

        foreach (Transform miningSpot in hit.collider.transform.GetComponent<MiningNode>().minerPositions.transform)
        {
            //Exclude spot from loop if it is already taken
            if (miningSpot.GetComponent<MinerPosition>().availiable == false)
            {
                continue;
            }

            float tempDist = Vector3.Distance(miningSpot.position, transform.position);
            if (tempDist < distance)
            {
                distance = tempDist;
                returnSpot = miningSpot.position;
                closestSpot = miningSpot;
            }
        }
        closestSpot.GetComponent<MinerPosition>().availiable = false;

        return returnSpot;
    }
}
