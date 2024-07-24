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
    [SerializeField] private int currentMap = 0;
    [SerializeField]
    private List<Color> groundColor = new();
    [SerializeField] private Vector2 groundRate = new(2f, 1f);
    [SerializeField] private Transform groundParent;

    [Space(10)]
    [Header("Generate Chesss")]
    [SerializeField] private ChessPrefab b_vua = new(new(10f / 255f, 0, 0, 1));
    [SerializeField] private ChessPrefab b_hau = new(new(20f / 255f, 0, 0, 1));
    [SerializeField] private ChessPrefab b_tuong = new(new(30f / 255f, 0, 0, 1));
    [SerializeField] private ChessPrefab b_xe = new(new(40f / 255f, 0, 0, 1));
    [SerializeField] private ChessPrefab b_ma = new(new(50f / 255f, 0, 0, 1));
    [SerializeField] private ChessPrefab b_tot = new(new(60f / 255f, 0, 0, 1));

    [SerializeField] private ChessPrefab l_vua = new(new(10f / 255f, 1f, 1f, 1));
    [SerializeField] private ChessPrefab l_hau = new(new(10f / 255f, 1f, 1f, 1));
    [SerializeField] private ChessPrefab l_tuong = new(new(10f / 255f, 1f, 1f, 1));
    [SerializeField] private ChessPrefab l_xe = new(new(10f / 255f, 1f, 1f, 1));
    [SerializeField] private ChessPrefab l_ma = new(new(10f / 255f, 1f, 1f, 1));
    [SerializeField] private ChessPrefab l_tot = new(new(10f / 255f, 1f, 1f, 1));

    private List<ChessPrefab> chessPrefabs = new();
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
        SetupChessPrefabList();
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
    public Texture2D GetChessTexture2D()
    {
        return maps[currentMap].chessTexture;
    }
    public void LoadMap()
    {
        Texture2D mapTexture = GetMapTexture2D();
        Texture2D chessTexture = GetChessTexture2D();

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
        bool needReload = (width * height) % 2 == 0;

        for (int i = 0; i < width; i++)
        {
            if (needReload)
            {
                GetGround();
            }
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                Color currentColor = mapTexture.GetPixel(i, j);
                Color chessColor = chessTexture.GetPixel(i, j);
                int depth = GroundIndex(currentColor);
                SpawnGround(depth, i, j, chessColor);
            }
        }
    }
    public int GroundIndex(Color newColor)
    {
        for (int i = 0; i < groundColor.Count; i++)
        {
            if (ColorsAreApproximatelyEqual(groundColor[i], newColor))
            {
                return i;
            }
        }
        return 0;
    }

    private bool ColorsAreApproximatelyEqual(Color color1, Color color2)
    {
        return Mathf.Approximately(color1.r, color2.r) &&
               Mathf.Approximately(color1.g, color2.g) &&
               Mathf.Approximately(color1.b, color2.b) &&
               Mathf.Approximately(color1.a, color2.a);
    }
    public void SpawnGround(int depth, int x, int y, Color color)
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

        GameObject chess = GetChessPrefab(color);
        if (chess != null)
        {
            Vector3 chessPos = new(spawnPos.x, depth * groundRate.y + 1f, spawnPos.z);
            GameObject chessInstance = Instantiate(chess, groundParent);
            chessInstance.transform.localPosition = chessPos;
        }
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
            Color.black,
            Color.red,
            Color.green,
            Color.blue,
            new(1f,1f,0f,1f),
            new(0f,1f,1f,1f),
            new(1f,0f,1f,1f)
        };
        SetupChessPrefabList();
        prefabs?.Clear();
    }
    public void SetupChessPrefabList()
    {
        List<ChessPrefab> tempList = new()
        {
            b_vua,
            b_hau,
            b_tuong,
            b_xe,
            b_ma,
            b_tot,
            l_vua,
            l_hau,
            l_tuong,
            l_xe,
            l_ma,
            l_tot,
        };

        chessPrefabs = new(tempList);
        tempList?.Clear();
    }
    public GameObject GetChessPrefab(Color newColor)
    {
        foreach (ChessPrefab chessPrefab in chessPrefabs)
        {
            if (ColorsAreApproximatelyEqual(chessPrefab.GetColor(), newColor))
            {
                return chessPrefab.chess;
            }
        }

        return null;
    }
}
[System.Serializable]
public class ChessPrefab
{
    public GameObject chess;
    public Color color;
    public ChessPrefab(GameObject chess, Color color)
    {
        this.chess = chess;
        this.color = color;
    }
    public ChessPrefab(Color color)
    {
        this.color = color;
    }
    public Color GetColor()
    {
        return color;
    }
}