using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;
    public Sprite sprite;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (sr.sprite != sprite)
            {
                AudioManager.instance.Play("CP");
            }
            StaticProperties.SpawnPos = gameObject.transform.position;
            anim.enabled = false;
            sr.sprite = sprite;
        }
    }

}
