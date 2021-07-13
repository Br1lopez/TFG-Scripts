/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    [SerializeField] float mov_escala = 15f;
    [SerializeField] float grav_max = 15f;
    [SerializeField] float grav = 1f;
    [SerializeField] float salto = 15f;
    float hsp;
    float vsp;
    bool colision;


    // Use this for initialization
    void Start()
    {
        Conosle.WriteLine("hey");
        vsp = 10;
    }

    // Update is called once per frame
    void Update()
    {
        //Input izquierda derecha
        float hsp = mov_escala * Input.GetAxisRaw("Horizontal");

        Vector2 vel = new Vector2(hsp, -vsp);

        GetComponent<Rigidbody2D>().velocity = vel;


        if (colision == false)
        {
            if (vsp < grav_max)
            {
                vsp += grav;
            }
            else
            {
                vsp = grav_max;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vsp = salto * (-1);
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {

            colision = true;
            vsp = 0;

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {
            colision = false;


        }
    }


}
*/






/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    [SerializeField] float mov_escala = 0.15f;
    [SerializeField] float grav_max = 15f;
    [SerializeField] float grav = 0.1f;
    [SerializeField] float salto = 0.15f;
    int saltando;
    float dir_horizontal;

    //Velocidad que lleva:
    float hsp;
    float vsp;

    //Sitio donde se dibuja:
    float x = 0;
    float y = 0;

    bool colision;
    Vector2 vel;
    Vector2 xy;

    int colision_num;
    RaycastHit2D[] colision_punto = new RaycastHit2D[1];




    // Use this for initialization
    void Start()
    {
        print("hey");
        vsp = 0;
        colision = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            saltando = 1;
        }

        dir_horizontal = Input.GetAxisRaw("Horizontal");

        //Formula: velocidad horizontal
        hsp = mov_escala * dir_horizontal;

        //Formula: velocidad vertical
        if (vsp < grav_max)
        {
            vsp += grav;
        }
        else
        {
            vsp = grav_max;
        }

        //Vectores
        Vector2 vel = new Vector2(hsp, -vsp);
        Vector2 xy = new Vector2(x, y);

        //Colision: velocidad vs dibujado
        colision_num = GetComponent<PolygonCollider2D>().Cast(vel, colision_punto, vel.magnitude, true);

        if (colision_num > 0)
        {
            xy = vel * colision_punto[0].fraction;
            vel = new Vector2(0,0);
            
        }
        else
        {
            xy = vel;
        }

        
        //Dibujado final
        GetComponent<Rigidbody2D>().MovePosition (GetComponent<Rigidbody2D>().position + xy);


    }

   /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {

           colision = true;
           vsp = (-1) * saltando * salto;
            

        }
      
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap")
        {
            colision = false;

            saltando = 0;
        }
    }
   

}
 */