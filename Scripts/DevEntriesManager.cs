using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevEntriesManager : MonoBehaviour
{
    public GameObject obj_banner;
    public GameObject obj_popup;
    public TMPro.TextMeshProUGUI text;
    public enum DevState {Off, Banner, PopUp }
    public DevState state = DevState.Off;

    private void Awake()
    {
        StaticProperties.DevEntryManager = this;
        ClosePopUp();
    }

    public void StartBanner()
    {
        state = DevState.Banner;
        obj_banner.SetActive(true);
        obj_popup.SetActive(false);
        StartCoroutine(BannerTimer());
    }

    public void CloseBanner()
    {
        obj_banner.SetActive(false);
        obj_popup.SetActive(false);
        state = DevState.Off;
    }


    void StartPopUp()
    {
        StaticMethods.FreezeGame();
        state = DevState.PopUp;
        obj_banner.SetActive(false);
        obj_popup.SetActive(true);        
    }

    void ClosePopUp()
    {
        StaticMethods.UnfreezeGame();
        state = DevState.Off;
        obj_banner.SetActive(false);
        obj_popup.SetActive(false);        
    }

    public void ChangeText(string s)
    {
        text.text = s;
    }

    IEnumerator BannerTimer()
    {
        yield return new WaitForSeconds(3);
        if(obj_banner.active)
        CloseBanner();
    }

    private void Update()
    {
        switch (state)
        {
            case DevState.Banner:
                if (StaticProperties.controles.Menu.EnterBanner.triggered&& StaticProperties.PausedByMenu == false)
                    StartPopUp();
                break;
            case DevState.PopUp:
                if (StaticProperties.controles.Interact.Next.triggered&&StaticProperties.PausedByMenu == false)
                    ClosePopUp();
                break;
        }
    }
}