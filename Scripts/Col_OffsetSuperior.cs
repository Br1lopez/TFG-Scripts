using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col_OffsetSuperior : GameClass
{
    public BoolRef OffsetSuperiorCol = new BoolRef(false);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OffsetSuperiorCol.Value = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OffsetSuperiorCol.Value = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OffsetSuperiorCol.Value = false;
    }
}
