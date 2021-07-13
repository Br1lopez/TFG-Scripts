using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    [SerializeField] float mov_escala = 0.15f;
    [SerializeField] float grav_max = 15f;
    [SerializeField] float grav_escala = 0.01f;
    [SerializeField] float salto = 0.15f;
    public Vector2 grav_dir = new Vector2(0, 0);
    Vector2 grav_vector = new Vector2(0, 0);
    float grav_fuerza = 0f;
    int saltando;
    float dir_horizontal = 0;
    float dir_vertical = 0;

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
    int pruebaza;
    RaycastHit2D[] colision_punto_h = new RaycastHit2D[1];
    RaycastHit2D[] colision_punto_v = new RaycastHit2D[1];
    RaycastHit2D[] colision_punto_d = new RaycastHit2D[1];
    RaycastHit2D[] colision_prueba = new RaycastHit2D[1];

    float dist_min = 0.01f;
    Vector2 dist_min_vector = new Vector2(0.1f, 0.1f);





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

        dir_horizontal = Input.GetAxisRaw("Horizontal");
        dir_vertical = Input.GetAxisRaw("Vertical");




       // print(dir_horizontal + "  " + dir_vertical);

        //Formula: velocidad horizontal
        hsp = mov_escala * dir_horizontal;
        vsp = mov_escala * dir_vertical;

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
        Vector2 vel = new Vector2(hsp, vsp);
        Vector2 vel_v = new Vector2(0, vel.y);

        colision_num_v = GetComponent<PolygonCollider2D>().Cast(new Vector2(0, Mathf.Sign(vel.y)), colision_punto_v, vel.y, true);
        colision_num_h = GetComponent<PolygonCollider2D>().Cast(new Vector2(vel.x, 0), colision_punto_h, vel.x, true);
        colision_num_d = GetComponent<PolygonCollider2D>().Cast(vel, colision_punto_d, vel.magnitude, true);        

        // print(colision_num_v);
        //Colision: velocidad vs dibujado
        // print("h " + vel_h + " v " + vel_v);
        

        if (colision_num_d > 0)
        {
            if (colision == false)
           {
               xy = vel - (vel * (1 - colision_punto_d[0].fraction)) - (dist_min_vector * vel);
             
           }            
           else
            {

               if (colision_num_h > 0)
                {
                    xy.x = 0;
                    //colision = true;
                    //print("choque h");
                    //grav_fuerza -= (Mathf.Abs(grav_dir.x) * grav_fuerza);

                }
                else
                {
                    xy.x = vel.x;
                    //colision = false;
                    //print("no choque h");
                }
                
                if (colision_num_v > 0)
                {
                    xy.y = 0;
                    //colision = true;
                   // print("choque v");
                    //grav_fuerza -= (Mathf.Abs(grav_dir.y) * grav_fuerza);

                }
                else
                {
                   // xy.y = vel.y;
                    //colision = false;
                   // print("no choque v " + "Velocidad: " + vel + " Gravedad: " + grav_vector + " xy: " + xy);
                    // print("nochoquev " + new Vector2(0, vel.y) + colision_punto_v + vel.y);
                    //print(colision_num_v);
                }
            }
            colision = true;
            print("tru" + vel);
        }
        else
        {
            xy = vel;
            colision = false;
            print("false" + vel);
           // print("no choque" + xy.x + " y  " + xy.y + GetComponent<Rigidbody2D>().position);

        }



       


        /*if (colision == true)
        {
            grav_fuerza = 0;
        }
        */
        //print("Velocidad: " + vel + " Gravedad: " + grav_vector + " xy: " + xy );

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
    */

}
