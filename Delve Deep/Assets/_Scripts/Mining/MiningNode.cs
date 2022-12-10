using System;
using System.Collections;
using UnityEngine;

public class MiningNode : MonoBehaviour
{
    public string mineralType;
    public float mineralValue;
    public float mineralMiningTime;

    private MeshRenderer _mineralMesh;
    [SerializeField] private TMPro.TMP_Text progressPrefab;
    [SerializeField] private Canvas uiCanvas;
    public TMPro.TMP_Text progressText;
    public bool hasBeenMined;
    public bool beingMined;
    public bool beingMinedActively;

    public GameObject minerPositions;

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
        float time = 0;
        while(time < 2)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(progressText);
        Destroy(this.gameObject);
    }

}
