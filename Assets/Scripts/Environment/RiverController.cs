using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RiverController : MonoBehaviour
{ 
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public Transform checkpoints;
    public List<Vector3> verticeList;
    public List<int> triangleList;
    public River data;
    public GameObject checkpointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;

        CreateCheckpointsFromData();

        BuildRiverMesh();
    }

    private void Update()
    {
        BuildRiverMesh();
    }

    private void CreateCheckpointsFromData()
    {
        foreach (Transform child in checkpoints.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (RiverCheckpointData checkpointData in data.checkpoints)
        {
            GameObject child = Instantiate(checkpointPrefab, checkpoints, true);
            child.transform.position = new Vector3(checkpointData.x, checkpointData.y, checkpointData.z);
            child.transform.eulerAngles = new Vector3(0, checkpointData.rotation, 0);
            child.transform.localScale = new Vector3(1, 1, checkpointData.width);
        }
    }

    void BuildRiverMesh()
    {
        UpdateRiverData();
        CreateShape();
        UpdateMesh();
    }

    Vector3[] GetVerticesByCheckpoint(Transform checkpoint)
    {
        Vector3 position = checkpoint.position;
        float scale = checkpoint.localScale.z;
        Vector3 pointLeft = position + Quaternion.AngleAxis(checkpoint.eulerAngles.y, Vector3.up) * new Vector3(0, 0, 1) * scale;
        Vector3 pointRight = position + Quaternion.AngleAxis(checkpoint.eulerAngles.y, Vector3.up) * new Vector3(0, 0, -1) * scale;

        return new Vector3[]
        {
            pointLeft,
            pointRight
        };
    }

    void CreateShape()
    {
        vertices = verticeList.ToArray();
        triangles = triangleList.ToArray();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void UpdateRiverData()
    {
        verticeList = new List<Vector3>();
        triangleList = new List<int>();

        data.checkpoints = new List<RiverCheckpointData>();
        for (int i = 0; i < checkpoints.childCount; i++)
        {
            Transform child = checkpoints.GetChild(i);
            Vector3[] checkpointVertices = GetVerticesByCheckpoint(child);
            verticeList.Add(checkpointVertices[0]);
            verticeList.Add(checkpointVertices[1]);
            
            if (i+1 < checkpoints.childCount) {
                triangleList.Add(i*2);
                triangleList.Add(i*2+1);
                triangleList.Add(i*2+2);
                triangleList.Add(i*2+1);
                triangleList.Add(i*2+3);
                triangleList.Add(i*2+2);
            }

            RiverCheckpointData checkpointData = new RiverCheckpointData();
            Vector3 childPos = child.position;
            checkpointData.x = childPos.x;
            checkpointData.y = childPos.y;
            checkpointData.z = childPos.z;
            checkpointData.width = child.localScale.z;
            checkpointData.rotation = child.eulerAngles.y;
            data.checkpoints.Add(checkpointData);
        }
    }

    private void OnApplicationQuit()
    {
        AssetDatabase.SaveAssets();
    }
}
