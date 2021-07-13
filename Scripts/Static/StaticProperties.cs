using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticProperties
{
    static public bool pauseEnabled = true;
    static public bool initialCodeRan = false;

    static public GameClass.BoolRef red = new GameClass.BoolRef(false);
    static public GameClass.BoolRef green = new GameClass.BoolRef(false);
    static public GameClass.BoolRef blue = new GameClass.BoolRef(false);

    static public bool MusicSceneLoaded = false;

    static public bool BaseAlreadyExists = false;
    static public Vector3 SpawnPos;

    static public float CameraDamping = 4;
    static public SceneReference CurrentScene;
    static public AudioSource[] CurrentMusic;

    static public float transitionTime = 0.15f;

    static public bool DepthPreview = true;

    static public Controller controles;

    static public string CurrentLayerName
    {
        get
        {
            if (red.Value)
            {
                return "RED_ground";
            } else if (green.Value)
            {
                return "GREEN_ground";

            } else if (blue.Value)
            {
                return "BLUE_ground";
            }
            else
            {
                return null;
            }
        }
    }
    static public int CurrentLayerIndex
    {
        get
        {
            if (red.Value)
            {
                return 8;
            }
            else if (green.Value)
            {
                return 9;

            }
            else if (blue.Value)
            {
                return 10;
            }
            else
            {
                return 11;
            }
        }
    }

    static public RigidbodyInterpolation DefaultInterpolation = RigidbodyInterpolation.Interpolate;
    static public RigidbodyInterpolation2D DefaultInterpolation2D = RigidbodyInterpolation2D.Interpolate;

    public static bool AreControlsHorizontal = true;

    public static bool isCurrentSceneUnloading = false;
    public static bool isBaseSceneUnloading = false;

    public static int GravityFieldsWorking = 0;
    public static GravityFied activeGravField = new GravityFied();

    public static int MusicStampTime;

    public static bool blocked = false;

    public static GameClass.ColorEnum currentColor
    {
        get
        {
            if (red.Value)
            {
                return GameClass.ColorEnum.Red;
            }else if (green.Value)
            {
                return GameClass.ColorEnum.Green;
            }
            else if (blue.Value)
            {
                return GameClass.ColorEnum.Blue;
            }
            else
            {
                return GameClass.ColorEnum.Main;
            }
        }
    }

    public static Bateria battery;

    public static DevEntry[] devEntries = {
            new DevEntry(false, "Diario del desarrollador – 08/11/99\n\nYa he terminado de crear el primer nivel. Llevo 48 horas sin dormir, pero está mereciendo la pena.\n\nPor fin puedo hacer algo personal, cuando trabajaba en la desarrolladora no tenía ninguna libertad creativa.\n\nAdemás, este proyecto me ayudará a mantener la mente ocupada y no darle vueltas a lo de mi despido.\n\nAunque yo no lo llamaría despido. Lo que me hicieron fue una traición.\n\n- David"),
            new DevEntry(false, "Diario del desarrollador – 28/11/99\n\nEstoy trabajando en muchas características nuevas. Se que llevo tiempo sin salir de casa, pero el resultado valdrá la pena.\n\nVoy a dedicar todo mi tiempo al juego. Mi novia nunca lo entendió: decía que me estaba obsesionando, que no era yo mismo.\n\nAhora que me ha dejado podré dedicarle a esto el 100% de mi atención.\n\n- David"),
            new DevEntry(false, "Diario del desarrollador – 27/12/99\n\nYa llevo 8 días sin trabajar en este proyecto. Me detuve en Navidad, después ocurrió lo de mi madre, y su funeral…\n\nYa es oficial: este juego es lo único que me queda. Será mejor que me centre en ello.\n\n- David"),
    };

    public static DevEntriesManager DevEntryManager;

    public static Vector2 grav_dir_vector
    {
        get
        {
            return (StaticMethods.Dir2Vec(grav_dir));
        }


    }
    public static GameClass.Direction grav_dir = GameClass.Direction.Down;

    public static bool batteryAlwaysFull = false;

    public static bool BlackAndWhite = false;
    public static DialogueGUI dialogueGUI;
    public static DialogueLine nullDialogue = new DialogueLine();

    public static bool freezeControls = false;

    public static GameObject PauseMenu;
    public static bool PausedByMenu = false;
    public static bool PausedByDialogue = false;

    public static Cortinilla cortinilla;
    public static bool shooting = true;

}

