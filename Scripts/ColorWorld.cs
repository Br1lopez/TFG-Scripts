using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ColorWorld : GameClass
{
    // private Tilemap tm;
    protected Color ShaderColor;
    private float alpha = 1f;
    private bool trans_end = true;
    private float trans_vel;    
    private Color trans_color;
    private SpriteRenderer otros;    
    string[] ChildNames = new string[26];
    //List<GameObject> ChildList;
    int ChildArrayNumber;
    GameObject fondo_spr;
    protected Color ColorFondo;
    [SerializeField] GameObject EmptyObject;

    //Color utilizado para relleno y borde de plataformas:
    protected Color ColorBase;

    protected BoolRef ColorActual = new BoolRef(false);
   

    protected string NombreFondo;
    protected string NombreBoton;


    [SerializeField] Sprite ButtonOn;
    [SerializeField] Sprite ButtonOff;

    GameObject[] ChildArray;


    void ChildUpdate(GameObject a)
    {
        
        if (a != null && a.GetComponent<SpriteRenderer>()!=null && a.GetComponent<SpriteRenderer>().sharedMaterial !=null)
        {

            var tempMaterial = new Material(a.GetComponent<SpriteRenderer>().sharedMaterial);           
            
            Color c = a.GetComponent<SpriteRenderer>().sharedMaterial.color;

            if (a.name != "Blurred Clone")
            {
                tempMaterial.color = new Color(c.r, c.g, c.b, (alpha));
            }
            else
            {
                tempMaterial.color = new Color(c.r,c.g,c.b,(1-alpha));
                
            }

            a.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;
        }
    }

    void GetChildArray(GameObject a, bool b)
    {
        /*ChildArrayNumber = 1;
        if (b)
        {
            ChildArray[0] = a;
            GetChild(a);
        }
        else
        {
            GetChild(a);
        }*/

        ChildArray = FindGameObjectsWithLayer(gameObject.layer);
    }

    void GetChild(GameObject c)
    {
        /*foreach (Transform child in c.transform)
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
                print("fin");
                return;
            }

        }  */      
    }

    GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            { goList.Add(goArray[i]);
            }
        }

        if (goList.Count == 0)
        {
            return null;
        }

        return goList.ToArray();
    }

    void OriginalPlats (GameObject original)
    {
        if (original != null && original.name !="Blurred Clone")
        {
            //Medidas del Outline (color):
            SetShaderFloat(original, 1 - 0.15f / original.transform.localScale.x, "_scaleX");
            SetShaderFloat(original, 1 - 0.15f / original.transform.localScale.y, "_scaleY");

            //Medidas del Outline (Alpha):
            SetShaderFloat(original, 1 - 0.15f / original.transform.localScale.x, "_scaleX_2");
            SetShaderFloat(original, 1 - 0.15f / original.transform.localScale.y, "_scaleY_2");
        }
    }

   void BlurredClone(GameObject original, Sprite sprite)
        //Crea un clon del objeto (plataforma) en el mismo lugar (lo uso para blur).
    {
        //Comprobamos que el clon no existe:
        bool exists = false;
        if (original != null)
        {
            foreach (Transform child in original.transform)
            {
                if(child.gameObject.name == "Blurred Clone")
                {
                    exists = true;
                }

            }

            if (!exists)
            {
                //El código solo se ejecuta para los objetos con la etiqueta ground y Sprite Renderer (plataformas):
                if (original.tag == "ground" && original.GetComponent<SpriteRenderer>() != null)
                {
                    //Instanciamos un nuevo objeto (con un Sprite Renderer vacío) usando el original como Parent:            
                    var newObject = Instantiate(EmptyObject, original.transform);
                    newObject.name = "Blurred Clone";

                    
                    
                    var tempMaterial = new Material(newObject.GetComponent<SpriteRenderer>().sharedMaterial);
                    tempMaterial.color = ColorBase;
                    newObject.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;
                    

                    //SetShaderColor(newObject, ColorBase);
                    //newObject.GetComponent<SpriteRenderer>().material.color = ColorBase;

                    newObject.transform.position = original.transform.position;
                    //ewObject.transform.localScale = original.transform.localScale;
                    // newObject.transform.localScale = new Vector3(1.3f,1.3f,1.3f);

                    //Asignamos el sprite al clon:
                    newObject.GetComponent<SpriteRenderer>().sprite = sprite;
                    print("nuevoSprite");

                }
            }
        }
    }


    void Start()
    {
        
        EmptyObject = Resources.Load("EmptyPlat") as GameObject;
        ButtonOn = ButtonOff;
        trans_vel = PlayerAuto.tvel;
        fondo_spr = GameObject.Find(NombreFondo);
        //tm = GetComponent<Tilemap>();
        //otros = GameObject.FindWithTag("GREEN").GetComponent<SpriteRenderer>();      
        GetChildArray(gameObject, true);


        if (ColorActual.Value)
        {
            //Asignar valores
            alpha = 1f;            

            //SetShaderAlpha(fondo_spr, 2 * (alpha - 0.5f));
            foreach (GameObject a in ChildArray)
            {

                //Aplicar valores a hijos
                ChildUpdate(a);
                OriginalPlats(a);
                if (a!=null && a.GetComponent<Obstaculo>() != null && platsLook!=null)
                {
                    BlurredClone(a, platsLook.PinchosBlurred);
                }
            }

            if (GameObject.Find(NombreBoton) != null)
            {
                GameObject.Find(NombreBoton).GetComponent<Image>().sprite = ButtonOn;
            }
        }

        else
        {
            //Asignar valores
            alpha = 0;            

            //SetShaderAlpha(fondo_spr, 2 * (alpha - 0.5f));
            foreach (GameObject a in ChildArray)
            {
                //Aplicar valores a hijos
                ChildUpdate(a);
                OriginalPlats(a);
                if (a != null && platsLook!=null)
                {
                    if (a.GetComponent<Obstaculo>() != null|| a.transform.parent.GetComponent<Obstaculo>() != null)
                    {
                        BlurredClone(a, platsLook.PinchosBlurred);
                    }
                    else
                    {

                        //print(a.name);
                        BlurredClone(a, platsLook.PlatsBlurred);
                    }
                }
            }

            if (GameObject.Find(NombreBoton) != null)
            {
                GameObject.Find(NombreBoton).GetComponent<Image>().sprite = ButtonOff;
            }
        }



                     
    }

    // Update is called once per frame
    void Update()
    {

        if (ColorActual.Value)
        {
            if (alpha > 1)
            {
                alpha = 1;

                foreach (GameObject a in ChildArray)
                {
                    ChildUpdate(a);
                }

            }
            else if (alpha < 1f)
            {
                alpha += trans_vel * Time.deltaTime;

                //SetShaderAlpha(fondo_spr, 2 * (alpha - 0.5f));
                //ChildUpdate(fondo_spr);

                foreach (GameObject a in ChildArray)
                {
                    ChildUpdate(a);
                }

            }



            if (GameObject.Find(NombreBoton) != null)
            {
                GameObject.Find(NombreBoton).GetComponent<Image>().sprite = ButtonOn;
            }

        }


        else
        {
            if (alpha < 0)
            {
                alpha = 0;
                               
                foreach (GameObject a in ChildArray)
                {
                    ChildUpdate(a);
                }

            }
            else if (alpha > 0)
            {
                alpha -= trans_vel * Time.deltaTime;
                //SetShaderAlpha(fondo_spr, 2 * (alpha - 0.5f));
                //ChildUpdate(fondo_spr);
                foreach (GameObject a in ChildArray)
                {
                    ChildUpdate(a);
                }
            }

                       
            if (GameObject.Find(NombreBoton) != null)
            {
                GameObject.Find(NombreBoton).GetComponent<Image>().sprite = ButtonOff;
            }
        }
        
    }

    protected void SetShaderColor(GameObject go, Color c)
    {
        if (go != null)
        {
            if (go.GetComponent<TilemapRenderer>() != null)
            {
                var tempMaterial = new Material(go.GetComponent<TilemapRenderer>().sharedMaterial);
                tempMaterial.SetColor("_color", c);
                go.GetComponent<TilemapRenderer>().sharedMaterial = tempMaterial;
            }
            else

        if (go.GetComponent<LineRenderer>() != null)
            {
                var tempMaterial = new Material(go.GetComponent<LineRenderer>().sharedMaterial);
                tempMaterial.SetColor("_color", c);
                go.GetComponent<LineRenderer>().sharedMaterial = tempMaterial;
            }
            else

        if (go.GetComponent<SpriteRenderer>() != null)
            {
                var tempMaterial = new Material(go.GetComponent<SpriteRenderer>().sharedMaterial);
                tempMaterial.SetColor("_color", c);
                go.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;
            }
        }
    }

    protected void SetShaderAlpha(GameObject go, float a)
    {
        if (go != null)
        {

            if (go.GetComponent<TilemapRenderer>() != null && go.GetComponent<TilemapRenderer>().sharedMaterial != null)
            {
                var tempMaterial = new Material(go.GetComponent<TilemapRenderer>().sharedMaterial);
                tempMaterial.SetFloat("_alpha", a);
                go.GetComponent<TilemapRenderer>().sharedMaterial = tempMaterial;
            }
            else

            if (go.GetComponent<LineRenderer>() != null && go.GetComponent<LineRenderer>().sharedMaterial != null)
            {
                var tempMaterial = new Material(go.GetComponent<LineRenderer>().sharedMaterial);
                tempMaterial.SetFloat("_alpha", a);
                go.GetComponent<LineRenderer>().sharedMaterial = tempMaterial;
            }
            else

            if (go.GetComponent<SpriteRenderer>() != null && go.GetComponent<SpriteRenderer>().sharedMaterial != null)
            {
                var tempMaterial = new Material(go.GetComponent<SpriteRenderer>().sharedMaterial);
                tempMaterial.SetFloat("_alpha", a);
                go.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;

            }
        }
    }

    protected void SetShaderFloat(GameObject g_object, float Value, string PropertyName)
    {

        if (g_object != null)
        {
            if (g_object.GetComponent<TilemapRenderer>() != null && g_object.GetComponent<TilemapRenderer>().sharedMaterial != null)
            {
                var tempMaterial = new Material(g_object.GetComponent<TilemapRenderer>().sharedMaterial);
                tempMaterial.SetFloat(PropertyName, Value);
                g_object.GetComponent<TilemapRenderer>().sharedMaterial = tempMaterial;
            }
            else

        if (g_object.GetComponent<LineRenderer>() != null && g_object.GetComponent<LineRenderer>().sharedMaterial != null)
            {
                var tempMaterial = new Material(g_object.GetComponent<LineRenderer>().sharedMaterial);
                tempMaterial.SetFloat(PropertyName, Value);
                g_object.GetComponent<LineRenderer>().sharedMaterial = tempMaterial;
            }
            else

        if (g_object.GetComponent<SpriteRenderer>() != null && g_object.GetComponent<SpriteRenderer>().sharedMaterial!=null)
            {
                var tempMaterial = new Material(g_object.GetComponent<SpriteRenderer>().sharedMaterial);
                tempMaterial.SetFloat(PropertyName, Value);
                g_object.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;

            }
        }
    }
}
