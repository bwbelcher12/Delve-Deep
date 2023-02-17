using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiner : MonoBehaviour
{
    [SerializeField] GameObject _miner;

    public void SpawnNewMiner()
    {
        Vector3 SpawnPoint = new Vector3(GameObject.Find("World Generator").GetComponent<WorldGen>().entryPoint.x, .6f, GameObject.Find("World Generator").GetComponent<WorldGen>().entryPoint.y);

        GameObject miner = Instantiate(_miner, SpawnPoint, Quaternion.identity);
    }
}
