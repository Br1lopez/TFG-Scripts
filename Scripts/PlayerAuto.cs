using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAuto : OverridableMonoBehaviour
{
    public Movement mov;
    public Controller controls;

    public float _vida;
    public float vida
    {
        get
        {
            return _vida;
        }

        set
        {
            _vida = value;
            if (_vida <= 0)
            {
                if (!StaticProperties.blocked)
                    StaticMethods.BlockAndRestart(0.8f);
            }
        }
    }

    //Control Test
    public bool IsDoubleJumpEnabled = true;
    public bool JumpOnRight = true;
    public bool JoystickEnabled = true;
    public bool IsSwipeInverted = true;


    //Bala
    public GameObject bala_obj;
    public float bala_angle;
    public float shootInterval;
    public bool balaReady = true;
    [SerializeField] float velBala;
    [SerializeField] float alcanceBala;
    public Transform posbala;
    GameClass.BoolRef disparoPressed = new GameClass.BoolRef(false);


    //Melee
    public GameObject melee_obj;
    public bool meleeAftermath = false;


    //Colores:
    public GameClass.BoolRef red = new GameClass.BoolRef(false);
    public GameClass.BoolRef green = new GameClass.BoolRef(false);
    public GameClass.BoolRef blue = new GameClass.BoolRef(false);


    //Otros:
    int num_contactos;
    [SerializeField] static public float tvel = 5f;
    public Sprite _playerDeadSprite;
    int a = 0;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        QualitySettings.antiAliasing = 4;
        QualitySettings.SetQualityLevel(3);
        controls = new Controller();
        StaticProperties.controles = controls;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // Use this for initialization
    void Start()
    {
        if (StaticProperties.initialCodeRan == false)
        {
            Screen.fullScreen = true;
            StaticProperties.initialCodeRan = true;
            Cursor.visible = false;
        }
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

        SetInputEvents();
    }

    void StartVariables()
    {
        Time.timeScale = Time.fixedDeltaTime / 0.02f;
}
  

    // Update is called once per frame
    public override void UpdateMe()
    {
        if (!Application.isMobilePlatform)
        {
            //GetKeyInput();
            Disparar();
        }
        else
        {
            GetMobileInput();
        }
    }

    
    public override void FixedUpdateMe()
    {
        if (Application.isMobilePlatform)
        {
            //GetMobileInput();
        }
        Physics2D.IgnoreLayerCollision(12, 8, !StaticProperties.red.Value);
            Physics2D.IgnoreLayerCollision(12, 9, !StaticProperties.green.Value);
            Physics2D.IgnoreLayerCollision(12, 10, !StaticProperties.blue.Value);

        CheckDaño();
    }


    void SetInputEvents()
    {
        //Movimiento:
        controls.Movement.Left.started += ctx => mov.leftPressed = true;        
        controls.Movement.Right.started += ctx => mov.rightPressed = true;
        controls.Movement.Up.started += ctx => mov.upPressed = true;
        controls.Movement.Down.started += ctx => mov.downPressed = true;
        controls.Movement.LeftPad.started += ctx => mov.leftPressed = true;
        controls.Movement.RightPad.started += ctx => mov.rightPressed = true;
        controls.Movement.UpPad.started += ctx => mov.upPressed = true;
        controls.Movement.DownPad.started += ctx => mov.downPressed = true;


        controls.Movement.Jump.started += ctx => CheckandJump();

        controls.Movement.Left.canceled += ctx => mov.leftPressed = false;
        controls.Movement.Right.canceled += ctx => mov.rightPressed = false;
        controls.Movement.Up.canceled += ctx => mov.upPressed = false;
        controls.Movement.Down.canceled += ctx => mov.downPressed = false;
        controls.Movement.LeftPad.canceled+= ctx => mov.leftPressed = false;
        controls.Movement.RightPad.canceled += ctx => mov.rightPressed = false;
        controls.Movement.UpPad.canceled += ctx => mov.upPressed = false;
        controls.Movement.DownPad.canceled += ctx => mov.downPressed = false;

        //Combate:
        controls.Combat.Main.started += ctx => StaticMethods.SetBool(disparoPressed, true);        
        controls.Combat.Main.canceled += ctx => StaticMethods.SetBool(disparoPressed, false);        
        controls.Combat.Special.started += ctx => Melee();


        //Colores:
        controls.Colors.Red.performed += ctx => StaticMethods.ChangeWorldColor(GameClass.ColorEnum.Red);
        controls.Colors.Green.performed += ctx => StaticMethods.ChangeWorldColor(GameClass.ColorEnum.Green);
        controls.Colors.Blue.performed += ctx => StaticMethods.ChangeWorldColor(GameClass.ColorEnum.Blue);
    }

    void CheckandJump()
    {
        if (!StaticProperties.blocked)
        {
            if ((StaticProperties.dialogueGUI.GUItype == DialogueLine.dialogueType.None || !StaticProperties.dialogueGUI.skippable) && !StaticProperties.freezeControls)
            {
                mov.Saltar();
            }
        }
    }
    
    void CheckAndStartMoving(InputAction i, GameClass.Direction d)
    {        
        //if (i.phase == InputActionPhase.Waiting)
           // mov.IniciarMovimiento(d);
    }

    void CheckAndStopMoving(InputAction i, GameClass.Direction d)
    {
        //print(InputActionPhase.Waiting + " stop");
        //if (i.phase == InputActionPhase.Performed)
        //mov.TerminarMovimiento(d);
    }

    void GetMobileInput()
    {
        //Inputs: (por alguna razón, en teclado solo funcionan en Update())

        if (TouchBehaviour.Jump)
        {
            mov.Saltar();
        }

        /*
        if (tempjump != IsJumpBeingPressed)
        {
            if (IsJumpBeingPressed) {
                //print("salto");
                    }
            tempjump = IsJumpBeingPressed;
        }*/
        

            if (TouchBehaviour.Colour_R)
            {
            StaticProperties.red.Value = true;
            StaticProperties.green.Value = false;
            StaticProperties.blue.Value = false;
            }

            if (TouchBehaviour.Colour_G)
            {
            StaticProperties.red.Value = false;
            StaticProperties.green.Value = true;
            StaticProperties.blue.Value = false;
            }

            if (TouchBehaviour.Colour_B)
            {
            StaticProperties.red.Value = false;
            StaticProperties.green.Value = false;
            StaticProperties.blue.Value = true;
            }


    }

    void GetKeyInput()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            mov.Saltar();
        }

        if (!StaticProperties.BlackAndWhite)
        {
            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Alpha1))
            {
                AudioManager.instance.Play("ColorChange");
                StaticProperties.red.Value = true;
                StaticProperties.green.Value = false;
                StaticProperties.blue.Value = false;
            }

            if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.Alpha2))
            {
                AudioManager.instance.Play("ColorChange");
                StaticProperties.red.Value = false;
                StaticProperties.green.Value = true;
                StaticProperties.blue.Value = false;
            }

            if (Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.Alpha3))
            {
                AudioManager.instance.Play("ColorChange");
                StaticProperties.red.Value = false;
                StaticProperties.green.Value = false;
                StaticProperties.blue.Value = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
        Cinemachine.LensSettings.Default.OrthographicSize -= 1;
        }


        if (Input.GetKey(KeyCode.F))
        {
            if (!StaticProperties.blocked)
                Disparar();
            
        }

        if (Input.GetKey(KeyCode.V))
        {

                Melee();               

        }
    }

    void Disparar()
    {
        if (balaReady&&disparoPressed.Value&&StaticProperties.shooting)
        {
            if(StaticProperties.pauseEnabled)
            AudioManager.instance.Play("Shot");
            var nuevabala_player = Instantiate(bala_obj);
            nuevabala_player.GetComponent<Bala>().angle = bala_angle;
            nuevabala_player.GetComponent<Bala>().rb.position = posbala.position;
            GameClass.player_animator.SetTrigger("Shoot");
            StopCoroutine(descansoBala());
            StartCoroutine(descansoBala());            
        }
    }

    void Melee()
    {
        if (StaticProperties.battery.num == 3&&!StaticProperties.blocked)
        {
            if(StaticProperties.pauseEnabled)
            AudioManager.instance.Play("Melee");
            var newobject2 = Instantiate(melee_obj, GameClass.player_rb.transform);
            newobject2.GetComponent<Melee>().rb.position = mov.rb.position;
            StaticProperties.battery.num = 0;
            StartCoroutine(offsetMelee());
        }
    }

    public IEnumerator descansoBala()
    {
        balaReady = false;
        yield return new WaitForSeconds(shootInterval);
        balaReady = true;
    }

    IEnumerator offsetMelee()
    {
        meleeAftermath = true;
        yield return new WaitForSeconds(0.2f);
        if (StaticProperties.batteryAlwaysFull)
        {
            StaticProperties.battery.num = 3;
        }
        meleeAftermath = false;
    }


    void CheckDaño()
    {
        if (GameClass.ColManager.ObstaculoCollision)
        {
            foreach (Collider2D col in GameClass.ColManager.colPiesArray)
            {
                if (col != null && col.gameObject.HasComponent<Obstaculo>())
                {
                    Obstaculo obs = col.gameObject.GetComponent<Obstaculo>();
                    vida -= obs.daño;
                    if (obs.destroysOnContact)
                    {
                        Destroy(col.gameObject);
                    }
                }
            }
            foreach (Collider2D col in GameClass.ColManager.colPlayerArray)
            {
                if (col != null && col.gameObject.HasComponent<Obstaculo>())
                {
                    Obstaculo obs = col.gameObject.GetComponent<Obstaculo>();
                    vida -= obs.daño;
                    if (obs.destroysOnContact)
                    {
                        Destroy(col.gameObject);
                    }

                }
            }
        }
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

public class TimeOffset
    //Hace que un valor sea true durante cierto numero de runs desde que seejecuta FreshStart().
{
    public bool Value = false;
    int counter;
    int _maxRuns;

    public void FreshStart(int maxRuns)
    {
        //Comienza un conteo después del salto.
        Value = true;
        counter = 0;
        _maxRuns = maxRuns;
    }

    public void UpdateValue()
    {
        if (counter < _maxRuns)
        {
            counter++;
        }
        else
        {
            Value = false;
        }
    }
}