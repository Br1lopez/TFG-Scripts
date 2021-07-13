using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatsLook : GameClass
{ 
      Metodos myMethods = new Metodos();
      Material tempMaterial;
      public Sprite PinchosBlurred;
        public Sprite PlatsBlurred;

    // Start is called before the first frame update
    [ExecuteInEditMode]

    // Start is called before the first frame update
    private void Awake()
    {
        platsLook = this;
    }
    void Start()
    {

        if (PlatsBlurred = null)
        {
            PlatsBlurred = Sprite.Create(Resources.Load("BlurredPlat.png") as Texture2D, new Rect(), new Vector2());
        }

        if (PinchosBlurred = null)
        {
            PinchosBlurred = Sprite.Create(Resources.Load("BlurredPinchos.png") as Texture2D, new Rect(), new Vector2());
        }


        myMethods.GetChildArray(gameObject, true);

    foreach (GameObject g in myMethods.ChildArray)
    {
        if (g != null)
        {
            if (g.GetComponent<SpriteRenderer>() != null)
            {
                //print(g.name);
                tempMaterial = g.GetComponent<SpriteRenderer>().sharedMaterial;
                tempMaterial.SetFloat("_scaleX", (1 - 0.15f / g.transform.localScale.x));
                tempMaterial.SetFloat("_scaleY", (1 - 0.15f / g.transform.localScale.y));
                g.GetComponent<Renderer>().sharedMaterial = tempMaterial;
                //print(g.transform.localScale.x);
            }
        }
    }
}

}
