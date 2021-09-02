using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class Simulation : MonoBehaviour
{

    [SerializeField] ComputeShader computeShader;
    
    const int PLANET_COUNT = 50_000;

    Planet[] planets = new Planet[PLANET_COUNT];
    ComputeBuffer planetBuffer;

    GameObject[] objects = new GameObject[PLANET_COUNT];


    int currPlanetId = 0;


    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    int _planetsId;
    int _planetCountId;

    int simulationKernel = 0;

    private void OnEnable()
    {
        _planetsId = Shader.PropertyToID("planets");
        _planetCountId = Shader.PropertyToID("planetCount");

        planetBuffer = new ComputeBuffer(PLANET_COUNT, PLANET_STRUCT_SIZE);
        computeShader.SetBuffer(simulationKernel, _planetsId, planetBuffer);
        computeShader.SetInt(_planetCountId, PLANET_COUNT);
        GeneratePlanets();
    }

    // 3 Vector3's + 2 floats + 1 byte ( + 3 because has to be multiple )
    const int PLANET_STRUCT_SIZE = sizeof(float) * 3 * 3 + sizeof(float) * 2;
    struct Planet
    {
        public Vector3 position;
        public Vector3 velocity;

        public Vector3 force;

        public float radius;
        public float mass;
    };

    void GeneratePlanets(bool dataOnly = false)
    {
        if (dataOnly)
        {
            for (int i = 0; i < PLANET_COUNT; i++)
            {
               CreatePlanetData(
                    currPlanetId++,
                    new Vector3(
                        Random.Range(-15, 15),
                        Random.Range(-15, 15),
                        Random.Range(-15, 15)
                        )
                    );
            }
        }
        else
        {
            for (int i = 0; i < PLANET_COUNT; i++)
            {
                CreatePlanet(
                    currPlanetId++,
                    new Vector3(
                        Random.Range(-5, 5),
                        Random.Range(-5, 5),
                        Random.Range(-5, 5)
                        )
                    );
            }
        }

    }

    void SyncPlanetsFromGpu()
    {
        planetBuffer.GetData(planets);

        for (int i = 0; i < 100; i++)
        {
            // TODO update planet mass and radius
            GameObject p = objects[i];
            p.transform.position = planets[i].position;
        }
    }

    void SyncPlanetsToGpu() {
        planetBuffer.SetData(planets);
    }



    private void CreatePlanet(int id, Vector3 position, Vector3 velocity = new Vector3(), Vector3 force = new Vector3(), float radius = 1, float mass = 1)
    {
        CreatePlanetData(id, position, velocity, force, radius, mass);
        CreatePlanetObject(id, position, radius);
    }

    private void CreatePlanetData(int id, Vector3 position, Vector3 velocity = new Vector3(), Vector3 force = new Vector3(), float radius = 1, float mass = 1)
    {
        Planet planetData = new Planet();

        planetData.position = position;
        planetData.velocity = id == 0 ? new Vector3(0, 1000, 0) : velocity;
        planetData.radius = radius;
        planetData.mass = mass;
        planetData.force = force;
        planets[id] = planetData;
    }

    private void CreatePlanetObject(int id, Vector3 position, float radius)
    {

        if (id >= 100)
            return;

        GameObject obj = new GameObject("Planet " + id, typeof(MeshFilter), typeof(MeshRenderer));
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().material = material;


        obj.transform.localScale = Vector3.one * radius;
        obj.transform.position = position;

        objects[id] = obj;
    }


    private void OnDisable()
    {
        planetBuffer.Dispose();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Stopwatch sw = new Stopwatch();

            SyncPlanetsToGpu();
            
            sw.Start();
            computeShader.Dispatch(0, Mathf.CeilToInt(PLANET_COUNT / 64f), 1, 1);
            SyncPlanetsFromGpu();
            sw.Stop();

            Debug.Log(sw.ElapsedMilliseconds); 
        }
    }
}
