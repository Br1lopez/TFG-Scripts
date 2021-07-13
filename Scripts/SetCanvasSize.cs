using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