public class StaticCoroutine : MonoBehaviour
{

    private static StaticCoroutine mInstance = null;

    private static StaticCoroutine instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(StaticCoroutine)) as StaticCoroutine;

                if (mInstance == null)
                {
                    mInstance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
                }
            }
            return mInstance;
        }
    }

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as StaticCoroutine;
        }
    }

    IEnumerator Perform(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
        Die();
    }

    /// <summary>
    /// Place your lovely static IEnumerator in here and witness magic!
    /// </summary>
    /// <param name="coroutine">Static IEnumerator</param>
    public static void DoCoroutine(IEnumerator coroutine)
    {
        instance.StartCoroutine(instance.Perform(coroutine)); //this will launch the coroutine on our instance
    }

    void Die()
    {
        mInstance = null;
        Destroy(gameObject);
    }

    void OnApplicationQuit()
    {
        mInstance = null;
    }
}


public class StaticMethods
{
        static public Vector2 Dir2Vec(GameClass.Direction dir)
    {
        switch (dir)
        {
            case GameClass.Direction.Down:
                return new Vector2(0, -1);
            case GameClass.Direction.Up:
                return new Vector2(0, 1);
            case GameClass.Direction.Right:
                return new Vector2(1, 0);
            case GameClass.Direction.Left:
                return new Vector2(-1, 0);
        }
        return new Vector2(0, -1);
    }

    public static void ChangeWorldColor(GameClass.ColorEnum c)
    {
        if (!StaticProperties.BlackAndWhite)
        {
            AudioManager.instance.Play("ColorChange");
            switch (c)
            {
                case GameClass.ColorEnum.Red:
                    StaticProperties.red.Value = true;
                    StaticProperties.green.Value = false;
                    StaticProperties.blue.Value = false;
                    break;
                case GameClass.ColorEnum.Green:
                    StaticProperties.red.Value = false;
                    StaticProperties.green.Value = true;
                    StaticProperties.blue.Value = false;
                    break;
                case GameClass.ColorEnum.Blue:
                    StaticProperties.red.Value = false;
                    StaticProperties.green.Value = false;
                    StaticProperties.blue.Value = true;
                    break;
            }
        }
}

    public static void FreezeGame()
    {
        StaticProperties.freezeControls = true;
        Time.timeScale = 0;        
    }

    public static void WaitAndUnfreezeGame(float time)
    {
        StaticCoroutine.DoCoroutine(FreezeTimer(time));
    }
    private static IEnumerator FreezeTimer(float _time)
    {
        Time.timeScale = Time.fixedDeltaTime / 0.02f;
        yield return new WaitForSecondsRealtime(_time);
        StaticProperties.freezeControls = false;
    }

    public static void UnfreezeGame()
    {
        Time.timeScale = Time.fixedDeltaTime / 0.02f;
        StaticProperties.freezeControls = false;
    }

    static void BlockAndRestart()

    {
        GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        SceneManager.LoadScene(StaticProperties.CurrentScene);
        StaticProperties.red.Value = false;
        StaticProperties.green.Value = false;
        StaticProperties.blue.Value = false;
        Physics2D.IgnoreLayerCollision(12, 8, true);
        Physics2D.IgnoreLayerCollision(12, 9, true);
        Physics2D.IgnoreLayerCollision(12, 10, true);
    }

    public static void BlockAndRestart(float _time)

    {
        StaticCoroutine.DoCoroutine(BlockTimer(_time));
    }

