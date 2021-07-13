using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orders : MonoBehaviour
{
    public bool EnableNoShootingZone = false;
    public bool DisableNoShootingZone = false;

    public bool EnableFullBattery = false;
    public bool DisableFullBattery = false;

    public bool EnableLeftArrow;
    public bool DisableLeftArrow;

    public bool EnableUpArrow;
    public bool DisableUpArrow;

    public bool EnableRightArrow;
    public bool DisableRighttArrow;

    public bool EnableDialogueBox;
    public bool DisableDialogueBox;
    public DialogueBox TargetDialogueBox;

    public bool EnableOtherObject;
    public bool DisableOtherObject;
    public GameObject TargetObject;

    public bool QuitGame = false;

    public bool UnlockDevEntry = false;
    public int DevEntryToUnlock  = 1;

    public bool CortinillaOn = false;
    public bool CortinillaOff = false;
    public bool CortinillaOnAndOff = false;

    protected void RunOrders()
    {
        if (EnableNoShootingZone)
        {
            StaticMethods.EnableNoShootingZone();
        }

        if (DisableNoShootingZone)
        {
            StaticMethods.DisableNoShootingZone();
        }

        if (EnableFullBattery)
        {
            StaticProperties.battery.num = 3;
            StaticProperties.batteryAlwaysFull = true;
        }

        if (DisableFullBattery)
        {            
            StaticProperties.batteryAlwaysFull = false;
        }

        if (EnableLeftArrow)
        {
            GameClass.leftArrow.color = new Color(1, 0, 0, 1);
        }


        if (DisableLeftArrow)
        {
            GameClass.leftArrow.color = new Color(1, 0, 0, 0);
        }

        if (EnableRightArrow)
        {
            GameClass.rightArrow.color = new Color(0, 0, 1, 1);
        }

        if (DisableRighttArrow)
        {
            GameClass.rightArrow.color = new Color(0, 0, 1, 0);
        }

        if (EnableUpArrow)
        {
            GameClass.upArrow.color = new Color(0, 1, 0, 1);
        }

        if (DisableUpArrow)
        {
            GameClass.upArrow.color = new Color(0, 1, 0, 0);
        }

        if (EnableDialogueBox)
        {
            TargetDialogueBox.gameObject.SetActive(true);
            TargetDialogueBox.StartDialogue();
        }

        if (DisableDialogueBox)
        {
            TargetDialogueBox.EndDialogue();
        }

        if (EnableOtherObject)
        {
            TargetObject.gameObject.SetActive(true);
        }

        if (DisableOtherObject)
        {
            TargetObject.gameObject.SetActive(false);
        }

        if (QuitGame)
        {
            StaticMethods.QuitGame();
        }

        if (UnlockDevEntry)
        {
            StartCoroutine(WaitAndUnlockDevEntry());
        }

        if (CortinillaOn)
        {
            StaticProperties.cortinilla.On();
        }else if (CortinillaOff)
        {
            StaticProperties.cortinilla.Off();
        }
        else if (CortinillaOnAndOff)
        {
            StaticProperties.cortinilla.OnAndOff();
        }
    }

    IEnumerator WaitAndUnlockDevEntry()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.instance.Play("unlock");
        StaticMethods.UnlockDevEntry(StaticProperties.devEntries[DevEntryToUnlock - 1]);
    }
}
