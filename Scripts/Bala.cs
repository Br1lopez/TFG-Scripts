using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float vel;
    public float angle;
    public Rigidbody2D rb;
    Vector2 vel_vector;
    public float alcance;
    public bool IsFromPlayer = false;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        //rb.rotation = -angle-90;
        transform.Rotate(0, 0, angle);
        angle = -angle+180;  
        vel_vector.x = Mathf.Sin(Mathf.Deg2Rad * angle) * vel/60;
        vel_vector.y = Mathf.Cos(Mathf.Deg2Rad * angle) * vel/60;
        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position += vel_vector;
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(alcance/vel);
        if (IsFromPlayer)
        {
            AudioManager.instance.Play("BulletHit");
        }
        if (explosion != null)
        {
            var newobject = Instantiate(explosion, transform.position, transform.rotation);
            newobject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFromPlayer)
        {
            AudioManager.instance.Play("BulletDestroy");
        }
        if (collision.gameObject.tag == "ground"&& !collision.gameObject.HasComponent<Obstaculo>())
        {
            if (explosion != null)
            {
                var newobject = Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {

    }
}
