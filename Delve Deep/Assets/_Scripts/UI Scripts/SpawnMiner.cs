using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiner : MonoBehaviour
{
    [SerializeField] GameObject _miner;

    public void SpawnNewMiner()
    {
        GameObject node = Instantiate(_miner, new Vector3(0, .6f, 0), Quaternion.identity);
    }
}
