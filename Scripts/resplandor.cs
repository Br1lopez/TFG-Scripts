using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resplandor : MonoBehaviour
{
    public SpriteRenderer sr;
    float alpha;
    enum textState {Appearing, Disappearing};
    textState state;

    float changeIndex;

    private void Start()
    {
        alpha = 0.8f;
        UpdateAlpha();
        state = textState.Disappearing;
    }

    private void Update()
    {
        changeIndex = 0.02f;

        if (alpha> 0.8f)
        {
            state = textState.Disappearing;
            alpha = 0.8f;
            UpdateAlpha();
        }
        else if (alpha<0)
        {
            state = textState.Appearing;
            alpha = 0;
            UpdateAlpha();
        }

        switch (state)
        {
            case textState.Disappearing:
                alpha -= changeIndex;
                UpdateAlpha();
                break;
            case textState.Appearing:
                alpha += changeIndex;
                UpdateAlpha();
                break;
        }
    }

    private void UpdateAlpha()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
}
