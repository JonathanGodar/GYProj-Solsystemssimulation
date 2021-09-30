using Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalPlanet : MonoBehaviour
{
    Planet p;


    int i = -1;
    LineRenderer ln;

    public void SetPlanet(Planet p)
    {
        this.p = p;
    }


    List<Transform> positions = new List<Transform>();


    public void UpdatePosition()
    {
        if (i != -1)  
            ln.SetPosition(i, transform.position);
        
        i++;
        
        transform.position = (Vector3)p.Position;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
    // Start is called before the first frame update
    void Start()
    {
        ln = GetComponent<LineRenderer>();
        ln.positionCount = 100_000;

    }
}
