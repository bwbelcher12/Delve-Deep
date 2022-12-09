using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUnit : MonoBehaviour
{
    [SerializeField] private Transform _target;

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position;
    }

    public void NewTarget(Transform target)
    {
        _target = target;
    }
}
