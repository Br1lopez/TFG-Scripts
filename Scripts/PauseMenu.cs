using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject obj_PauseMenu;

    private void Awake()
    {
        StaticProperties.PauseMenu = obj_PauseMenu;
    }
    private void Start()
    {
        StaticMethods.Pause(false);
        StaticProperties.controles.Menu.Pause.started += ctx => ChangeState();
    }

    void ChangeState()
    {        
        if(!StaticProperties.DevEntryManager.obj_banner.active&& !StaticProperties.DevEntryManager.obj_popup.active && StaticProperties.pauseEnabled)
        StaticMethods.Pause(!obj_PauseMenu.activeSelf);
    }


}
