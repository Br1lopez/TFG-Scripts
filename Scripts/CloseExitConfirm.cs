using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseExitConfirm : ButtonOrder
{
    public GameObject exitConfirm_obj;
    public GameObject pauseMenu_obj;
    // Start is called before the first frame update
    public override void Activate()
    {
        pauseMenu_obj.SetActive(true);
        exitConfirm_obj.SetActive(false);
    }
}
