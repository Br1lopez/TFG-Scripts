using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class Movement: MonoBehaviour
{
    public bool isPlayer = false;
    bool groundTrigger = false;
    //Medir altura:    
    public float IntialHeight;
    public float FirstJumpMaxHeight;
    public float SecondJumpMaxHeight;

    //Control Test
    public bool IsDoubleJumpEnabled = true;
    public bool JumpOnRight = true;
    public bool JoystickEnabled = true;
    public bool IsSwipeInverted = true;

    // Parámetros de movimiento:
    [SerializeField] float mov_escala = 0.15f;
    [SerializeField] public float salto = 0.265f;    [SerializeField] float FirstJumpDivider = 8;

    [SerializeField] float DoubleJumpTime = 0.5f;


    // Giro
    public float z_rotation
    {
        get
        {
            switch (StaticProperties.grav_dir)
            {
                case GameClass.Direction.Down:
                    return 0;
                case GameClass.Direction.Right:
                    return 90;
                case GameClass.Direction.Up:
                    return 180;
                case GameClass.Direction.Left:
                    return 270;
            }
                    return 0;
        }
    }
    public Vector2 player_giro = new Vector2();
    bool? _isFacingRight = true;
    public bool? isFacingRight
    {
        get
        {
            return _isFacingRight;
        }

        set
        {
            if (value != null)
            {
                ActualizarGiro();
                _isFacingRight = value;
            }
        }
    }


    public void ActualizarGiro()
    {
            if (StaticMethods.IsVectorVertical(StaticProperties.grav_dir_vector))
            {
                if (isFacingRight == true)
                {
                    player_giro = new Vector2(0, 0);
                }
                else if (isFacingRight == false)
                {
                    player_giro = new Vector2(0, 180);
                }
            }
            else
            {
                if (isFacingRight == true)
                {
                    player_giro = new Vector2(0, 0);
                }
                else if (isFacingRight == false)
                {
                    player_giro = new Vector2(180, 0);
                }
            }        
        transform.rotation = Quaternion.Euler(player_giro.x, player_giro.y, z_rotation);
    }

    // Parámetros de gravedad:
    [SerializeField] public float grav_max = 0.45f;
    [SerializeField] public float grav_escala = 0.015f;
    public float grav_fuerza = 0f;
    Vector2 grav_vector = new Vector2(0, 0);

    //Inercia (campo gravitacional):
    public Vector2 InerciaGrav_dir = new Vector2(0, 0);
    public float InerciaGrav_fuerza = 0;
    Vector2 InerciaGrav_vector = new Vector2(0, 0);

    //Inercia (plataformas móviles):
    public Vector2 MovingPlatsInfluence_vector
    {
        get
        {
            if (ColManager.movingPlatCollision)
            {
                return ColManager.collidingMovPlat.movPlatVel;
            }
            else
            {
                return new Vector2(0, 0);
            }
        }
    }

    //Cancelacion del salto(tb):
    public GameClass.BoolRef IsJumpCancelled = new GameClass.BoolRef(false);
    bool tempjump;

    //Booleanos (salto):
    bool IsJumpBeingPressed;



    public bool OnAir
    {
        get
        {
            if (IsJumpBeingPressed || AfterJump.Value == true)
            {
                return true;
            }
            else
            {
                return !(ColManager.groundCollision);
            }
        }
    }

    bool offsetOnAirBool;

    public int JumpsPerformed;
    public int MaxJumps = 2;
    bool JoystickRestart = true;
    bool ExtraTimeForDoubleJump;
    int JumpCoroutinesRunning = 0;
    TimeOffset AfterJump = new TimeOffset();

    //Booleanos (colisiones):
    private GameClass.BoolRef IsInfOffCol = new GameClass.BoolRef(false);
    private GameClass.BoolRef IsSupOffCol = new GameClass.BoolRef(false);
    public bool colision_cabeza = false;
    public bool colision_player = false;

    //Booleanos:
    public bool IsPlayerMoving;


    //Input:
    public float input_horizontal
    {
        get
        {
            if (leftPressed == rightPressed)
            {
                if (GameClass.player_animator != null)
                    GameClass.player_animator.SetBool("Running", false);
                return 0;
            }else if (leftPressed)
            {
                if (GameClass.player_animator != null)
                    GameClass.player_animator.SetBool("Running", true);
                return -1;
            }
            else if (rightPressed)
            {
                if (GameClass.player_animator != null)
                    GameClass.player_animator.SetBool("Running", true);
                return 1;
            }
            else
            {
                if (GameClass.player_animator != null)
                    GameClass.player_animator.SetBool("Running", false);
                return 0;
            }
        }
    }
    public float input_vertical
    {
        get
        {
            if (downPressed == upPressed)
            {
                return 0;
            }
            else if (downPressed)
            {
                return -1;
            }
            else if (upPressed)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    public bool leftPressed = false;
    public bool rightPressed = false;
    public bool downPressed = false;
    public bool upPressed = false;


    //Velocidad que lleva:
    float hsp;
    float vsp;

    //Sitio donde se dibuja:
    public Vector2 vel;

    //Componentes:
    public Rigidbody2D rb;
    private PolygonCollider2D ofcol;
    public CollisionManager ColManager;

    //Otros:
    int num_contactos;
    float posy;
    float posx;
    [SerializeField] static public float tvel = 5f;
    int a = 0;


    // Use this for initialization
    void Start()
    {
        StaticMethods.SetDefaultInterpolation(rb.gameObject);        
        StartVariables();
        //print("hey");

        /*
        grav_escala = grav_escala / 60;
        grav_max = grav_max / 60;
        mov_escala = mov_escala / 60;
        salto = salto / 60;
        */

        //ofcol = GameObject.Find("Col_pies").GetComponent<PolygonCollider2D>();
        //IsInfOffCol = GameObject.Find("Col_OffsetInferior").GetComponent<Col_OffsetInferior>().OffsetInferiorCol;
        // IsSupOffCol = GameObject.Find("col_OffsetSuperior").GetComponent<Col_OffsetSuperior>().OffsetSuperiorCol;
        //print(GameObject.Find("col_OffsetSuperior").GetComponent<Col_OffsetSuperior>());
        //Application.targetFrameRate = 50;
        //FullScreenMode.ExclusiveFullScreen = 0;     
        //print(AnimatedTile.checkeonombre[0].name);

    }

    void StartVariables()
    {
        IsDoubleJumpEnabled = true;
        MaxJumps = 2;
        Time.timeScale = Time.fixedDeltaTime / 0.02f;
    }



    public void FixedUpdate()
    {
        if (Application.isMobilePlatform)
        {
            //GetMobileInput();
        }

        MovimientoGravedad();
        InerciaMovimiento();


        if (input_horizontal > -0.1 && input_horizontal < 0.1 && input_vertical > -0.1 && input_vertical < 0.1)
        {
            IsPlayerMoving = false;
        }
        else
        {
            IsPlayerMoving = true;
        }

        //Vectores
        vel.x = (mov_escala * input_horizontal * Mathf.Abs(StaticProperties.grav_dir_vector.y)) + grav_vector.x + MovingPlatsInfluence_vector.x;
        vel.y = (mov_escala * input_vertical * Mathf.Abs(StaticProperties.grav_dir_vector.x)) + grav_vector.y + MovingPlatsInfluence_vector.y;

        if (vel.y < (-0.1))
        {
            if (GameClass.player_animator != null)
                GameClass.player_animator.SetBool("Diving",true);
        }
        else
        {
            if (GameClass.player_animator != null)
                GameClass.player_animator.SetBool("Diving", false);
        }
        //print(GameClass.player_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        setFacingDirection();

        //Dibujado final
        //rb.MovePosition(rb.position + vel);
        //rb.AddForce(vel, ForceMode2D.Force);
        //rb.velocity = xy;
        rb.MovePosition(rb.position + vel);

    }

    void setFacingDirection()
    {
        if (StaticMethods.IsVectorVertical(StaticProperties.grav_dir_vector)&&!StaticProperties.blocked)
        {
            isFacingRight = !StaticMethods.NumberToBool((StaticProperties.grav_dir_vector.y) * input_horizontal);
        }
        else
        {
            isFacingRight = StaticMethods.NumberToBool((StaticProperties.grav_dir_vector.x) * input_vertical);
        }

        switch (StaticProperties.grav_dir)
        {
            case GameClass.Direction.Down:
                GameClass.player.bala_angle = isFacingRight == false ? 270 : 90;
                break;
            case GameClass.Direction.Right:
                GameClass.player.bala_angle = isFacingRight == false ? 0 : 180;
                break;
            case GameClass.Direction.Up:
                GameClass.player.bala_angle = isFacingRight == false ? 90 : 270;
                break;
            case GameClass.Direction.Left:
                GameClass.player.bala_angle = isFacingRight == false ? 180 : 0;
                break;
        }
    }

    public void Saltar()
    {
        IsJumpBeingPressed = true;
    }
    /*
    public void Mover(GameClass.Direction d)
    {
        print("ey");
        switch (d)
        {
            case GameClass.Direction.Left:
                input_horizontal=-1;
                break;
            case GameClass.Direction.Right:
                input_horizontal=1;
                break;
            case GameClass.Direction.Up:
                input_vertical=1;
                break;
            case GameClass.Direction.Down:
                input_vertical=-1;
                break;
        }
    }
    public void Parar()
    {
        input_horizontal = 0;
        input_vertical = 0;
    }

    public void IniciarMovimiento(GameClass.Direction d)
    {

        switch (d)
        {
            case GameClass.Direction.Left:
                input_horizontal = input_horizontal == 0? -1: 0;
                break;
            case GameClass.Direction.Right:
                input_horizontal = input_horizontal == 0 ? 1 : 0;
                break;
            case GameClass.Direction.Up:
                input_vertical = input_vertical == 0 ? 1 : 0;
                break;
            case GameClass.Direction.Down:
                input_vertical = input_vertical == 0 ? -1 : 0;
                break;
        }
    }

    public void TerminarMovimiento(GameClass.Direction d)
    {

        switch (d)
        {
            case GameClass.Direction.Left:
                input_horizontal= 0;
                break;
            case GameClass.Direction.Right:
                input_horizontal= 0;
                break;
            case GameClass.Direction.Up:
                input_vertical= 0;
                break;
            case GameClass.Direction.Down:
                input_vertical= 0;
                break;
        }
    }
    */
    void MovimientoGravedad()
    {
        //Checkeo de si está en el suelo
        if (!OnAir)
        {
            if (groundTrigger)
            {
                AudioManager.instance.Play("Ground");
                groundTrigger = false;
            }
            if (GameClass.player_animator != null)
            {
                GameClass.player_animator.SetBool("Jumping", false);
            }
            JumpsPerformed = 0;
            grav_fuerza = 0;
            IntialHeight = rb.position.y;
            FirstJumpMaxHeight = 0;
            SecondJumpMaxHeight = 0;
            StopCoroutine(offsetOnAir());
            StartCoroutine(offsetOnAir());
        }
        else
        {
            groundTrigger = true;
            if (JumpsPerformed == 1)
            {
                if ((rb.position.y - IntialHeight) > FirstJumpMaxHeight)
                {
                    FirstJumpMaxHeight = rb.position.y - IntialHeight;
                }
            }
            else if (JumpsPerformed == 2)
            {
                if ((rb.position.y - IntialHeight) > SecondJumpMaxHeight)
                {
                    SecondJumpMaxHeight = rb.position.y - IntialHeight;
                }
            }
            //print(FirstJumpMaxHeight + " " + SecondJumpMaxHeight);


        }

        //En qué momentos se acepta el salto y se añade una fuerza:
        if (IsJumpBeingPressed)
        {
            if (JumpsPerformed < MaxJumps)
            {
                if (JumpsPerformed == 0)
                {
                    if (isPlayer&&GameClass.player_animator != null)
                    {
                        if(StaticProperties.pauseEnabled)
                        AudioManager.instance.Play("Jump");
                        GameClass.player_animator.SetBool("Jumping", true);
                        GameClass.player_animator.SetTrigger("Jump");
                    }

                    grav_fuerza = -salto;
                    JumpsPerformed++;
                }
                else if (JumpsPerformed == 1)
                {
                    if (isPlayer)
                    {
                        if (StaticProperties.pauseEnabled)
                            AudioManager.instance.Play("Jump");
                        if (GameClass.player_animator != null)
                            GameClass.player_animator.SetTrigger("Jump");
                    }

                    grav_fuerza = -salto / FirstJumpDivider;
                    JumpsPerformed++;
                }
            }
            IsJumpBeingPressed = false;
            AfterJump.FreshStart(3);
            //print("salto ");
        }
        AfterJump.UpdateValue();

        /*
        if (OnAir&&!AfterJump.Value && JumpsPerformed == 0&&offsetOnAirBool)
        {
            JumpsPerformed = 1;
        }*/

        //Excepciones:
        if (colision_cabeza || IsJumpCancelled.Value)
        {
            grav_fuerza += grav_max / 5;
            //print("ey");
        }

        if (IsJumpCancelled.Value)
        {
            grav_fuerza = 0;
        }


        //DEFINIMOS LAS FUERZAS:

        //Inercia:
        if (InerciaGrav_fuerza > 0)
        {
            InerciaGrav_fuerza -= grav_escala * 6;
        }
        else if (InerciaGrav_fuerza < 0)
        {
            InerciaGrav_fuerza = 0;
        }

        //Gravedad:
        if (OnAir)
        {
            if (grav_fuerza < grav_max)
            {
                grav_fuerza += grav_escala;
            }
            else
            {
                grav_fuerza = grav_max;
            }
        }

        //Vectores finales:   
        InerciaGrav_vector = InerciaGrav_dir * InerciaGrav_fuerza;
        grav_vector = StaticProperties.grav_dir_vector * grav_fuerza + InerciaGrav_vector;
    }

    void InerciaMovimiento()
    {

        //if (!IsPlayerColliding.Value)
        {
            /* if (colision_pie)
             {
                 InerciaMovimiento_fuerza = 0;
             }else if (InerciaMovimiento_fuerza > 0)
             {
                 InerciaMovimiento_fuerza -= 0.001f;
             }
             else if (InerciaMovimiento_fuerza < 0)
             {
                 InerciaMovimiento_fuerza = 0;
             }*/

        }
        //Vector
    }



    IEnumerator offsetOnAir()
    {
        offsetOnAirBool = false;
        yield return new WaitForSeconds(0.55f);
        offsetOnAirBool = true;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision_player = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colision_player = false;

    }



    //COLISION CON CAMPO DE GRAVEDAD

    /*

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("colision");

        if ((collision.gameObject.tag == "GravField") && (green.Value))
        {
            grav_dir = collision.gameObject.GetComponentInParent<GravityFied>().GravFieldValue;
            print("colision");
        }

    }

   private void OnTriggerStay2D(Collider2D collision)
    {
        print("efectivamente");
        if ((collision.gameObject.tag == "GravField") && (green.Value))
        {
            grav_dir = collision.gameObject.GetComponentInParent<GravityFied>().GravFieldValue;
            print("colisionstay");
        }else if ((collision.gameObject.tag == "GravField") && (!green.Value))
            {
            print("colisionstayout");
            grav_dir = new Vector2(0, -1);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        print("colisionexit");
        if (collision.gameObject.tag == "GravField")
        {
            grav_dir = new Vector2(0,-1);
            print("colisionexit");
        }
    }
    */
}
