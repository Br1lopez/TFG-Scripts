using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_2018_3_OR_NEWER
using UnityEngine.Rendering;
#else
using UnityEngine.Experimental.Rendering;
#endif
using UnityEngine.Experimental.U2D;
using Unity.Collections;


public class GUIManager : GameClass
{
  

    public List<GameObject> Move = new List<GameObject>();     
    int Move_Index = 0;
    public GameObject Move_Buttons;
    public GameObject Move_Joystick;

    public List<GameObject> Colour = new List<GameObject>();
    int Colour_Index = 0;
    public GameObject Colour_Buttons;
    public GameObject Colour_Joystick;

    public Text SwipeDistanceText;

    public Sprite[] ColourButtonSprites = new Sprite[3];

    bool GUIDebugging = false;
    // Start is called before the first frame update
    void Start()
    {      

        ItemsOnList();
        DefaultOptions();
        StaticProperties.controles.Debug.NextScene.started += ctx => NextScene();
        StaticProperties.controles.Debug.PrevScene.started += ctx => PreviousScene();
        //print(Colour_Joystick.name);

    }

    // Update is called once per frame
    void Update()
    {
        //SwipeDistanceText.text = TouchBehaviour.SwipeDistance.ToString();
        //print("Update: "+nuevo);        
        if (SimpleInput.GetButtonDown("HIDEGUI"))
        {
            HideGUI();
        }
    }

    public void SpriteChanger()
    {
        player.IsSwipeInverted = !player.IsSwipeInverted;

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("In"))
        {
            //g.SetActive(false);
            g.GetComponent<SVGImage>().enabled = player.IsSwipeInverted;
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Out"))
        {
            //g.SetActive(true);
            g.GetComponent<SVGImage>().enabled = !player.IsSwipeInverted;

        }

    }

    public void HideGUI()
    {
        GUIDebugging = !GUIDebugging;


        if (!GUIDebugging)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Debug"))
            {
                g.GetComponent<RectTransform>().Translate(new Vector2(0, 1500));
            }


        }
        else
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Debug"))
            {
                g.GetComponent<RectTransform>().Translate(new Vector2(0, -1500));
            }

        }

       
    }

    public void SwipeDistanceSlider(float f)
    {
        TouchBehaviour.SwipeDistance = f;
    }

    public void DoubleJumpEnabler()
    {
        player.IsDoubleJumpEnabled = !player.IsDoubleJumpEnabled;

        if (!player.IsDoubleJumpEnabled)
        {
            player.mov.MaxJumps = 1;
            //player.salto = 0.35f;
        }
        else if (player.IsDoubleJumpEnabled)
        {
            player.mov.MaxJumps = 2;
            //player.salto = 0.28f;
        }
    }

    public void WhereToJump()
    {
        player.JumpOnRight = !player.JumpOnRight;
                
       //tb.MoveUp.active = (!player.JumpOnRight && !player.JoystickEnabled);
        
        
    }

    public void JoystickEnabler()
    {
        if (Move_Index > Move.Count - 3)
        {
            Move_Index = 0;
        }
        else
        {
            Move_Index++;
        }

        PrefabReplacer(Move, Move_Index, true);        
        //TrueIfExists("Move_Joystick", player.JoystickEnabled);
       
        
    }

    public void ColourControls()
    {
        if (Colour_Index > Colour.Count - 3)
        {
            Colour_Index = 0;
        }
        else
        {
            Colour_Index++;
        }

        PrefabReplacer(Colour, Colour_Index, false);
        //TrueIfExists("Colour_Joystick", player.JoystickEnabled);

        if (Colour_Index == 1)
        {
            tb.Colour.active = false;
        }
        else
        {
            tb.Colour.active = true;
        }

    }

    void DefaultOptions()
    {
        HideGUI();
        if (Application.isMobilePlatform)
        {
            PrefabReplacer(Move, Move_Joystick, Move_Index, true);
            PrefabReplacer(Colour, Colour_Joystick, Colour_Index, false);
            
            SpriteChanger();
            JoystickEnabler();
            DoubleJumpEnabler();
            DoubleJumpEnabler();

            ColourControls();
        }
    }

    void ItemsOnList()
    {
        Move.Add(Move_Joystick);
        Move.Add(Move_Buttons);

        Colour.Add(Colour_Joystick);
        Colour.Add(Colour_Buttons);

    }

    public void NextScene()
    {
        //Este bool se vuelve true para que el jugador reaparezca
        StaticProperties.BaseAlreadyExists = false;

        StaticProperties.MusicSceneLoaded = false;

        //Se define nuevo Spawn para SceneConfig
        Transitioner.NewSpawn = true;

        //Se carga la nueva escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void PreviousScene()
    {
        //Este bool se vuelve true para que el jugador reaparezca
        StaticProperties.BaseAlreadyExists = false;
        StaticProperties.MusicSceneLoaded = false;

        //Se define nuevo Spawn para SceneConfig
        Transitioner.NewSpawn = true;

        //Se carga la nueva escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

