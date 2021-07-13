using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevEntry : MonoBehaviour
{
    public bool unlocked = false;
    public string text;

    public DevEntry(bool Unlocked, string Text)
    {
        unlocked = Unlocked;
        text = Text;
        text = text.ToUpper();
    }

}
