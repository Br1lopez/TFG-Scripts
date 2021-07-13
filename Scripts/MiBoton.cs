using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiBoton : MonoBehaviour
{    
    public string Name;

    //Para acceder a esto: StaticProperties.MisBotones["Nombre"].Pressed
    public bool Pressed;



    // Start is called before the first frame update
    void Awake()
    {      
    }

    private void FixedUpdate()
    {
        Pressed = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Touch"|| col.gameObject.name == "Touch(Clone)")
        {
            Pressed = true;
        }
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Touch" || col.gameObject.name == "Touch(Clone)")
        {
            Pressed = true;
        }
    }
    

      private void OnTriggerExit2D (Collider2D col)
        {
            if (col.gameObject.name == "Touch" || col.gameObject.name == "Touch(Clone)")
            {
                Pressed = false;
            }
        }

/*
    void Update()
    {
        if (Pressed)
            print(Name+ " presion");

    }
    */
}

