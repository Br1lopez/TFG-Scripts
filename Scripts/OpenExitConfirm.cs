using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpenExitConfirm : ButtonOrder
{
    public GameObject exitConfirm_obj;
    public GameObject pauseMenu_obj;
    // Start is called before the first frame update
    public override void Activate()
    {
        exitConfirm_obj.SetActive(true);
        pauseMenu_obj.SetActive(false);
    }
}
