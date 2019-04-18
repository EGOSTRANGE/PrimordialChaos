using UnityEngine;

//[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    public static GameController _ref;

    public static float Height
    {
        get { return _height; }
        set { _height = value; }
    }

    public static GameController Reference
    {
        get { return _ref; }
    }

    public static RenderTextureDescriptor MapDescriptor
    {
        get { return _offsetMapDescriptor; }
    }

    public static RenderTexture OffsetMap
    {
        get { return _offsetMap; }
    }

    private static RenderTexture _offsetMap;

    public static RenderTexture WaveMap
    {
        get { return _waveMap; }
    }

    private static RenderTexture _waveMap;

    public static RenderTexture TerrainTypeMap
    {
        get { return _terrainTypeMap; }
    }

    private static RenderTexture _terrainTypeMap;
    private static RenderTextureDescriptor _offsetMapDescriptor;
    private static float _height;

    //number of chunks
    private static int wSize = 2;

    private static int chunkSize = 128;

    public Texture initialMap;

    public Texture initialLandMap;
    // Start is called before the first frame update
    void Awake()
    {
        if (_ref != null)
            Destroy(this);
        _ref = this;
        _offsetMapDescriptor = new RenderTextureDescriptor(
            wSize * chunkSize,
            wSize * chunkSize,
            RenderTextureFormat.ARGB32);
        _offsetMap = new RenderTexture(_offsetMapDescriptor);
        _terrainTypeMap = new RenderTexture(_offsetMapDescriptor);
        //temporal stuff to see
        Graphics.Blit(initialMap, _offsetMap);
        Graphics.Blit(initialLandMap, _terrainTypeMap);
        _waveMap = new RenderTexture(_offsetMapDescriptor);

        uvScale = 1.0f / chunkSize;
        var planeScale = 1.0f / wSize;

        var mesh = ChunkMesh();

        for (int i = 0; i < wSize; i++)
        {
            for (int j = 0; j < wSize; j++)
            {
                var chunk = Instantiate(chunkPrefab);
                var filter = chunk.GetComponent<MeshFilter>();
                filter.sharedMesh = mesh;
                chunk.transform.position = new Vector3(i * chunkSize, 0, j * chunkSize);
                chunk.transform.parent = transform;

                var renderer = chunk.GetComponent<MeshRenderer>();
                var material = renderer.material;

                material.SetVector("_TerrainTypeMap_ST",
                    new Vector4(planeScale, planeScale, planeScale * i, planeScale * j));
                material.SetVector("_OffsetTex_ST",
                    new Vector4(planeScale, planeScale, planeScale * i, planeScale * j));
                material.SetVector("_SuperHeightMap_ST",
                    new Vector4(planeScale, planeScale, planeScale * i, planeScale * j));

                material.SetTexture("_TerrainTypeMap", _terrainTypeMap);
                material.SetTexture("_OffsetTex", _offsetMap);
                material.SetTexture("_SuperHeightMap", _waveMap);
                
                renderer.material = material;
            }
        }
    }

    public Material _clearMaterial;

    void Start()
    {
    }


    //Update is called once per frame
    void Update()
    {
        var temp = RenderTexture.GetTemporary(_offsetMapDescriptor);
        Graphics.Blit(_waveMap, temp);
        Graphics.Blit(temp, _waveMap, _clearMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;
    private int vertexCount;
    private float uvScale;
    public GameObject chunkPrefab;

    Mesh ChunkMesh()
    {
        vertices = new Vector3[chunkSize * chunkSize * 4];
        uvs = new Vector2[chunkSize * chunkSize * 4];
        triangles = new int[chunkSize * chunkSize * 6];

        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                AddQuad(i, j);
            }
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.bounds = new Bounds(new Vector3(chunkSize / 2, chunkSize / 2, chunkSize / 2),
            new Vector3(chunkSize, chunkSize, chunkSize));

        vertices = null;
        triangles = null;
        uvs = null;

        return mesh;
    }

    void AddQuad(int i, int j)
    {
        vertices[vertexCount + 0] = new Vector3(0 + i, 0, 0 + j);
        vertices[vertexCount + 1] = new Vector3(1 + i, 0, 0 + j);
        vertices[vertexCount + 2] = new Vector3(1 + i, 0, 1 + j);
        vertices[vertexCount + 3] = new Vector3(0 + i, 0, 1 + j);

        uvs[vertexCount + 0] = new Vector2(uvScale * i + uvScale * 0, uvScale * j + uvScale * 0);
        uvs[vertexCount + 1] = new Vector2(uvScale * i + uvScale * 1, uvScale * j + uvScale * 0);
        uvs[vertexCount + 2] = new Vector2(uvScale * i + uvScale * 1, uvScale * j + uvScale * 1);
        uvs[vertexCount + 3] = new Vector2(uvScale * i + uvScale * 0, uvScale * j + uvScale * 1);

        triangles[(i * chunkSize + j) * 6 + 0] = 0 + vertexCount;
        triangles[(i * chunkSize + j) * 6 + 1] = 3 + vertexCount;
        triangles[(i * chunkSize + j) * 6 + 2] = 2 + vertexCount;
        triangles[(i * chunkSize + j) * 6 + 3] = 0 + vertexCount;
        triangles[(i * chunkSize + j) * 6 + 4] = 2 + vertexCount;
        triangles[(i * chunkSize + j) * 6 + 5] = 1 + vertexCount;

        vertexCount += 4;
    }
}