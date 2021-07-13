using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// CLASE PRINCIPAL PARA OTROS OBJETOS Y CLASES DEL JUEGO
public class GameClass : MonoBehaviour
{
    public static PlayerAuto player;
    public static Rigidbody2D player_rb;
    public static Transform player_t;
    public static SpriteRenderer[] player_sr;
    public static TouchBehaviour tb;
    public static EdgeCollider2D colPies;
    public static PolygonCollider2D player_col;
    public enum Gesture { Tap, Arriba, Derecha, Izquierda, Abajo, Fin, None, DoubleTap };
    public static PlatsLook platsLook;
    public static CollisionManager ColManager;
    public enum ColorEnum { Main, Red, Green, Blue, None};
    public enum Direction { Up, Right, Down, Left };
    public static Image rightArrow;
    public static Image upArrow;
    public static Image leftArrow;
    public static GameObject colors_minigui;
    public static Animator player_animator;

    public class BoolRef
    {
        public bool Value { get; set;}       
        public BoolRef(bool value) { Value = value; }
        
    }

    public class FloatRef
    {
        public float Value { get; set; }
        public FloatRef(float value) { Value = value; }

    }

    public class GestureEnum
    {
        public Gesture Value { get; set; }
        public GestureEnum(Gesture value) { Value = value;}
    }

    [System.Serializable]
    public class TouchZone
    {
        public GestureEnum Gesto;
        [SerializeField] public RectTransform Zona;
        public List<Touch> Lista;
        public bool active;

        public TouchZone(GestureEnum Gesto, RectTransform Zona, List<Touch> Lista)
        {
            this.Gesto = Gesto;
            this.Zona = Zona;
            this.Lista = Lista;
        }

        public TouchZone()
        {
            this.Gesto = new GestureEnum(Gesture.None);
            this.Lista = new List<Touch>();
            this.active = true;
        }

        public TouchZone(bool active)
        {
            this.Gesto = new GestureEnum(Gesture.None);
            this.Lista = new List<Touch>();
            this.active = active;
        }

        void Update()
        {
            if (Zona.GetComponentInParent<Image>() != null)
            {
                print("heydddss");
                Zona.GetComponentInParent<Image>().enabled = active;
            }
            if (Zona == null)
            {
                print("heyss");
            }
        }

    }

