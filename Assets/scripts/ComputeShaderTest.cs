using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;

    Cube[] cubes;

    void Start()
    {

    }

    struct Cube {
        Vector3 pos;
        Color color;
    }

    int count = 10;

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    private void CreateCube(int x, int y) {
        GameObject cube = new GameObject("Cube " + x + y * count, typeof(MeshFilter), typeof(MeshCollider));
        cube.GetComponent<MeshFilter>().mesh = mesh;
        cube.GetComponent<MeshCollider>().material = material;

        Color color = Random.ColorHSV();
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", color);


    
    }

    private void RandomizeOnGPU() {



    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (renderTexture == null) {
        //    renderTexture = new RenderTexture(256, 256, 24);
        //    renderTexture.enableRandomWrite = true;
        //    renderTexture.Create();
        //}

        //computeShader.SetTexture(0, "Result", renderTexture);
        //computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
        //Graphics.Blit(renderTexture, destination);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
