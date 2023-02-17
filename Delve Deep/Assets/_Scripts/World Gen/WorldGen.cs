using UnityEngine;
using UnityEngine.AI;

public class WorldGen : MonoBehaviour
{

    [SerializeField] int width = 256;
    [SerializeField] int height = 256;

    [SerializeField] int xOffset = 0;
    [SerializeField] int yOffset = 0;

    [SerializeField] float scale = 20f;

    //Noise thresholds for each base material;
    [Header("This should be the highes value")] [Range(0, 1)] const float deepstoneThreshold = .45f;
    [Header("This should be the lowest value")] [Range(0, 1)] const float airThreshold = .75f;

    private float[,] tilemap;
    private MeshFilter[] meshFilters;
    private CombineInstance[] combine;

    [SerializeField] GameObject baseBlockPrefab;

    //Materials
    [SerializeField] Material deepstoneMat;
    [SerializeField] Material baseStoneMat;


    private Vector2 centerCoord;
    public Vector2 entryPoint;
    //private float maxDistance;

    private void Start()
    {
        centerCoord = new Vector2(width / 2, height / 2);
        entryPoint = new Vector2(width - 5, height / 2);

        GenerateWorld();
    }



    public void GenerateWorld()
    {
        ClearCurrentBlocks();

        tilemap = new float[width, height];

        xOffset = Random.Range(-1000000, 1000000);
        yOffset = Random.Range(-1000000, 1000000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap[x, y] = CalculateNoiseValue(x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateBlock(x, y, tilemap[x, y]);
            }
        }

        //CombineMesh();

    }

    private float CalculateNoiseValue(int x, int y)
    {
        float sample;
        float xCoord = ((float)x + (float)xOffset) / width * scale;
        float yCoord = ((float)y + (float)yOffset) / height * scale;

        Vector2 currentCoord = new Vector2(x, y);

        float distanceFromCenter = CalculateDistanceFromCenter(currentCoord);
        float distanceFromEntryPoint = CalculateDistanceFromEntryPoint(currentCoord);

        if(distanceFromEntryPoint < distanceFromCenter)
        {
            sample = Mathf.Pow(Mathf.PerlinNoise(xCoord, yCoord), CalculateDistanceFromEntryPoint(currentCoord) * .02f);
            return sample;
        }

        sample = Mathf.Pow(Mathf.PerlinNoise(xCoord, yCoord), CalculateDistanceFromCenter(currentCoord) * .02f);
        return sample;
    }

    private float CalculateDistanceFromCenter(Vector2 currentPoint)
    {
        float distance = Mathf.Sqrt((currentPoint.x - centerCoord.x) * (currentPoint.x - centerCoord.x) + (currentPoint.y - centerCoord.y) * (currentPoint.y - centerCoord.y));

        return (distance);
    }

    private float CalculateDistanceFromEntryPoint(Vector2 currentPoint)
    {
        float distance = Mathf.Sqrt((currentPoint.x - entryPoint.x) * (currentPoint.x - entryPoint.x) + (currentPoint.y - entryPoint.y) * (currentPoint.y - entryPoint.y));

        return (distance);
    }

    private void ClearCurrentBlocks()
    {
        Transform environment = GameObject.Find("Environment").transform;

        for (int x = 0; x < environment.childCount; x++)
        {
            Destroy(environment.GetChild(x).gameObject);
        }
    }

    private void CreateBlock(int x, int y, float noiseValue)
    {

        if (noiseValue > airThreshold)
        {
            return;
        }

        GameObject block = Instantiate(baseBlockPrefab, baseBlockPrefab.transform.position, Quaternion.identity);
        block.transform.position = new(x, .5f, y);
        block.transform.parent = GameObject.Find("Environment").transform;
        block.transform.name = x + ", " + y;

        switch (noiseValue)
        {
            case < deepstoneThreshold:
                block.GetComponent<MeshRenderer>().material = deepstoneMat;
                break;

            default:
                block.GetComponent<MeshRenderer>().material = baseStoneMat;
                break;
        }
    }

    private void CombineMesh()
    {
        Transform environment = GameObject.Find("Environment").transform;

        meshFilters = environment.GetComponentsInChildren<MeshFilter>();
        combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        environment.GetComponent<MeshFilter>().mesh = new Mesh();
        environment.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        environment.gameObject.SetActive(true);
    }

}
