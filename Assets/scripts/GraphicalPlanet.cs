using Simulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalPlanet : MonoBehaviour
{
    Planet p;


    int i = -1;
    bool tracePath = true;
    LineRenderer ln;

    public void SetPlanet(Planet p)
    {
        this.p = p;
    }


    public void UpdatePosition()
    {
        //if (i != -1)  
        //    ln.SetPosition(i, transform.position);
        i++;
        transform.position = (Vector3)p.Position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        if (tracePath) {
            ln.positionCount += 1;
            ln.SetPosition(ln.positionCount - 1, (Vector3)p.Position);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ln = GetComponent<LineRenderer>();

        ln.positionCount = 0;
    }
}