    private static IEnumerator BlockTimer(float _time)
    {
        AudioManager.instance.Play("Death");
        StaticProperties.blocked = true;
        GameClass.player_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        /*GameClass.player_rb.bodyType = RigidbodyType2D.Static;*/
        GameClass.player_sr[0].GetComponentInParent<Animator>().speed = 0;
        foreach (SpriteRenderer sr in GameClass.player_sr)
        {
            sr.color = Color.red;
        }
        if (GameClass.player_sr[0].sprite != null && GameClass.player_sr[0].sprite != null)
        {
            GameClass.player_sr[0].sprite = null;
        }
        yield return new WaitForSecondsRealtime(_time * 0.33f);
        StaticProperties.cortinilla.On();
        yield return new WaitForSecondsRealtime(_time * 0.66f);
        GameClass.player_sr[0].GetComponentInParent<Animator>().speed = 1;
        StaticProperties.BaseAlreadyExists = false;
        StaticProperties.MusicSceneLoaded = false;
        //StaticProperties.MusicStampTime = StaticProperties.CurrentMusic[0].timeSamples;
        SceneManager.LoadScene(StaticProperties.CurrentScene);
        StaticProperties.isCurrentSceneUnloading = true;
        StaticProperties.red.Value = false;
        StaticProperties.green.Value = false;
        StaticProperties.blue.Value = false;
        Physics2D.IgnoreLayerCollision(12, 8, true);
        Physics2D.IgnoreLayerCollision(12, 9, true);
        Physics2D.IgnoreLayerCollision(12, 10, true);        
    }
   /* public static void WaitForSceneToUnload()
    {
        if (StaticProperties.isCurrentSceneUnloading)
        {
            SceneManager.LoadScene(StaticProperties.CurrentScene, LoadSceneMode.Additive);
            StaticProperties.isCurrentSceneUnloading = false;
            StaticProperties.isBaseSceneUnloading = true;
            SceneManager.UnloadSceneAsync("BASE_SCENE_3");
        }

        if (StaticProperties.isBaseSceneUnloading)
        {
            SceneManager.LoadScene("BASE_SCENE_3", LoadSceneMode.Additive);
            StaticProperties.isBaseSceneUnloading = false;
        }
    }*/

    public static void ChangeColor(GameObject a, Color color)
    {
        if (a != null && a.GetComponent<SpriteRenderer>() != null && a.GetComponent<SpriteRenderer>().sharedMaterial != null)
        {
            var tempMaterial = new Material(a.GetComponent<SpriteRenderer>().sharedMaterial);
            tempMaterial.color = color;
            a.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;
        }
        else if (a != null && a.GetComponent<UnityEngine.UI.Image>() != null && a.GetComponent<UnityEngine.UI.Image>().material != null)
        {
            var tempMaterial = new Material(a.GetComponent<UnityEngine.UI.Image>().material);
            tempMaterial.color = color;
            a.GetComponent<UnityEngine.UI.Image>().material = tempMaterial;
        }
    }

    public Color ColorCopy(Color original)
    {
        Color copy = new Color(original.r, original.g, original.b, original.a);
        return copy;
    }

    public static void ChangeFrameRate()
    {
        if (Application.targetFrameRate == 30)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }
    }

    public static void ChangeVSync()
    {
        if (QualitySettings.vSyncCount == 0| QualitySettings.vSyncCount == 1)
        {
            QualitySettings.vSyncCount++;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public static void SetDefaultInterpolation(GameObject g)
    {
        if (g.GetComponent<Rigidbody>() != null)
        {
            g.GetComponent<Rigidbody>().interpolation = StaticProperties.DefaultInterpolation;
        }
        else if (g.GetComponent<Rigidbody2D>() != null)
        {
            g.GetComponent<Rigidbody2D>().interpolation = StaticProperties.DefaultInterpolation2D;
        }
    }

    public static bool IsVectorVertical(Vector2 v)
    {
        if (Mathf.Abs(v.x)>Mathf.Abs(v.y))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public GameClass gc;

    public static bool? NumberToBool(float f)
    {
        if (f > 0)
        {
            return true;
        }else if (f < 0)
        {
            return false;
        }
        else
        {
            return null;
        }
    }

    public static float BoolToNumber(bool? b)
    {
        if (b==true)
        {
            return 1;
        }
        else if (b==false)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }


    public static void EnableNoShootingZone()
    {
        StaticProperties.shooting = false;
    }

    public static void DisableNoShootingZone()
    {
        StaticProperties.shooting = true;
    }

    public static IEnumerator WaitAndDisableZone(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        DisableNoShootingZone();
    }

    public static void SetBool(GameClass.BoolRef br, bool value)
    {
        br.Value = value;
    }

    public static void UnlockDevEntry(DevEntry de)
    {
        de.unlocked = true;
        StaticProperties.DevEntryManager.ChangeText(de.text);
        StaticProperties.DevEntryManager.StartBanner();
    }

    public static void Pause(bool isPaused)
    {
        StaticProperties.PauseMenu.SetActive(isPaused);
        if (isPaused == true)
        {
            StaticProperties.PausedByMenu = true;
            FreezeGame();
        }
        else
        {
            StaticProperties.PausedByMenu = false;

            if(!StaticProperties.PausedByDialogue)
            UnfreezeGame();
        }
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}


