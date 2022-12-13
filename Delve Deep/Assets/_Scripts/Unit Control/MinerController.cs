using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MinerController : RTSUnitControllerScript
{
    [SerializeField] List<GameObject> _targetNodes;
    private bool destinationIsMiningNode;
    private Transform closestSpot;

    public void SetCurrentTarget(GameObject node)
    {
        if(_targetNodes.Contains(node))
        {
            return;
        }

        foreach (GameObject nodeInList in _targetNodes)
        {
            nodeInList.GetComponent<MiningNode>().beingMinedActively = false;
        }

        _targetNodes.Clear();

        _targetNodes.Add(node);
        node.GetComponent<MiningNode>().miners.Add(gameObject);
    }

    public void RemoveNode(GameObject node)
    {
        _targetNodes.Remove(node);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_targetNodes.Contains(other.gameObject.transform.parent.transform.parent.transform.gameObject))
        {
            MiningNode node = other.gameObject.transform.parent.transform.parent.transform.gameObject.GetComponent<MiningNode>();
            if (node.beingMined.Equals(false))
            {
                StartCoroutine(node.Mine());
            }
            node.beingMinedActively = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_targetNodes.Contains(other.gameObject.transform.parent.transform.parent.transform.gameObject))
        {
            MiningNode node = other.gameObject.transform.parent.transform.parent.transform.gameObject.GetComponent<MiningNode>();
            node.beingMinedActively = true;
        }
    }

    public override void GoToTarget(Vector3 target)
    {
        _agent.destination = target;
        if (destinationIsMiningNode.Equals(false))
        {
            _targetNodes.Clear();
            closestSpot = transform;
        }

    }

    public override Vector3 CheckHit(RaycastHit hit)
    {
        if (hit.collider.transform.CompareTag("MiningNode").Equals(true))
        {
            destinationIsMiningNode = true;

            SetCurrentTarget(hit.collider.transform.gameObject);

            float distance = Mathf.Infinity;

            Vector3 returnSpot = transform.position;
            closestSpot = hit.collider.transform.GetComponent<MiningNode>().minerPositions.transform;

            foreach(Transform miningSpot in hit.collider.transform.GetComponent<MiningNode>().minerPositions.transform)
            {
                //Exclude spot from loop if it is already taken
                if (miningSpot.GetComponent<MinerPosition>().availiable == false)
                {
                    continue;
                }

                float tempDist = Vector3.Distance(miningSpot.position, transform.position);
                if(tempDist < distance)
                {
                    distance = tempDist;
                    returnSpot = miningSpot.position;
                    closestSpot = miningSpot;
                }
            }
            closestSpot.GetComponent<MinerPosition>().availiable = false;

            return returnSpot;
        }
        else
        {
            foreach (GameObject nodeInList in _targetNodes.ToArray())
            {
                nodeInList.GetComponent<MiningNode>().beingMinedActively = false;
            }
            _targetNodes.Clear();

            return base.CheckHit(hit);
        }
       
    }
}
