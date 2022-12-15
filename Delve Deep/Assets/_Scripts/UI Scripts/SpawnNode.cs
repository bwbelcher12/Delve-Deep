using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    [SerializeField] GameObject _miningNode;

    public void SpawnNewNode()
    {
        GameObject node = Instantiate(_miningNode, new Vector3(Random.Range(15, -15), 0, Random.Range(6, -6)), Quaternion.identity);

        MiningNode nodeInfo = node.GetComponent<MiningNode>();

        nodeInfo.mineralType = "Ruby";
        nodeInfo.mineralValue = 15f;
        nodeInfo.baseMiningTime = 5;
        nodeInfo.resourceAmount = 100;

    }
}
