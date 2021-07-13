using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : GameClass {

    public float _vida;
    public float vida
    {
        get
        {
            return _vida;
        }
        set
        {
                _vida = value;
                if (_vida < 0)
                {
                    if (!player.meleeAftermath&&sumaALaBateria)
                    {
                        StaticProperties.battery.num++;                    
                    }

                    Destroy(gameObject);
                    AudioManager.instance.Play("EnemyDeath");
            }
            else
            {
                AudioManager.instance.Play("EnemyHit");                
                if (srs != null)
                {
                    dañoAnim = true;
                    foreach (SpriteRenderer sr in srs)
                    {
                        sr.color = Color.black;
                        color_index = 0;
                    }
                        
                }

            }
        }
    }

    public bool sumaALaBateria = true;

    public float daño = 1;
    public bool destroysOnContact = false;
    Collider2D[] colArray = new Collider2D[16];
    ContactFilter2D cf = new ContactFilter2D();

    public SpriteRenderer[] srs;
    float color_index = 1;
    bool dañoAnim = false;

    private void Update()
    {
        if (srs != null&&dañoAnim) {

            foreach (SpriteRenderer sr in srs)
            {
                color_index += Time.unscaledDeltaTime * 1.9f;
                if (color_index < 1)
                {
                    sr.color = new Color(1, color_index, color_index);
                }
                else
                {
                    sr.color = Color.white;
                    dañoAnim = false;
                }
            }

                }
    }

    private void FixedUpdate()
    {
        if (vida != 0)
        {
            System.Array.Clear(colArray, 0, 16);
            if (gameObject.GetComponent<Collider2D>() != null)
            {
                gameObject.GetComponent<Collider2D>().OverlapCollider(cf, colArray);
                foreach (Collider2D col in colArray)
                {                    
                    if (col!=null && col.gameObject.GetComponent<Arma>() != null)
                    {
                        vida -= (col.gameObject.GetComponent<Arma>().daño + 0.001f);

                        if (col.gameObject.GetComponent<Arma>().destroyOnCollision)
                        {
                            Destroy(col.gameObject);
                        }
                    }
                }
            }
        }
    }


    public void OnDestroy()
    {
    }
}
