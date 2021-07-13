using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ojos : GameClass
{
    Vector2 startPos;
   
    [SerializeField] Vector3 Multiplier;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = startPos + player.mov.vel*Multiplier;
    }
}
