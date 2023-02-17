using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningNode : MonoBehaviour
{
    public string mineralType;
    public float mineralValue;
    public int resourceAmount;
    public float miningSpeedMultiplier;

    public bool depleted;
    public GameObject minerPositions;

    public GameObject miner;

    void Awake()
    {
    }

    private void Update()
    {
        if(resourceAmount < 1)
        {
            depleted = true;
            FullyMined();
        }
    }

    private void FullyMined()
    {
        Cleanup();
    }
    private void Cleanup()
    {
 
        miner.GetComponent<MinerUnitController>().RemoveNode(gameObject);
        //Ensure that there aren't any stragglers when node is destroyed
        miner.GetComponent<MinerMiningController>().StopAllCoroutines();
        miner.GetComponent<MinerMiningController>().progressText.enabled = false;
        

        
        Destroy(this.gameObject);
    }
}
