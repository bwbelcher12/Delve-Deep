using UnityEngine;

public class WorldGen : MonoBehaviour
{

    [SerializeField] int width = 256;
    [SerializeField] int height = 256;

    [SerializeField] int xOffset = 0;
    [SerializeField] int yOffset = 0;

    [SerializeField] float scale = 20f;

    //Noise thresholds for each base material;
    [Header("This should be the highes value")] [Range(0, 1)] const float deepstoneThreshold = .6f;
    [Header("This should be the lowest value")] [Range(0, 1)] const float airThreshold = .4f;


    [SerializeField] GameObject baseBlockPrefab;

    //Materials
    [SerializeField] Material deepstoneMat;
    [SerializeField] Material baseStoneMat;

    private void Start()
    {
        GenerateWorld();
    }



    public void GenerateWorld()
    {
        ClearCurrentBlocks();


        Texture2D noiseTexture = new Texture2D(width, height);

        xOffset = Random.Range(-1000000, 1000000);
        yOffset = Random.Range(-1000000, 1000000);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                noiseTexture.SetPixel(x, y, color);

                if (color.r < airThreshold)
                {
                    continue;
                }

                GameObject block = Instantiate(baseBlockPrefab, baseBlockPrefab.transform.position, Quaternion.identity);
                block.transform.position = new(x, .5f, y);
                block.transform.parent = GameObject.Find("Environment").transform;
                block.transform.name = x + ", " + y;

                switch (color.r)
                {
                    case > deepstoneThreshold:
                        block.GetComponent<MeshRenderer>().material = deepstoneMat;
                        break;

                    default:
                        block.GetComponent<MeshRenderer>().material = baseStoneMat;
                        break;
                }
            }
        }

        noiseTexture.Apply();
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = ((float)x + (float)xOffset) / width * scale;

        float yCoord = ((float)y + (float)yOffset) / height * scale;


        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return new Color(sample, sample, sample);
    }

    private void ClearCurrentBlocks()
    {
        Transform environment = GameObject.Find("Environment").transform;

        for (int x = 0; x < environment.childCount; x++)
        {
            Destroy(environment.GetChild(x).gameObject);
        }
    }
}
