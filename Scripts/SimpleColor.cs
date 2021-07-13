using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColor : GameClass
{
    public Color BlackColor;
    public Color RedColor;
    public Color GreenColor;
    public Color BlueColor;

    SpriteRenderer sr;
    


    private void Awake()
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
        }
        
        
    }

    private void Update()
    {
        if(StaticProperties.red.Value)
        ColorFunction(RedColor);
        else if (StaticProperties.green.Value)
            ColorFunction(GreenColor);
        else if (StaticProperties.blue.Value)
            ColorFunction(BlueColor);
        else
            ColorFunction(BlackColor);
    }

    void ColorFunction(Color ScriptColor)
    {

        if (sr.color != ScriptColor)
        {
            sr.color = ScriptColor;
        }
    }
}
