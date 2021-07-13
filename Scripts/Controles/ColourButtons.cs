using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourButtons : OverridableMonoBehaviour
{
    Collider2D[] colliders = new Collider2D[32];

    public Collider2D SmallCircle;
    public Collider2D BigCircle;
    public Collider2D LowerThird;
    public Collider2D MediumThird;
    public Collider2D HigherThird;

    public RectTransform zona;

    bool ColL = false;
    bool ColM = false;
    bool ColH = false;
    bool BigCircleBool = false;
    bool SmallCircleBool = false;

    List<Touch> MyTouches = new List<Touch>();

    public override void UpdateMe()
    {
        CheckInput();
    }

    public override void FixedUpdateMe()
    {
        CheckInput();
    }

    void CheckInput()
    {
         MyTouches = MyTouchArray(zona);

        TouchBehaviour.RealJump = false;

        TouchBehaviour.Colour_R = false;
        TouchBehaviour.Colour_G = false;
        TouchBehaviour.Colour_B = false;

        ColL = false;
        ColM = false;
        ColH = false;
        BigCircleBool = false;
        SmallCircleBool = false;

        if (MyTouches.Count > 0)
        {
            Physics2D.OverlapPointNonAlloc(MyTouches[0].position, colliders);

            foreach (Collider2D c in colliders)
            {
                if (c == SmallCircle)
                {
                    TouchBehaviour.RealJump = true;
                    SmallCircleBool = true;
                }

                if (c == BigCircle)
                {
                    BigCircleBool = true;
                }

                if (c == LowerThird)
                {
                    ColL = true;
                }

                if (c == MediumThird)
                {
                    ColM = true;
                }

                if (c == HigherThird)
                {
                    ColH = true;
                }
            }

            //Anillo exterior:
            if (BigCircleBool && !SmallCircleBool)
            {
                if (ColL)
                {
                    TouchBehaviour.Colour_R = true;
                }
                if (ColM)
                {
                    TouchBehaviour.Colour_G = true;
                }
                if (ColH)
                {
                    TouchBehaviour.Colour_B = true;
                }
            }

            System.Array.Clear(colliders,0,colliders.Length);
        }
    }

    List<Touch> ListName = new List<Touch>();
    Vector3[] RTpoints = new Vector3[4];
    int forcounter = 0;
    Touch myTouch = new Touch();
    public List<Touch> MyTouchArray(RectTransform Zona)
    {
        //Método para crear un Array con los toques en la zona. True si está en Start() y false si está en Update().      

        if (Input.touchCount != 0)
        {
            ListName.Clear();
            Zona.GetWorldCorners(RTpoints);

            //Usado en un futuro para evitar que este codigo se ejecute constantemente:
            //Touch[] ToquesEnZonaLastUpdate = new Touch[Input.touches.Length];
            //Array.Copy(Input.touches, ToquesEnZonaLastUpdate, Input.touches.Length);

            for (forcounter = 0; forcounter < Input.touchCount; forcounter++)
            {
                if ((Input.GetTouch(forcounter).position.x > RTpoints[0].x) && (Input.GetTouch(forcounter).position.x < RTpoints[2].x) && (Input.GetTouch(forcounter).position.y > RTpoints[0].y) && (Input.GetTouch(forcounter).position.y < RTpoints[2].y))
                {
                    ListName.Add(Input.GetTouch(forcounter));
                }

            }
            //print(ToquesEnZona.Count);
        }
        else
        {
            ListName.Clear();
        }

        return ListName;
    }
}
