using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //number of chunks
    private static int wSize = 1;

    private static int chunkSize = 128;
    
    // Start is called before the first frame update
    void Awake()
    {
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
                
                renderer.material = material;
            }
        }
    }

    void Start()
    {
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