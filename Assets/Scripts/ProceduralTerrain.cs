using Unity.AI.Navigation;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
struct DeformJob : IJobParallelFor
{
    public NativeArray<Vector3> vertices;
    public Vector3 hitPoint;
    public float radius;
    public float strength;

    public void Execute(int index)
    {
        float dist = Vector3.Distance(vertices[index], hitPoint);
        if (dist < radius)
        {
            float deformation = math.lerp(strength, 0, dist / radius);
            Vector3 v = vertices[index];
            v.y += deformation;
            vertices[index] = v;
        }
    }
}


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ProceduralTerrain : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float scale = 0.1f;
    public float amplitude = 5f;

    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;
    private MeshCollider meshCollider;
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();

        GenerateVertices();
        GenerateTriangles();
        UpdateMesh();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                DeformTerrain(hit.point);
            }
        }
    }

    void GenerateVertices()
    {
        vertices = new Vector3[width * height];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = z * width + x;
                float y = Mathf.PerlinNoise(x * scale, z * scale) * amplitude;
                vertices[i] = new Vector3(x, y, z);
            }
        }
    }

    void GenerateTriangles()
    {
        triangles = new int[(width - 1) * (height - 1) * 6];
        int ti = 0;
        for (int z = 0; z < height - 1; z++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                int i = z * width + x;

                triangles[ti++] = i;
                triangles[ti++] = i + width;
                triangles[ti++] = i + width + 1;

                triangles[ti++] = i;
                triangles[ti++] = i + width + 1;
                triangles[ti++] = i + 1;
            }
        }
    }

    Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float u = (float)x / (width - 1);
                float v = (float)z / (height - 1);
                uvs[z * width + x] = new Vector2(u, v);
            }
        }
        return uvs;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = GenerateUVs();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;

        if (navMeshSurface != null)
        {
            navMeshSurface.RemoveData();
            navMeshSurface.BuildNavMesh();
        }
    }



    public float radius = 3f;
    public float deformationStrength = 1f;

    void DeformTerrain(Vector3 hitPoint)
    {
        var vertexArray = new NativeArray<Vector3>(vertices, Allocator.TempJob);

        DeformJob job = new DeformJob
        {
            vertices = vertexArray,
            hitPoint = hitPoint,
            radius = radius,
            strength = deformationStrength
        };

        JobHandle handle = job.Schedule(vertexArray.Length, 64);
        handle.Complete();

        vertexArray.CopyTo(vertices);
        vertexArray.Dispose();

        UpdateMesh();
    }


}
