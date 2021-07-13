using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : GameClass { 

    [SerializeField] enum colorpicker { RED = 1, GREEN = 2, BLUE = 3, BLACK = 4, Mantener = 0 };
    [SerializeField] colorpicker colorinicial = new colorpicker();
    public bool SobreescribirColor = false;

    public bool BlackAndWhite = false;

   
    // Update is called once per frame
    void Start()
    {
        GameClass.upArrow.color = new Color(0,1,0,0);
        GameClass.leftArrow.color = new Color(1, 0, 0, 0);
        GameClass.rightArrow.color = new Color(0, 0, 1, 0);
        StaticProperties.BlackAndWhite = BlackAndWhite;
        GameClass.colors_minigui.SetActive(!BlackAndWhite);

        //El color se acciona si es un spawn nuevo o si marcamos la opcion sobreescribir color
        if (!StaticProperties.BaseAlreadyExists||SobreescribirColor)
        {
            switch (colorinicial)
            {
                case colorpicker.RED:
                    StaticProperties.red.Value = true;
                    StaticProperties.green.Value = false;
                    StaticProperties.blue.Value = false;
                    break;
                case colorpicker.GREEN:
                    StaticProperties.red.Value = false;
                    StaticProperties.green.Value = true;
                    StaticProperties.blue.Value = false;
                    break;
                case colorpicker.BLUE:
                    StaticProperties.red.Value = false;
                    StaticProperties.green.Value = false;
                    StaticProperties.blue.Value = true;
                    break;
                case colorpicker.BLACK:
                    StaticProperties.red.Value = false;
                    StaticProperties.green.Value = false;
                    StaticProperties.blue.Value = false;
                    break;
            }
        }
    }
}
