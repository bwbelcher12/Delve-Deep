using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningNode : MonoBehaviour
{
    public string mineralType;
    public float mineralValue;
    public int resourceAmount;
    public float baseMiningTime;

    private MeshRenderer _mineralMesh;
    public bool depleted;
    public GameObject minerPositions;

    public List<GameObject> miners;

    void Awake()
    {
        _mineralMesh = transform.Find("Cone").GetComponent<MeshRenderer>();
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
        _mineralMesh.enabled = false;
       
        StartCoroutine(Cleanup());
    }
    IEnumerator Cleanup()
    {
        foreach (GameObject miner in miners.ToArray())
        {
            miner.GetComponent<MinerUnitController>().RemoveNode(gameObject);
            //ensure that there aren't any stragglers when node is destroyed
            miner.GetComponent<MinerMiningController>().StopAllCoroutines();
        }

        float time = 0;
        while(time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
