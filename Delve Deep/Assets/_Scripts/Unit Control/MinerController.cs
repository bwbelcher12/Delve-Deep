using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MinerController : RTSUnitControllerScript
{
    [SerializeField] List<GameObject> _targetNodes;
    [SerializeField] private bool mining;
    private bool destinationIsMiningNode;

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
                StartCoroutine(Mine(node));

            }
            node.beingMinedActively = true;
            mining = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_targetNodes.Contains(other.gameObject.transform.parent.transform.parent.transform.gameObject))
        {
            MiningNode node = other.gameObject.transform.parent.transform.parent.transform.gameObject.GetComponent<MiningNode>();
            node.beingMinedActively = true;
            mining = true;
        }
    }

    IEnumerator Mine (MiningNode node)
    {
        node.beingMined = true;

        if (node.hasBeenMined == false && node.beingMined == true)
        {
            node.beingMined = true;
            float time = 0;
            float percentage;
            while (time < node.mineralMiningTime)
            {
                while (mining == false || node.beingMinedActively == false)
                {
                    yield return null;
                }
                time += Time.deltaTime;
                percentage = (time / node.mineralMiningTime) * 100;
                node.progressText.text = String.Format("{0:0}", percentage) + "%";
                yield return null;
            }

            

            node.FullyMined();

            RemoveNode(node.gameObject);

            yield return null;
        }
        else
        {
            yield return null;
        }
    }

    public override void GoToTarget(Vector3 target)
    {
        _agent.destination = target;
        if (destinationIsMiningNode.Equals(false))
        {
            _targetNodes.Clear();
        }

    }

    public override Vector3 CheckHit(RaycastHit hit)
    {
        mining = false;


        if (hit.collider.transform.CompareTag("MiningNode").Equals(true))
        {
            destinationIsMiningNode = true;

            SetCurrentTarget(hit.collider.transform.gameObject);

            float distance = Mathf.Infinity;

            Vector3 returnSpot = hit.collider.transform.position;

            foreach(Transform miningSpot in hit.collider.transform.GetComponent<MiningNode>().minerPositions.transform)
            {
                float tempDist = Vector3.Distance(miningSpot.position, transform.position);
                if(tempDist < distance)
                {
                    distance = tempDist;
                    returnSpot = miningSpot.position;
                }
            }

            return returnSpot;
        }
        else
        {
            foreach (GameObject nodeInList in _targetNodes)
            {
                nodeInList.GetComponent<MiningNode>().beingMinedActively = false;
            }
            _targetNodes.Clear();

            return base.CheckHit(hit);
        }
       
    }
}
