using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningNodeOLD : MonoBehaviour
{
    public string mineralType;
    public float mineralValue;
    public int ResourceAmount { get; private set; }
    public float baseMiningTime;

    private MeshRenderer _mineralMesh;
    [SerializeField] private TMPro.TMP_Text progressPrefab;
    [SerializeField] private Canvas uiCanvas;
    public TMPro.TMP_Text progressText;
    public bool hasBeenMined;
    public bool beingMined;
    public bool beingMinedActively;

    public GameObject minerPositions;

    public List<GameObject> miners;

    void Awake()
    {
        _mineralMesh = transform.Find("Cone").GetComponent<MeshRenderer>();
        uiCanvas = GameObject.Find("UI").GetComponent<Canvas>();

        progressText = Instantiate(progressPrefab);
        progressText.transform.SetParent(uiCanvas.transform);
        progressText.GetComponent<UIElementOffset>().target = this.gameObject;
        progressText.GetComponent<UIElementOffset>().canvasRect = uiCanvas.GetComponent<RectTransform>();


        hasBeenMined = false;
    }

    public void FullyMined()
    {
        _mineralMesh.enabled = false;
       
        StartCoroutine(Cleanup());
    }
    IEnumerator Cleanup()
    {
        foreach (GameObject miner in miners.ToArray())
        {
            miner.GetComponent<MinerUnitController>().RemoveNode(gameObject);
        }

        float time = 0;
        while(time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(progressText);
        Destroy(this.gameObject);
    }


    public IEnumerator Mine()
    {
        beingMined = true;
        float mineralMiningTime = baseMiningTime;

        if (hasBeenMined == false && beingMined == true)
        {
            beingMined = true;
            float time = 0;
            float percentage;
            while (time < mineralMiningTime)
            {
                mineralMiningTime = baseMiningTime * (1 - ((miners.Count - 1) / 10));
                while (beingMinedActively == false)
                {
                    yield return null;
                }
                time += Time.deltaTime;
                percentage = (time / mineralMiningTime) * 100;
                progressText.text = String.Format("{0:0}", percentage) + "%";
                yield return null;
            }

            

            FullyMined();

            yield return null;
        }
        else
        {
            yield return null;
        }
    }
}
