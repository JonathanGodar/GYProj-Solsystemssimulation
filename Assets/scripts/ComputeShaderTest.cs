using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;

    Cube[] cubes;

    void Start()
    {
        GenerateCubes();
    }

    struct Cube {
        public Vector3 pos;
        public Color color;
    }

    const int count = 24;

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    List<GameObject> objects = new List<GameObject>();

    Cube[] data = new Cube[count * count];

    private void CreateCube(int x, int y) {
        GameObject cube = new GameObject("Cube " + x + y * count, typeof(MeshFilter), typeof(MeshRenderer));
        cube.GetComponent<MeshFilter>().mesh = mesh;
        cube.GetComponent<MeshRenderer>().material = new Material(material);

        cube.transform.position = new Vector3(x, y, Random.Range(-0.1f, 0.1f));


        Color color = Random.ColorHSV();
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", color);

        Cube cubeData = new Cube();

        objects.Add(cube);

        cubeData.pos = cube.transform.position;
        cubeData.color = color;

        data[x + y * count] = cubeData;
    }

    private void GenerateCubes() {
        for (int i = 0; i < count; i++) {
            for (int j = 0; j < count; j++)
            {
                CreateCube(i, j);
                Debug.Log("Created cube");
            }
        }
    }

    private void RandomizeOnCpu() {
        for (int c = 0; c < objects.Count; c++) {
            GameObject obj = objects[c];
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, Random.Range(-0.1f, 0.1f));
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
        }
    }

    private void RandomizeOnGPU() {
        int colorSize = sizeof(float) * 4;
        int positionSize = sizeof(float) * 3;

        int totalSize = colorSize + positionSize;

        ComputeBuffer cubesBuffer = new ComputeBuffer(data.Length, totalSize );
        cubesBuffer.SetData(data);

        computeShader.SetBuffer(0, "cubes", cubesBuffer);
        computeShader.SetFloat("resolution", data.Length);

        computeShader.Dispatch(0, data.Length / 10, 1, 1);

        cubesBuffer.GetData(data);

        for (int i = 0; i < objects.Count; i++) {
            GameObject obj = objects[i];
            Cube cube = data[i];
            obj.transform.position = cube.pos;
            obj.GetComponent<MeshRenderer>().material.SetColor("_Color", cube.color);
        }


        cubesBuffer.Dispose();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RandomizeOnCpu();
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            RandomizeOnGPU();
        }
    }
}
