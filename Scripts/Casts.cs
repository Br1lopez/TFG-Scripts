using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casts : MonoBehaviour {

    Vector2 cast_Vector = new Vector2(0, -1);
    RaycastHit2D[] cast_hit = new RaycastHit2D[1];
    public int hitted;

    // Use this for initialization
    void Start () {
       

        hitted = GetComponent<BoxCollider2D>().Cast(cast_Vector, cast_hit, 2, true);

     

        for (int i = 0; i < 1; i++)
        {
            print("Colision: " + hitted);

           foreach (RaycastHit2D item in cast_hit)
            {
                print("print 1 " + item.distance);
            }
            print("print 2 " + cast_hit[0].distance);
        }

    }
	
	// Update is called once per frame
	void Update () {
 
    }

   

}
