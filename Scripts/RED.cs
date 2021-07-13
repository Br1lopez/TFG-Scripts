using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class RED : ColorWorld
{

    void OnEnable()
    {
        
       ColorActual = StaticProperties.red;        
        NombreFondo = "fondo_r";
        NombreBoton = "NINGUNO";
        ColorBase = new Color(1, 0, 0);
    }

}