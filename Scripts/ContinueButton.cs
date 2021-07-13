using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : ButtonOrder
{
    public GameObject obj_PauseMenu;

    override public void Activate()
    {
        StaticMethods.Pause(false);
    }
}
