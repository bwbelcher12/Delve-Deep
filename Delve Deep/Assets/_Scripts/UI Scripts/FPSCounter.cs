using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    TMPro.TMP_Text counterText;
    // Start is called before the first frame update
    void Start()
    {
        counterText = GetComponent<TMPro.TMP_Text>();   
    }

    // Update is called once per frame
    void Update()
    {
        float fps = (1 / Time.deltaTime);

        counterText.text = "FPS: " + String.Format("{0:0}", fps);
    }
}
