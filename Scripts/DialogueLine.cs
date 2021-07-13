using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueLine : MonoBehaviour
{
    public enum dialogueType { None, Top, Bottom, Fullscreen, BottomLeft, BottomRight, Top_AI , Top_Blinking};
    public enum dialogueImage { None, David, Miembro1, Miembro2, Emma};

    public string[] textLines = new string[1];
    [System.NonSerialized]public string text = null;
    public dialogueType type;
    public dialogueImage image;
    public bool skipButton = true;
    public TMPro.TMP_FontAsset font;
    public bool OverrideColor = false;
    public Color Color = new Color();

    private void Start()
    {
            for (int i = 0; i < (textLines.Length); i++)
            {
            textLines[i] = textLines[i].ToUpper();

            textLines[i] = textLines[i].Replace("X_", "<sprite name=xbox_");
            textLines[i] = textLines[i].Replace("_X", ">");
            text = text + textLines[i];
                if (i < textLines.Length - 1)
                {
                    text = text + "\n\n";
                }
            }
    }

}
