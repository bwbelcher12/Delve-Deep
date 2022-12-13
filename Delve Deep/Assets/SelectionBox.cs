using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.transform.position;
        transform.localScale = transform.parent.transform.localScale;
    }
}
