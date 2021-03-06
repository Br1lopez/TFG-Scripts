using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Desde este script se controla el valor de la inercia. 
//La suma de la inercia a la velocidad del jugador se realiza en PlayerAuto.

public class MovingPlatform : ActivableObject
{
    public Transform[] PosArray = new Transform[2];
    private Transform[] _tempPosArray;

    public bool AjustarSegunAnchura = true;

    public Transform mySprite;
    public bool voltear;

    private Rigidbody2D rb;
    public Vector2 PrevPos = new Vector2(0,0);
    public Vector2 CurrentPos = new Vector2();
    Vector3 prevMovPlatpos = new Vector3();
    public float vel;
    float _vel
    {
        get
        {
            if (velOverride.Length<(CurrentTargetIndex+1))
            {
                return vel/60;
            }
            else
            {
                if (CurrentTargetIndex == 0)
                {
                    return velOverride[velOverride.Length - 1] / 60;
                }
                else
                {
                    return velOverride[CurrentTargetIndex - 1]/60;
                }
            }
        }
    }
    float f = 0;
    int CurrentTargetIndex;
    public float[] velOverride=new float[0];

    public Animator anim;

    //Puntos de origen y destino, definidos según CurrentIndex
    Transform prevPoint
    {
        get
        {
            if (CurrentTargetIndex != 0)
            {
                return PosArray[CurrentTargetIndex - 1];
            }
            else
            {
                return PosArray[PosArray.Length - 1];
            }
        }

        set
        {
            prevPoint = value; 
        }
    }
    Transform nextPoint
    {
        get
        {
            return PosArray[CurrentTargetIndex];
        }
        set
        {
            nextPoint = value;
        }
    }

    public bool PlayerColliding = false;

    public Vector2 movPlatVel
    {
        get
        {
            if (ButtonActivated)
            {

                return (CurrentPos - PrevPos);
            }
            else
            {
                return new Vector2(0, 0);
            }
        }
    }

    public bool StopWhenFinish = false;

    private void Awake()
    {
        StaticMethods.SetDefaultInterpolation(gameObject);
    }

    //private Vector2 vel = new Vector2(0, 2);
    // Start is called before the first frame update
    void Start()
    {
        if((prevPoint.position.x - nextPoint.position.x) > 0)
        {
            if(voltear)
            mySprite.Rotate(0, 180, 0);
        }
        if (AjustarSegunAnchura)
        {
            RecalcularPuntos();
        }
        rb = gameObject.GetComponent<Rigidbody2D>();

        if (PosArray.Length > 0)
        {
            rb.position = PosArray[0].position;

        }

        if (PosArray.Length > 1)
        {
            CurrentTargetIndex = 1;
        }

        //Se calcula todo una vez para evitar bugs al activarla
        PlatformPositionScript();
        PlatformPositionScript();
        PlatformPositionScript();        
        f = 0;
        rb.MovePosition(prevPoint.position);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ButtonActivated)
        {
            PlatformPositionScript();
        }
    }

    void PlatformPositionScript()
    {
        PrevPos = CurrentPos;
        if (PosArray.Length > 1)
        {        

            //Definimos la posicion y nos desplazamos a ella
            CurrentPos = Vector2.Lerp(prevPoint.position, nextPoint.position, f);
            rb.MovePosition(CurrentPos);

            //Si aun no llegamos a la posición, avanzar. Si se ha llegado, sustituir la posición por una nueva.
            if (f < 1)
            {
                f += _vel/Vector2.Distance(prevPoint.position, nextPoint.position);
            }

            else
            {
                f = 0;

                if (CurrentTargetIndex == (PosArray.Length - 1))
                {
                    if (StopWhenFinish)
                    {
                        ButtonActivated = false;
                    }
                    else
                    {
                        CurrentTargetIndex = 0;
                        if (voltear)
                        {  
                        mySprite.Rotate(0, 180, 0); 
                        }
                        if (anim != null)
                        {
                            anim.SetBool("Stop", false);
                            anim.SetBool("Right", false);
                        }
                    }

                }
                else
                {
                    CurrentTargetIndex++;
                    if (voltear)
                    {
                        mySprite.Rotate(0, -180, 0);
                    }
                    if (anim != null)
                    {
                        anim.SetBool("Stop", false);
                        anim.SetBool("Right", true);
                    }
                }

            }

        }
    }

    void RecalcularPuntos()
    {
        //Array temporal para apuntar posicion inicial:
        _tempPosArray = PosArray;

        float SpriteWidth = 0;

        //De momento solo es válido para dos puntos.        
        if (PosArray.Length == 2)
        {
            
            float PathDistance_h = Mathf.Abs(PosArray[0].position.x - PosArray[1].position.x);

                if (gameObject.GetComponent<Collider2D>() != null)
                {
                    SpriteWidth = gameObject.GetComponent<Collider2D>().bounds.size.x*gameObject.transform.localScale.x;
                }
            
            
            //Si el primer punto está a la izquierda:
            if (PosArray[0].position.x< PosArray[1].position.x)
            {
                PosArray[0].position += (new Vector3((SpriteWidth/2),0,0));
                PosArray[1].position -= (new Vector3((SpriteWidth/2), 0, 0));
            }
            else
            {
                PosArray[0].position -= (new Vector3((SpriteWidth / 2), 0, 0));
                PosArray[1].position += (new Vector3((SpriteWidth / 2), 0, 0));
            }
            
        }

    }



    void OnApplicationQuit()
    {
        if (AjustarSegunAnchura)
        {
            PosArray = _tempPosArray;
        }
    }




}

