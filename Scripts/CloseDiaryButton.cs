using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDiaryButton : ButtonOrder
{
    public GameObject pause_obj;
    public GameObject diary_obj;

    public override void Activate()
    {
        pause_obj.SetActive(true);
        diary_obj.SetActive(false);
    }
}
