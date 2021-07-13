using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aranha : MonoBehaviour
{
    float grav_fuerza = 0f;
    [SerializeField] Collider2D col;
    Collider2D[] colArray = new Collider2D[16];
    ContactFilter2D cf = new ContactFilter2D();

    public SpriteRenderer mySprite;

    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    float left_point;
    float right_point;

    bool goesRight = true;
    bool changedirection = false;

    public Animator myanim;

    public Rigidbody2D rb;

    Vector2 _vel;
    Vector2 vel
    {
        get
        {
            if (OnAir||changedirection)
            {                
                if (goesRight)
                {
                    if (rb.position.x > right_point)
                    {
                        changedirection = true;
                    }
                    else
                    {
                        _vel.x = 0.2f;
                    }                        
                }
                else
                {
                    if (rb.position.x < left_point)
                    {
                        changedirection = true;
                    }
                    else
                    {
                        _vel.x = -0.2f;
                    }
                }
                
            }
            else
            {
                _vel.x = 0;
            }

            _vel.y = -grav_fuerza;

            return _vel;
        }
        set
        {
            value = _vel;
        }
    }

    [SerializeField] float salto = 0.265f;
    [SerializeField] float intervalo_salto = 1;

    bool saltoReady = true;

    public AudioSource audioJump;
    public AudioSource audioLand;

    private void Start()
    {
        if(pos1.position.x> pos2.position.x)
        {
            right_point = pos1.position.x;
            left_point = pos2.position.x;
        }
        else
        {
            left_point = pos1.position.x;
            right_point = pos2.position.x;
            mySprite.transform.parent.Rotate(0, 180, 0);

        }

    }

    public bool OnAir
    {
        get
        {
            System.Array.Clear(colArray, 0, 16);
            col.OverlapCollider(cf, colArray);
            foreach (Collider2D col in colArray)
            {
                if (col != null && col.gameObject.tag == "ground")
                {
                    return false;
                }
            }
            return true;
        }
    }

    bool audioTrigger = false;

    private void FixedUpdate()
    {

        //Gravedad:
        if (OnAir)
        {
            audioTrigger = true;
            if (grav_fuerza < GameClass.player.mov.grav_max)
            {
                grav_fuerza += GameClass.player.mov.grav_escala;
            }
            else
            {
                grav_fuerza = GameClass.player.mov.grav_max;
            }      
        }
        else
        {            
            myanim.SetBool("Jumping", false);
            if (audioTrigger)
            {
                audioLand.Play();
                audioTrigger = false; 
            }
            //grav_fuerza = 0;
        }

        if (saltoReady)
        {
            if (audioJump != null)
            {
                audioJump.Play();
            }
            Saltar();
        }
        

        rb.MovePosition(rb.position + vel);
    }

    void Saltar()
    {
        myanim.SetBool("Jumping", true);
        grav_fuerza = -salto;
        StartCoroutine(descansosalto());
        if (changedirection)
        {
            if (goesRight)
            {
                mySprite.transform.parent.Rotate(0, 180, 0);
            }
            else
            {
                mySprite.transform.parent.Rotate(0, -180, 0);
            }
            goesRight = !goesRight;
            changedirection = false;
        }
    }

    IEnumerator descansosalto()
    {
        saltoReady = false;
        yield return new WaitForSeconds(intervalo_salto);
        saltoReady = true;
    }

}
