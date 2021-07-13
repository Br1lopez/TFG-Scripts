using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFiltro : MonoBehaviour
{
    [SerializeField] Color _red;
    [SerializeField] Color _green;
    [SerializeField] Color _blue;
    [SerializeField] Color _grey;
    [SerializeField] Material _material;

    Color prevColor;
    Color nextColor;

    GameClass.BoolRef[] Colores = new GameClass.BoolRef[3];
    bool[] tempValues = new bool[3];

    bool changeColor = false;
    float startTime;
    float lerpTimer;

    private void Start()
    {

        if (StaticProperties.red.Value)
        {
            _material.color = _red;            
        }
        else if (StaticProperties.green.Value)
        {
            _material.color = _green;
        }
        else if (StaticProperties.blue.Value)
        {
            _material.color = _blue;
        }
        else
        {
            _material.color = _grey;
        }

        prevColor = _material.color;

        Colores[0] = StaticProperties.red;
        Colores[1] = StaticProperties.green;
        Colores[2] = StaticProperties.blue;

        for(int i = 0; i<Colores.Length; i++)
        {
            tempValues[i]=Colores[i].Value;
        }

    }

    private void Update()
    {
        
        for (int i = 0; i < Colores.Length; i++)
        {
            //Si hay algún cambio en los colores:
            if(tempValues[i] != Colores[i].Value)
            {
                changeColor = true;
                lerpTimer = 0;

                prevColor = _material.color;
                
                for (int j = 0; j < Colores.Length; j++)
                {

                    tempValues[j] = Colores[j].Value;  //Actualizar mis bool temporales

                    if (Colores[j].Value)  //Buscamos el nuevo color activo...
                    {
                        switch (j) //...para así definir nextColor
                        {
                            case 0:
                                nextColor = _red;
                                break;
                            case 1:
                                nextColor = _green;
                                break;
                            case 2:
                                nextColor = _blue;
                                break;
                        }
                    }
                }
                break;
            }
        }

        if (changeColor)
        {
            lerpTimer += Time.deltaTime/StaticProperties.transitionTime;
            _material.color = Color.Lerp(prevColor, nextColor, lerpTimer);

            if (lerpTimer >= 1)
            {
                changeColor = false;
            }
        }
    }
}