    public static void BlockPlayer()
    {
        player_rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public static void UnblockPlayer()
    {
        player_rb.constraints = RigidbodyConstraints2D.None;
        player_rb.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        print("player unblocked");
    }


    public void BlockAndRestart()

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

    public void BlockAndRestart(float _time)

    {
        StartCoroutine(BlockTimer(_time));        
    }

    private IEnumerator BlockTimer(float _time)
    {
        player_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //player_sr.sprite = player._playerDeadSprite;
        yield return new WaitForSecondsRealtime(_time);
        SceneManager.UnloadSceneAsync("BASE_SCENE_3");
        StaticProperties.BaseAlreadyExists = false;
        StaticProperties.MusicSceneLoaded = false;
        SceneManager.LoadScene(StaticProperties.CurrentScene);
        StaticProperties.red.Value = false;
        StaticProperties.green.Value = false;
        StaticProperties.blue.Value = false;
        Physics2D.IgnoreLayerCollision(12, 8, true);
        Physics2D.IgnoreLayerCollision(12, 9, true);
        Physics2D.IgnoreLayerCollision(12, 10, true);
    }

    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 0.2f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                init(0.2f);
            }

            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }

    protected void PrefabReplacer(List<GameObject> l, int CurrentObjectIndex)
    {
        Destroy(l[l.Count-1]);               
        l.RemoveAt(l.Count-1);                
       var newobject = Instantiate(l[CurrentObjectIndex].transform,GameObject.Find("GUI_controles").transform);              
       l.Add(newobject.gameObject);        

    }

    protected void PrefabReplacer(List<GameObject> l, GameObject DefaultObject, int CurrentObjectIndex)
    {
        
        var newobject = Instantiate(DefaultObject.transform, GameObject.Find("GUI_controles").transform);
        l.Add(newobject.gameObject);        

        print(newobject + " " + DefaultObject);
    }

    protected void PrefabReplacer(List<GameObject> l, int CurrentObjectIndex, bool IsOnLeft)
    {
        Destroy(l[l.Count - 1]);
        l.RemoveAt(l.Count - 1);
        var newobject = Instantiate(l[CurrentObjectIndex].transform, GameObject.Find("GUI_controles").transform);
        l.Add(newobject.gameObject);
        SetAreaCorrectly(newobject.gameObject, IsOnLeft);

    }

    protected void PrefabReplacer(List<GameObject> l, GameObject DefaultObject, int CurrentObjectIndex, bool IsOnLeft)
    {

        var newobject = Instantiate(DefaultObject.transform, GameObject.Find("GUI_controles").transform);
        l.Add(newobject.gameObject);
        SetAreaCorrectly(newobject.gameObject, IsOnLeft);

        //print(newobject + " " + DefaultObject);
    }

    public void TrueIfExists(string ObjectName, bool boolean)
    {
        if ((GameObject.Find(ObjectName)) != null)
        {
            boolean = true;
        }
        else
        {
            boolean = true;
        }
    }

    void SetAreaCorrectly(GameObject parent, bool IsOnLeft)
    {
        
            RectTransform ParentTransform = parent.GetComponent<RectTransform>();
            ParentTransform.sizeDelta = Vector2.zero;
            ParentTransform.anchorMin = new Vector2(0, 0);
            ParentTransform.anchorMax = new Vector2(1, 1);
            ParentTransform.pivot = new Vector2(0, 1);
            ParentTransform.offsetMin = Vector2.zero;
            ParentTransform.offsetMax = Vector2.zero;
        

        foreach (Transform child in parent.transform)
        {
            if(child.gameObject.name == "JoystickArea")
            {      
                if (IsOnLeft)
                {
                    RectTransform ChildTransform = child.gameObject.GetComponent<RectTransform>();
                    ChildTransform.sizeDelta = Vector2.zero;
                    ChildTransform.anchorMin = new Vector2(0, 0);
                    ChildTransform.anchorMax = new Vector2(0.5f, 1);
                    ChildTransform.pivot = new Vector2(0, 1);
                    ChildTransform.offsetMin = Vector2.zero;
                    ChildTransform.offsetMax = Vector2.zero;
                }
                else
                {
                    RectTransform ChildTransform = child.gameObject.GetComponent<RectTransform>();
                    ChildTransform.sizeDelta = Vector2.zero;
                    ChildTransform.anchorMin = new Vector2(0.55f, 0);
                    ChildTransform.anchorMax = new Vector2(1, 1);
                    ChildTransform.pivot = new Vector2(0, 1);
                    ChildTransform.offsetMin = Vector2.zero;
                    ChildTransform.offsetMax = Vector2.zero;
                }
            }
        }
        

    }

    public void PrintList(List<GameObject> l)
    {
        foreach (GameObject g in l)
            {
            print(g.name);
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
                if (( Input.GetTouch(forcounter).position.x > RTpoints[0].x) && ( Input.GetTouch(forcounter).position.x < RTpoints[2].x) && ( Input.GetTouch(forcounter).position.y > RTpoints[0].y) && ( Input.GetTouch(forcounter).position.y < RTpoints[2].y))
                {
                    ListName.Add( Input.GetTouch(forcounter));
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

public static class hasComponent
{
    public static bool HasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}

public class Metodos
{
    public GameObject[] ChildArray = new GameObject[128];
    string[] ChildNames = new string[26];
    //List<GameObject> ChildList;
    int ChildArrayNumber;

    public void GetChildArray(GameObject a, bool FirstOneIsParent)
    {
        //El primer hueco [0] se reserva al parent. Si no se añade queda null.
        ChildArrayNumber = 1;
        if (FirstOneIsParent)
        {
            ChildArray[0] = a;
            GetChild(a);
        }
        else
        {
            GetChild(a);
        }
    }

    void GetChild(GameObject c)
    {
        foreach (Transform child in c.transform)
        {

            ChildArray[ChildArrayNumber] = child.gameObject;
            //print(ChildArrayNumber + child.gameObject.name);
            ChildArrayNumber++;
            //print(i);
            if (c.transform.childCount > 0)
            {
                GetChild(child.gameObject);
            }
            else
            {                
                return;
            }

        }


    }

    public Color ColorCopy(Color original)
    {
        Color copy = new Color(original.r, original.g, original.b, original.a);
        return copy;
    }

}

public class ActivableObject : GameClass
{

    public bool ButtonActivated
    {
        get
        {
            return _ButtonActivated;
        }

        set
        {
            _ButtonActivated = value;
            if (value == true)
            {
                Activation();
            }            
        }
    }
    public bool _ButtonActivated = true;

    public virtual void Activation()
    {
    }

}

public class ActivationObject : GameClass
{

}
