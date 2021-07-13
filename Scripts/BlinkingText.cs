using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tm;
    enum textState {Appearing, Disappearing};
    textState state;
    public bool invisible = false;
    public float min = 0;
    public float max = 1;
    public float speed = 0.02f;

    float changeIndex;

    private void Start()
    {
        tm.alpha = min;
        state = textState.Appearing;
    }

    private void Update()
    {
        if (invisible)
        {
            if (tm.alpha != min)
                tm.alpha = min;
        }
        else
        {
            changeIndex = speed;

            if (tm.alpha > max)
            {
                state = textState.Disappearing;
                tm.alpha = max;
            }
            else if (tm.alpha < min)
            {
                state = textState.Appearing;
                tm.alpha = min;
            }

            switch (state)
            {
                case textState.Disappearing:
                    tm.alpha -= changeIndex;
                    break;
                case textState.Appearing:
                    tm.alpha += changeIndex;
                    break;
            }
        }
    }
}
