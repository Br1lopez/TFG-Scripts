using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTransitions : OverridableMonoBehaviour
{
    //Dos materiales para cada mundo (R,G,B)
    public Material red_mat;
    public Material green_mat;
    public Material blue_mat;

    public Material red_off_mat;
    public Material green_off_mat;
    public Material blue_off_mat;

    public Material filter_mat;

    public Material horizon_mat;

    //Un color para cada estado (activo,inactivo)
    public Color active_col;
    public Color inactive_col;

    public Color active_col_R;
    public Color inactive_col_R;

    public Color active_col_G;
    public Color inactive_col_G;

    public Color active_col_B;
    public Color inactive_col_B;

    public Color horizon_color_r;
    public Color horizon_color_g;
    public Color horizon_color_b;
    public Color horizon_color_main;

    bool[] boolSnapshot = new bool[3];
    Color[] colorSnapshot = new Color[8];
    Color[] nextColor = new Color[8];

    Metodos myMethods = new Metodos();

    float lerpTimer = 0;
    bool transitionHappening = false;

    private void Start()
    {
        //Valores iniciales
        if (StaticProperties.red.Value)
        {
            red_mat.color = active_col;
            red_off_mat.color = inactive_col_R;
        }
        else
        {
            red_mat.color = inactive_col;
            red_off_mat.color = active_col_R;
        }

        if (StaticProperties.green.Value)
        {
            green_mat.color = active_col;
            green_off_mat.color = inactive_col_G;
        }
        else
        {
            green_mat.color = inactive_col;
            green_off_mat.color = active_col_G;
        }

        if (StaticProperties.blue.Value)
        {
            blue_mat.color = active_col;
            blue_off_mat.color = inactive_col_B;
        }
        else
        {
            blue_mat.color = inactive_col;
            blue_off_mat.color = active_col_B;
        }
        filter_mat.color = StaticProperties.red.Value ? active_col_R: StaticProperties.green.Value ? active_col_G: StaticProperties.blue.Value ? active_col_B : Color.white;

        filter_mat.color = filter_mat.color / 3.29f;
        filter_mat.color += new Color(0.75f, 0.75f, 0.75f);


        //Capturamos los valores de booleanos RGB;
        TakeBoolSnapShot();
    }

    public override void UpdateMe()
    {
        //Si los valores del mundo RGB han cambiado...
        if (hasColorWoldChanged)
        {
            //Capturamos los valores de los materiales (valdrán de origen para la transición)
            TakeColorSnapShot();
            GetNextColor();
            //...activa la transición y pone el timer a 0.
            transitionHappening = true;
            lerpTimer = 0;
        }

        //Si la transición está activada...
        if (transitionHappening)
        {
            //...el timer suma tiempo
            lerpTimer += Time.deltaTime / StaticProperties.transitionTime;

            //...transicion de colores segun el valor del timer.
            red_mat.color = Color.Lerp(colorSnapshot[0], nextColor[0], lerpTimer);
            green_mat.color = Color.Lerp(colorSnapshot[1], nextColor[1], lerpTimer);
            blue_mat.color = Color.Lerp(colorSnapshot[2], nextColor[2], lerpTimer);
            red_off_mat.color = Color.Lerp(colorSnapshot[3], nextColor[3], lerpTimer);
            green_off_mat.color = Color.Lerp(colorSnapshot[4], nextColor[4], lerpTimer);
            blue_off_mat.color = Color.Lerp(colorSnapshot[5], nextColor[5], lerpTimer);
            filter_mat.color = Color.Lerp(colorSnapshot[6], nextColor[6], lerpTimer);
            horizon_mat.color = Color.Lerp(colorSnapshot[7], nextColor[7], lerpTimer);

            if (lerpTimer >= 1)
            {
                //...cuando el timer llega a 1, fin de la transicion.
                transitionHappening = false;
            }

        }
    }

    void OnApplicationQuit()
    {
        red_mat.color = inactive_col;
        green_mat.color = inactive_col;
        blue_mat.color = inactive_col;
        red_off_mat.color = active_col_R;
        green_off_mat.color = active_col_G;
        blue_off_mat.color = active_col_B;
    }


    //MÉTODOS:

    void TakeBoolSnapShot()
    {
        //Captura los valores de los booleanos RGB
        boolSnapshot[0] = StaticProperties.red.Value;
        boolSnapshot[1] = StaticProperties.green.Value;
        boolSnapshot[2] = StaticProperties.blue.Value;

    }

    void TakeColorSnapShot()
    {
        //Captura los valores de los colores
        colorSnapshot[0] = myMethods.ColorCopy(red_mat.color);
        colorSnapshot[1] = myMethods.ColorCopy(green_mat.color);
        colorSnapshot[2] = myMethods.ColorCopy(blue_mat.color);
        colorSnapshot[3] = myMethods.ColorCopy(red_off_mat.color);
        colorSnapshot[4] = myMethods.ColorCopy(green_off_mat.color);
        colorSnapshot[5] = myMethods.ColorCopy(blue_off_mat.color);
        colorSnapshot[6] = myMethods.ColorCopy(filter_mat.color);
        colorSnapshot[7] = myMethods.ColorCopy(horizon_mat.color);
    }

    void GetNextColor()
    {
        nextColor[0] = StaticProperties.red.Value ? active_col : inactive_col;
        nextColor[1] = StaticProperties.green.Value? active_col : inactive_col;
        nextColor[2] = StaticProperties.blue.Value? active_col : inactive_col;
        nextColor[3] = StaticProperties.red.Value? inactive_col_R : active_col_R;
        nextColor[4] = StaticProperties.green.Value? inactive_col_G : active_col_G;
        nextColor[5] = StaticProperties.blue.Value? inactive_col_B : active_col_B;
        nextColor[6] = StaticProperties.red.Value ? active_col_R : StaticProperties.green.Value ? active_col_G : StaticProperties.blue.Value ? active_col_B : Color.white;
        nextColor[6] = nextColor[6] / 3.29f;
        nextColor[6] += new Color(0.75f, 0.75f, 0.75f);
        nextColor[7] = StaticProperties.red.Value ? horizon_color_r : StaticProperties.green.Value ? horizon_color_g : StaticProperties.blue.Value ? horizon_color_b : Color.black;

    }
    

    private bool hasColorWoldChanged
    {
        //Devuelve true si los valores del mundo RGB han cambiado
        get
        {
            if (boolSnapshot[0] == StaticProperties.red.Value && boolSnapshot[1] == StaticProperties.green.Value && boolSnapshot[2] == StaticProperties.blue.Value)
            {
                return false;
            }
            else

            {
                TakeBoolSnapShot();
                return true;
            }

        }
    }

}

