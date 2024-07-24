using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public static MapGenerater instance;
    [SerializeField] private List<GameObject> grounds = new();
    private int currentGround = 0;
    [SerializeField] private GameObject soil = null;
    [SerializeField] private List<MapScriptableObject> maps = new();
    private int currentMap = 0;
    [SerializeField]
    private List<Color> groundColor = new();
    [SerializeField] private Vector2 groundRate = new(2f, 1f);
    [SerializeField] private Transform groundParent;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        LoadMap();
    }
    public GameObject GetGround()
    {
        GameObject returnObj = grounds[currentGround];
        currentGround++;
        if (currentGround == grounds.Count)
        {
            currentGround = 0;
        }
        return returnObj;
    }
    public void SetupMap(List<MapScriptableObject> newMap)
    {
        maps = new(newMap);
        newMap?.Clear();
    }
    public Texture2D GetMapTexture2D()
    {
        return maps[currentMap].mapTexture;
    }
    public void LoadMap()
    {
        Texture2D mapTexture = GetMapTexture2D();

        int width = mapTexture.width;
        int height = mapTexture.height;
        Vector3 newPos = new(-(width / 2f) * groundRate.x, 0f, -(height / 2f) * groundRate.x);
        if (groundParent != null)
        {
            DestroyImmediate(groundParent.gameObject);
        }
        if (groundParent == null)
        {
            GameObject ground = new("Ground");
            groundParent = ground.transform;
        }
        groundParent.transform.position = newPos;

        for (int i = 0; i < width; i++)
        {
            GetGround();
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                Color currentColor = mapTexture.GetPixel(i, j);
                int depth = GroundIndex(currentColor);
                SpawnGround(depth, i, j);
            }
        }
    }
    public int GroundIndex(Color newColor)
    {
        for (int i = 0; i < groundColor.Count; i++)
        {
            if (groundColor[i].Equals(newColor))
            {
                return i;
            }
        }
        return 0;
    }
    public void SpawnGround(int depth, int x, int y)
    {
        Vector3 spawnPos = new(x * groundRate.x, 0f, y * groundRate.x);
        for (int i = 0; i < depth; i++)
        {
            Vector3 spawnSoilPos = new(spawnPos.x, i * groundRate.y, spawnPos.z);
            GameObject soilInstance = Instantiate(soil, groundParent);
            soilInstance.transform.localPosition = spawnSoilPos;
        }
        Vector3 spawnGroundPos = new(spawnPos.x, depth * groundRate.y, spawnPos.z);
        GameObject groundInstance = Instantiate(GetGround(), groundParent);
        groundInstance.transform.localPosition = spawnGroundPos;
    }
    public void SetUpPrefabs(List<GameObject> prefabs, bool isSoil = false)
    {
        if (!isSoil)
        {
            grounds = new(prefabs);
        }
        else
        {
            soil = prefabs[0];
        }
        groundColor = new()
        {
            Color.white,
            Color.black
        };
        prefabs?.Clear();
    }
}
[System.Serializable]
public class MapGround
{
    public GameObject ground;
}