using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
public class GREEN : ColorWorld
{
    void OnEnable()
    {
        ColorActual = StaticProperties.green;

        NombreFondo = "fondo_g";
        NombreBoton = "NINGUNO";
        ColorBase = new Color(0,1,0);
    }


}