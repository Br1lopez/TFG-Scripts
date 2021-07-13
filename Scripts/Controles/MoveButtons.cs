using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtons : OverridableMonoBehaviour
{
    Collider2D[] colliders = new Collider2D[32];

    public Collider2D MoveLeftDownCol;
    public Collider2D MoveRightCol;
    public Collider2D MoveUpCol;
    public Collider2D MoveDownCol;

    float lerpTimer = 0;
    bool transitionHappening;
    public Material ctrl_horizontal_mat;
    public Material ctrl_vertical_mat;

    bool ControlsWereHorizontal;
    bool hasDirectionChanged
    {
        //Devuelve true si los valores de AreControlsHorizontal han cambiado
        get
        {
            if (ControlsWereHorizontal == StaticProperties.AreControlsHorizontal)
            {
                return false;
            }
            else

            {
                ControlsWereHorizontal = StaticProperties.AreControlsHorizontal;
                return true;
            }

        }
    }

    public Color activeColor;
    public Color inactiveColor;

    Metodos myMethods = new Metodos();
    Color[] colorSnapshot = new Color[2];

    public RectTransform zona;

    List<Touch> MyTouches = new List<Touch>();

    // Start is called before the first frame update

    private void Start()
    {
        SetMaterials();
    }

    // Update is called once per frame
    public override void UpdateMe()
    {
        CheckInput();
        TransitionScript();
    }

    public override void FixedUpdateMe()
    {
        CheckInput();
    }

    void CheckInput()
    {
        MyTouches = MyTouchArray(zona);

        TouchBehaviour.Move_Right = false;
        TouchBehaviour.Move_Left = false;
        TouchBehaviour.Move_Up = false;
        TouchBehaviour.Move_Down = false;

        if (MyTouches.Count > 0)
        {
            Physics2D.OverlapPointNonAlloc(MyTouches[0].position, colliders);

            foreach (Collider2D c in colliders)
            {
                if (StaticProperties.AreControlsHorizontal)
                {
                    if (c == MoveLeftDownCol)
                    {
                        TouchBehaviour.Move_Left = true;
                    }

                    if (c == MoveRightCol)
                    {
                        TouchBehaviour.Move_Right = true;
                    }
                }
                else
                {
                    if (c == MoveUpCol)
                    {
                        TouchBehaviour.Move_Up = true;
                    }

                    if (c == MoveDownCol)
                    {
                        TouchBehaviour.Move_Down = true;;
                    }
                }                
            }

            System.Array.Clear(colliders, 0, colliders.Length);
        }

    }


    void SetMaterials()
    {

        ctrl_horizontal_mat.color = StaticProperties.AreControlsHorizontal ? activeColor : inactiveColor;
        ctrl_vertical_mat.color = StaticProperties.AreControlsHorizontal ? inactiveColor : activeColor;

        //Capturamos los valores de booleanos;
        ControlsWereHorizontal = StaticProperties.AreControlsHorizontal;
    }

    void TransitionScript()
    {
        //Si los valores del mundo RGB han cambiado...
        if (hasDirectionChanged)
        {
            //Capturamos los valores de los materiales (valdrán de origen para la transición)
            TakeColorSnapShot();

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
            ctrl_horizontal_mat.color = Color.Lerp(colorSnapshot[0], StaticProperties.AreControlsHorizontal ? activeColor : inactiveColor, lerpTimer);
            ctrl_vertical_mat.color = Color.Lerp(colorSnapshot[1], StaticProperties.AreControlsHorizontal ? inactiveColor : activeColor, lerpTimer);

            if (lerpTimer >= 1)
            {
                //...cuando el timer llega a 1, fin de la transicion.
                transitionHappening = false;
            }

        }
    }

    void TakeColorSnapShot()
    {
        //Captura los valores de los colores
        colorSnapshot[0] = myMethods.ColorCopy(ctrl_horizontal_mat.color);
        colorSnapshot[1] = myMethods.ColorCopy(ctrl_vertical_mat.color);
    }


    private void OnApplicationQuit()
    {
        ctrl_horizontal_mat.color = activeColor;
        ctrl_vertical_mat.color = inactiveColor;
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
