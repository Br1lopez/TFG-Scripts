using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Col_interior : GameClass
{  


    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            BlockAndRestart(1);
        }

    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground") {
            BlockAndRestart(1);
        }
    }

    }
