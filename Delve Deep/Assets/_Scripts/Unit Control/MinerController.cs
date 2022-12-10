using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MinerController : RTSUnitControllerScript
{
    [SerializeField] List<GameObject> _targetNodes;
    private bool mining;
    private bool destinationIsMiningNode;

    public void AddTarget(GameObject node)
    {
        if(_targetNodes.Contains(node))
        {
            return;
        }
        _targetNodes.Add(node);
    }

    public void RemoveNode(GameObject node)
    {
        _targetNodes.Remove(node);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_targetNodes.Contains(other.gameObject))
        {
            MiningNode node = other.gameObject.transform.GetComponent<MiningNode>();

            node.beingMinedActively = true;
            StartCoroutine(Mine(node));
            GetComponent<NavMeshAgent>().destination = transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_targetNodes.Contains(other.gameObject))
        {
            mining = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetNodes.Contains(other.gameObject))
        {
            MiningNode node = other.gameObject.transform.GetComponent<MiningNode>();

            node.beingMinedActively = false;
        }
    }

    IEnumerator Mine (MiningNode node)
    {
        mining = true;
        node.beingMined = true;

        if (node.hasBeenMined == false && node.beingMined == true)
        {
            node.beingMined = true;
            float time = 0;
            float percentage;
            while (time < node.mineralMiningTime)
            {
                while (mining == false)
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

            AddTarget(hit.collider.transform.gameObject);

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
            return base.CheckHit(hit);
        }
       
    }
}
