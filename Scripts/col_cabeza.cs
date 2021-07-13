using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class col_cabeza : MonoBehaviour {

    private PlayerAuto player;

    // Use this for initialization
    void Start()
    {
        player = GetComponentInParent<PlayerAuto>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            player.mov.colision_cabeza = true;
        }
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            player.mov.colision_cabeza = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            player.mov.colision_cabeza = false;
        }
    }
}
