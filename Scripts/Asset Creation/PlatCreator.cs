using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatCreator : MonoBehaviour
{
    float i = 1;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
     foreach (Transform t in transform)
        {

            t.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(i/10, 0.1f);

            float sizesOffset = i / 20 - 0.01f;
            float centerScale = i - 0.4f;

            string iString = "";

            if (i < 10)
            {
               iString = " (0," + i.ToString() + " m)";
            }else
            {
                iString = " (" + (i/10).ToString() + " m)";
            }
  


            //t.name = (name+iString);
            foreach (Transform t2 in t.transform)
            {
                if (t2.name == "L")
                {
                    t2.localPosition = new Vector2((-sizesOffset), 0);
                }
                else if (t2.name == "R")
                {
                    t2.localPosition = new Vector2((sizesOffset), 0);
                }
                else if (t2.name == "C")
                {
                    t2.transform.localScale = new Vector3(centerScale, 1, 1);
                }
                if (t2.gameObject.name == "ON")
                {
                    foreach (Transform t3 in t2.transform)
                    {
                        if (t3.name == "L")
                        {
                            t3.localPosition = new Vector2((-sizesOffset), 0);
                        }
                        else if (t3.name == "R")
                        {
                            t3.localPosition = new Vector2((sizesOffset), 0);
                        }
                        else if (t3.name == "C")
                        {
                            t3.transform.localScale = new Vector3(centerScale, 1, 1);
                        }
                    }
                }
                /*else if (t2.gameObject.name == "OFF")
                {
                    foreach (Transform t3 in t2.transform)
                    {
                        if (t3.name == "L")
                        {
                            t3.localPosition = new Vector2(-((i/20)-0.05f), 0);
                        }
                        else if (t3.name == "R")
                        {
                            t3.localPosition = new Vector2(((i / 20) - 0.05f), 0);
                        }
                        else if (t3.name == "C")
                        {
                            t3.transform.localScale = new Vector3(i-1, 1, 1);
                        }
                    }
                }*/
            }
            i++;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
