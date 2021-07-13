using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVGMeshUnity;

public class Testsvg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var mesh = GetComponent<SVGMesh>();
        var svg = new SVGData();
        svg.Path("417.08, 500, 500.28, 0, 333.87, 0, 417.08, 500");
        mesh.Fill(svg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
