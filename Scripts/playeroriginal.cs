using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeroriginal : MonoBehaviour
{



    [SerializeField] float mov_escala = 0.15f;
    [SerializeField] float grav_max = 15f;
    [SerializeField] float grav_escala = 0.01f;
    [SerializeField] float salto = 0.15f;
    public Vector2 grav_dir = new Vector2(0, -1);
    Vector2 grav_vector = new Vector2(0, 0);
    float grav_fuerza = 0f;
    int saltando;
    float mov_direccion;

    //Velocidad que lleva:
    float hsp;
    float vsp;

    //Sitio donde se dibuja:


    bool colision;
    Vector2 vel;
    Vector2 xy;

    int colision_num_h;
    int colision_num_v;
    int colision_num_d;
    RaycastHit2D[] colision_punto_h = new RaycastHit2D[1];
    RaycastHit2D[] colision_punto_v = new RaycastHit2D[1];
    RaycastHit2D[] colision_punto_d = new RaycastHit2D[1];

    float dist_min = 0.01f;
    Vector2 dist_min_vector = new Vector2(0.01f, 0.01f);





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
        Vector2 xy = new Vector2(0, 0);

        //Inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            saltando = 1;
        }

        mov_direccion = Input.GetAxisRaw("Horizontal");

        //Formula: velocidad horizontal
        hsp = mov_escala * mov_direccion;

        // print(grav_fuerza);



        //Formula: velocidad vertical

        if (grav_fuerza < grav_max)
        {
            grav_fuerza += grav_escala;
        }
        else
        {
            grav_fuerza = grav_max;
        }


        grav_vector = grav_dir * grav_fuerza;

        //Vectores
        Vector2 vel = new Vector2(hsp + grav_vector.x, vsp + grav_vector.y);
        Vector2 vel_v = new Vector2(0, vel.y);

        colision_num_v = GetComponent<PolygonCollider2D>().Cast(new Vector2(0, Mathf.Sign(vel.y)), colision_punto_v, vel.y, true);
        colision_num_h = GetComponent<PolygonCollider2D>().Cast(new Vector2(vel.x, 0), colision_punto_h, vel.x, true);
        colision_num_d = GetComponent<PolygonCollider2D>().Cast(vel, colision_punto_d, vel.magnitude, true);
        // print(colision_num_v);
        //Colision: velocidad vs dibujado
        // print("h " + vel_h + " v " + vel_v);

        if (colision == true)
        {

            if (colision_num_h > 0)
            {
                xy.x = 0;
                //colision = true;
                print("choque h");
                grav_fuerza -= (Mathf.Abs(grav_dir.x) * grav_fuerza);

            }
            else
            {
                xy.x = vel.x;
                //colision = false;
                print("no choque h");
            }




            if (colision_num_v > 0)
            {
                xy.y = 0;
                //colision = true;
                print("choque v");
                grav_fuerza -= (Mathf.Abs(grav_dir.y) * grav_fuerza);

            }
            else
            {
                xy.y = vel.y;
                //colision = false;
                print("no choque v ");
                // print("nochoquev " + new Vector2(0, vel.y) + colision_punto_v + vel.y);
                //print(colision_num_v);
            }
        }


        if (colision_num_d > 0)
        {
            if (colision == false)
            {
                xy = vel - (vel * (1 - colision_punto_d[0].fraction)) - (dist_min_vector * vel);
            }
            colision = true;
            print("choque");
        }
        else
        {
            xy = vel;
            colision = false;
            print("no choque");

        }
        /*if (colision == true)
        {
            grav_fuerza = 0;
        }
        */
        //print("Velocidad: " + vel + " Gravedad: " + grav_vector + " xy: " + xy );

        //Dibujado final
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + xy);



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
     */

}
