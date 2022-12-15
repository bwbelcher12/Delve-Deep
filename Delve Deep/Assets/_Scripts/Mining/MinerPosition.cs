using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerPosition : MonoBehaviour
{
    public bool availiable;

    public GameObject parentNode;

    private void Awake()
    {
        parentNode = transform.parent.transform.parent.gameObject;
        availiable = true;
    }
}
