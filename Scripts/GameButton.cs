using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : ActivationObject
{
    public bool pressed = false;
    public Sprite Spr_ButtonActive;
    Sprite Spr_ButtonNotActive;
    //public Sprite Spr_ButtonNotActive;
    public bool stayPressed = true;


    private void Start()
    {
        Spr_ButtonNotActive = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if(collision.gameObject == player.gameObject)
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (stayPressed == false)
        {
            if (collision.gameObject == player.gameObject)
            {
                Deactivate();
            }
        }
    }


    void Activate()
    {
        pressed = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = Spr_ButtonActive;
    }

    void Deactivate()
    {
        pressed = false;
    }
}
