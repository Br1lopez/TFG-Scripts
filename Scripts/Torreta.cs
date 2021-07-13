using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public SpriteRenderer sr;
    public LayerMask lm;

    bool _idle;
    public bool idle
    {
        get
        {
            if (gameObject.layer == StaticProperties.CurrentLayerIndex|| gameObject.layer == 11)
            {
                _idle = false;
            }
            else {
                _idle = true;                
            }
             
            return _idle;
        }

        set
        {
            _idle = value;
        }
    }

    public Rigidbody2D rb;
    public GameObject bala_obj;
    [SerializeField] float velBala;
    [SerializeField] float alcanceBala;
    public float shootInterval;
    public bool balaReady = true;

    Vector2 pos = new Vector2(0, 0);
    RaycastHit2D[] hits = new RaycastHit2D[8];

    public AudioSource audioSource;

    bool LineHits(RaycastHit2D[] rc_array)
    {
        if (rc_array != null)
        {
            foreach (RaycastHit2D rc in rc_array)
            {

                if (rc.collider != null&&!rc.collider.gameObject.HasComponent<Torreta>())
                {
                    if (rc.collider.gameObject.tag == "ground" && (rc.collider.gameObject.layer == StaticProperties.CurrentLayerIndex || rc.collider.gameObject.layer == 11))
                    {
                        return true;
                    }

                    if (rc.collider.gameObject.layer == 12)
                        {
                            return false;
                        }

                }

            }
        }
        return false;
    }



    void Start()
    {
        pos = gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!idle && GameClass.player!=null)
        {
            Physics2D.LinecastNonAlloc(gameObject.transform.position, GameClass.player.transform.position, hits);
            Debug.DrawLine(gameObject.transform.position, GameClass.player.transform.position,Color.white,0.02f,false);

            if (LineHits(hits))
            {
                //sr.color = Color.black;

            }
            else
            {
                //sr.color = Color.yellow;
                rb.rotation = 180 + Mathf.Atan2(GameClass.player.transform.position.y - gameObject.transform.position.y, GameClass.player.transform.position.x - gameObject.transform.position.x) * 180 / Mathf.PI;
                if (!StaticProperties.blocked)
                    StartCoroutine(MiraComoMueres());
                    if (balaReady)
                {
                    Disparar();
                    StartCoroutine(descansoBala());
                }
            }
        }
    }

    void Disparar()
    {
        if(audioSource!= idle)
        {
            audioSource.Play();
        }
        var nuevabala_torreta = Instantiate(bala_obj);
        nuevabala_torreta.GetComponent<Bala>().angle = rb.rotation - 90 + Random.Range(-5,5);
        nuevabala_torreta.GetComponent<Bala>().rb.position = rb.position;
        nuevabala_torreta.GetComponent<Bala>().alcance = alcanceBala;
        nuevabala_torreta.GetComponent<Bala>().vel = velBala;
    }

    IEnumerator MiraComoMueres()
    {
        StaticProperties.blocked = true;
        yield return new WaitForSecondsRealtime(0.3f);
            StaticMethods.BlockAndRestart(0.8f);
    }

    IEnumerator descansoBala()
    {
        balaReady = false;
        yield return new WaitForSeconds(shootInterval);
        balaReady = true;
    }
}
