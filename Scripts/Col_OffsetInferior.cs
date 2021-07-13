using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col_OffsetInferior : GameClass
{
    
    public BoolRef OffsetInferiorCol = new BoolRef(false);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OffsetInferiorCol.Value = true;
        }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OffsetInferiorCol.Value = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
        {
            OffsetInferiorCol.Value = false;
        }
    
}
