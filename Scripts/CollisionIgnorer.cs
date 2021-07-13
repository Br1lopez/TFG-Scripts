using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    public Collider2D col1;
    public Collider2D col2;

    private void Start()
    {
        Physics2D.IgnoreCollision(col1, col2, true);
    }
}
