using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ContrasrestarPadre : MonoBehaviour
{

    void Update()
    {
        transform.localScale = new Vector2(1, 1) / transform.parent.parent.localScale;
    }
}
