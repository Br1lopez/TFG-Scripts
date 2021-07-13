using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rename : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.name = t.gameObject.name.Replace("_B_", "_main_");
            foreach (Transform t2 in t.transform)
            {
                foreach(Transform t3 in t2.transform)
                {
                    t3.gameObject.name = t3.gameObject.name.Replace("BLUE", "MAIN");
                }
            }
        }
    }
}
