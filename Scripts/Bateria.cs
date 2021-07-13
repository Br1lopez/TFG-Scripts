using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bateria : MonoBehaviour
{
    Image img;
    private int _num = 0;
    public  int num
    {
        get
        {
            return _num;
        }
        set
        {
            if (value < 4 && value > -1)
            {
                _num = value;
            }
            else
            {
                _num = 3;
            }
                UpdateBattery(_num);
            
        }
    }

    public Sprite sprite_0;
    public Sprite sprite_1;
    public Sprite sprite_2;
    public Sprite sprite_3;

    // Start is called before the first frame update
    void Start()
    {
       img=gameObject.GetComponent<Image>();
        UpdateBattery(_num);


        StaticProperties.battery = this;
    }

    void UpdateBattery(int charge)
    {
        switch (charge)
        {
            case 0:
                img.sprite = sprite_0;
                break;
            case 1:
                img.sprite = sprite_1;
                break;
            case 2:
                img.sprite = sprite_2;
                break;
            case 3:
                AudioManager.instance.Play("BatteryFull");
                img.sprite = sprite_3;
                break;
        }

    }
}
