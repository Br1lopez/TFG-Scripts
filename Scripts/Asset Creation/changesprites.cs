using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changesprites : MonoBehaviour
{
    public Sprite[] oldSprite;
    public Sprite[] newSprite;

    public Material oldMaterial;
    public Material newMaterial;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.GetComponent<BoxCollider2D>().size = t.gameObject.GetComponent<BoxCollider2D>().size * 1.5f;
            foreach (Transform t2 in t.transform)
            {                

                foreach (Transform t3 in t2.transform)
               {
                    t3.localPosition = t3.localPosition * 1.5f;
                    for (int i = 0; i < oldSprite.Length; i++)
                    {
                        if (t3.gameObject.GetComponent<SpriteRenderer>() != null && t3.gameObject.GetComponent<SpriteRenderer>().sprite == oldSprite[i])
                        {
                            t3.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite[i];
                        }
                    }

                    /*
                    for (int i = 0; i < oldSprite.Length; i++)
                    {
                        if (t2.gameObject.GetComponent<SpriteRenderer>() != null && t2.gameObject.GetComponent<SpriteRenderer>().sprite == oldSprite[i])
                        {
                            t2.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite[i];
                        }
                    }*/

                    /*if (t2.gameObject.GetComponent<SpriteRenderer>() != null && t2.gameObject.GetComponent<SpriteRenderer>().sharedMaterial == oldMaterial)
                    {
                        t2.gameObject.GetComponent<SpriteRenderer>().sharedMaterial = newMaterial;
                    }

                    if (t2.gameObject.layer == 8)
                    {
                        t2.gameObject.name = t2.gameObject.name.Replace("BLUE", "RED");
                        print("r");
                    }else if (t2.gameObject.layer == 9)
                    {
                        t2.gameObject.name = t2.gameObject.name.Replace("BLUE", "GREEN");
                    }
                    */


                }

            }
        }

    }

    // Update is called once per frame

    void Update()
    {

    }
}
