using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
public class BLUE : ColorWorld
{
    void OnEnable()
    {

        ColorActual = StaticProperties.blue;        
        NombreFondo = "fondo_b";
        NombreBoton = "NINGUNO";
        ColorBase = new Color(0,0,1);
    }


}