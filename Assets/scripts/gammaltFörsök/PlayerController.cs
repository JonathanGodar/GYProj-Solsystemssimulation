using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        int x = 0; // Heltal Sätt värdet till 0

        int f = 10;

        
        string y = "Det här är text"; // Text
        float z; // Decimaltal
        
    }


    [SerializeField] Vector3 movement = new Vector3(1, 1, 1);

    void Update()
    {
        transform.position += movement;
    }
}
