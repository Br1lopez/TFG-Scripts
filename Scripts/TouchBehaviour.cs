using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TouchBehaviour : GameClass
{


    public TouchZone Colour = new TouchZone(true);   

    [SerializeField] public static float SwipeDistance = 2f;
    Touch[] ToquesEnZonaLastUpdate;         
    
    Gesture LastSwipe = Gesture.None;
    bool IsFirstPhaseFinished = false;
    bool IsGestureFinished = false;
    LineDrawer LineDebug = new LineDrawer();
    IEnumerator JumpClock;
    bool IsJumpActivatedThroughCoroutine = false;
    BoolRef IsJumpCancelled = new BoolRef(false);
    Vector2 pos1;
    Vector2 pos2;
    Vector2 mov;

    float DoubleTapTimer = 0.7f;
    bool CanDoubleTapHappen = false;

    bool RepeatableSwipeSent = false;
    



    //Controles: alojados todos aquí. Los colliders manejan su valor desde los prefabs de los controles.
    public static bool Move_Left;
    public static bool Move_Right;
    public static bool Move_Up;
    public static bool Move_Down;

    public static bool RealJump;
    public static bool Colour_R;
    public static bool Colour_G;
    public static bool Colour_B;

    bool HoldingJump = false;
    public static bool Jump= false;

    private void Awake()
    {
        tb = this;
    }

    private void Update()
    {
        
        //GestureDefiner(Colour);

        //LineDebug.DrawLineInGameView(pos1, pos2, Color.white);
        //print(MainGesture.Value + " " + Angulo + " " + mov.magnitude);
        //print(Colour.Gesto.Value);       
        if (!Jump&&RealJump&&!HoldingJump)
        {
            Jump = true;
            HoldingJump = true;
        }
        else if (Jump)
        {
            Jump = false;
        }
        if (!RealJump)
        {
            HoldingJump = false;
        }

    }
    void Start()
    {

        Colour.Gesto.Value = Gesture.None;
        //MyTouchArrayGetter(true);
        JumpClock = null;
        IsJumpCancelled = player.mov.IsJumpCancelled;
        IsJumpCancelled.Value = false;


    }

    public void MyTouchArrayGetter(RectTransform Zona, List<Touch> ListName)
    {
        //Método para crear un Array con los toques en la zona. True si está en Start() y false si está en Update().

        if (Input.touchCount != 0)
        {

            ListName.Clear();

            Vector3[] RTpoints = new Vector3[4];
            Zona.GetWorldCorners(RTpoints);                  
            
            

            //Usado en un futuro para evitar que este codigo se ejecute constantemente:
            //Touch[] ToquesEnZonaLastUpdate = new Touch[Input.touches.Length];
            //Array.Copy(Input.touches, ToquesEnZonaLastUpdate, Input.touches.Length);


            foreach (Touch t in Input.touches)
            {  
                if ((t.position.x > RTpoints[0].x) && (t.position.x < RTpoints[2].x) && (t.position.y > RTpoints[0].y) && (t.position.y < RTpoints[2].y))
                {
                    ListName.Add(t);
                }
                                
            }
            //print(ToquesEnZona.Count);
        }
        else
        {
            ListName.Clear();
        }


    }

    Touch MainTouch;
    float Angulo;
    void GestureDefiner(TouchZone tz)
    {
        
            MyTouchArrayGetter(tz.Zona, tz.Lista);
            tz.Gesto.Value = Gesture.None;

            if (tz.Lista.Count != 0)
            {
                //Se define MainTouch, el primer toque del Array que creamos, como el unico toque a tener en cuenta:
                MainTouch = tz.Lista[0];


                //Acciones si el toque ha empezado:
                if (MainTouch.phase == TouchPhase.Began)
                {
                    pos1 = Camera.main.ScreenToWorldPoint(MainTouch.position);
                    tz.Gesto.Value = Gesture.Tap;

                if (CanDoubleTapHappen)
                {
                    tz.Gesto.Value = Gesture.DoubleTap;
                    CanDoubleTapHappen = false;
                }
                else
                {
                    StartCoroutine(DoubleTap(DoubleTapTimer));
                }
                }

                //Acciones si el toque no ha acabado:
                else if (MainTouch.phase == TouchPhase.Moved || MainTouch.phase == TouchPhase.Stationary)
                {
                    pos2 = Camera.main.ScreenToWorldPoint(MainTouch.position);
                    Angulo = Mathf.Atan2(pos2.x - pos1.x, pos2.y - pos1.y) * Mathf.Rad2Deg;
                    mov = pos2 - pos1;
                //print(mov.magnitude);




                        CanDoubleTapHappen = false;

                        if ((45 > Angulo && Angulo > 0) || (0 > Angulo && Angulo > -45))
                        {
                            GestureHandlerRepeatable(tz.Gesto, Gesture.Arriba);
                        }
                        else if (135 > Angulo && Angulo > 45)
                        {
                            GestureHandler(tz.Gesto, Gesture.Derecha);

                        }
                        else if ((180 > Angulo && Angulo > 135) || (-135 > Angulo && Angulo > -180))
                        {
                            GestureHandler(tz.Gesto, Gesture.Abajo);

                        }
                        else if (-45 > Angulo && Angulo > -135)
                        {
                            GestureHandler(tz.Gesto, Gesture.Izquierda);

                        }

                        //SwipeStats[1]++;
                    

                }

                else if (MainTouch.phase == TouchPhase.Ended)
                {
                    tz.Gesto.Value = Gesture.Fin;
                    RepeatableSwipeSent = false;
            }

            }
        

        if (tz.Zona.gameObject!= null)
        {
            //tz.Zona.GetComponent<Image>().enabled = tz.active;
            //print("si");
        }


        if (!tz.active)
        {
            tz.Gesto.Value = Gesture.None;
        }
    }

    void GestureHandler(GestureEnum G, Gesture ThisGesture)
    {
        //SWIPES RUN JUST ONCE:
        if (LastSwipe != ThisGesture)
        {
            if (mov.magnitude > SwipeDistance)
            {
                G.Value = ThisGesture;
                LastSwipe = ThisGesture;
                pos1 = pos2;
            }
        }
        else
        {
            if (mov.magnitude > SwipeDistance/10)
            {
                pos1 = pos2;
            }
        }
    }

    void GestureHandlerRepeatable(GestureEnum G, Gesture ThisGesture)
    {
        //SWIPES RUN WHENEVER THEY WANT:
        if (RepeatableSwipeSent)
        {
            if (mov.magnitude > SwipeDistance / 10)
            {
                pos1 = pos2;
            }
        }

        if (mov.magnitude > SwipeDistance/3)
        {
            G.Value = ThisGesture;
            RepeatableSwipeSent = true;
            pos1 = pos2;
        }



    }

    IEnumerator DoubleTap(float f)
    {
        CanDoubleTapHappen = true;
        yield return new WaitForSecondsRealtime(f);
        CanDoubleTapHappen = false;
    }
    
    
}


