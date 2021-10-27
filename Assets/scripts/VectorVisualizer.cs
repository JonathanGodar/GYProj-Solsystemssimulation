using Simulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorVisualizer : MonoBehaviour
{

    public static VectorVisualizer instance {
        get;
        private set;
    }

    [SerializeField] GameObject rendererPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }


    public Action Visualize(VectorD3 origin, VectorD3 vec, Color col) {

        var newGo = Instantiate(rendererPrefab);
        var ln = newGo.GetComponent<LineRenderer>();

        ln.positionCount = 2;
        ln.SetPosition(0, (Vector3)origin);
        ln.SetPosition(1, (Vector3)(origin + vec));

        return () => { Destroy(newGo); };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
