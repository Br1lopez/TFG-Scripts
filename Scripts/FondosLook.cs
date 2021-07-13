using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondosLook : MonoBehaviour
{
    Metodos myMethods = new Metodos();
    Material tempMaterial;

    // Start is called before the first frame update
    void Start()
    {
        myMethods.GetChildArray(gameObject, true);

        foreach (GameObject g in myMethods.ChildArray)
        {
            if (g != null)
            {
                if (g.GetComponent<SpriteRenderer>() != null)
                {
                    //print(g.name);
                    tempMaterial = g.GetComponent<SpriteRenderer>().sharedMaterial;
                    g.GetComponent<Renderer>().sharedMaterial = tempMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
