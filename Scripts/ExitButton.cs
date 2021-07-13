using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : ButtonOrder
{
    override public void Activate()
    {
        StaticMethods.QuitGame();
    }
}
