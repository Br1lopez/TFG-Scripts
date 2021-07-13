using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryButton : ButtonOrder
{
    public GameObject pause_obj;
    public GameObject diary_obj;

    public override void Activate()
    {
        pause_obj.SetActive(false);
        diary_obj.SetActive(true);
    }
}
