using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerMiningController : MonoBehaviour
{
    //represents 10 seconds / speed. 1 = 10 sec, 2 = 5 sec, 3 = 3.3 sec etc...
    [Range(1, 10)]
    [SerializeField] int miningSpeed;

    [SerializeField] private TMPro.TMP_Text hoverTextPrefab;
    [SerializeField] private Canvas uiCanvas;

    private TMPro.TMP_Text progressText;
    private MinerUnitController unitController;

    private void Awake()
    {
        SetupProgressText();

        unitController = GetComponent<MinerUnitController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if unit has entered a minerPosition;
        if (unitController.targetNodes.Contains(other.GetComponent<MinerPosition>().parentNode).Equals(true))
        {
            StartCoroutine(Mine(other.GetComponent<MinerPosition>().parentNode.GetComponent<MiningNode>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<MinerPosition>().availiable = true;
        if (unitController.targetNodes.Contains(other.GetComponent<MinerPosition>().parentNode).Equals(true))
        {
            StopAllCoroutines();
            progressText.enabled = false;
        }
    }

    IEnumerator Mine(MiningNode node)
    {
        progressText.enabled = true;

        float time = 0;

        while (node.depleted.Equals(false))
        {
            float totalTime = 10 / miningSpeed;
            time += Time.deltaTime;

            float progress = (time / totalTime) * 100;

            progressText.text = String.Format("{0:0}", progress);

            if (time > totalTime)
            {
                node.resourceAmount--;
                time = 0;
            }

            yield return null;
        }

        yield return null;
    }

    private void SetupProgressText()
    {
        uiCanvas = GameObject.Find("UI").GetComponent<Canvas>();
        progressText = Instantiate(hoverTextPrefab);
        progressText.transform.SetParent(uiCanvas.transform);
        progressText.GetComponent<UIElementOffset>().target = this.gameObject;
        progressText.GetComponent<UIElementOffset>().canvasRect = uiCanvas.GetComponent<RectTransform>();
        progressText.enabled = false;
    }    
}
