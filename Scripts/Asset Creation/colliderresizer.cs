using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class colliderresizer : MonoBehaviour
{
    Vector2 tempVector = new Vector2();

    private void Start()
    {
        tempVector = gameObject.GetComponent<SpriteRenderer>().size;
    }
    private void Update()
    {
        if (tempVector != gameObject.GetComponent<SpriteRenderer>().size)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null && gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().size - new Vector2(0.67f, 0.08f);
                tempVector = gameObject.GetComponent<SpriteRenderer>().size;
            }
        }
    }
       
}
