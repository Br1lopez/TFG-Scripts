using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGUI : MonoBehaviour
{
    //TIPO DE CUADRO DE TEXTO
    DialogueLine.dialogueType _GUItype = DialogueLine.dialogueType.None;
    public DialogueLine.dialogueType GUItype
    {
        get
        {
            return _GUItype;
        }
        set
        {
            switch (value)
            {
                case DialogueLine.dialogueType.None:
                    ActivateGUIObject(0);
                    break;
                case DialogueLine.dialogueType.Top:
                    ActivateGUIObject(1);
                    break;
                case DialogueLine.dialogueType.Bottom:
                    ActivateGUIObject(2);
                    break;
                case DialogueLine.dialogueType.Fullscreen:
                    ActivateGUIObject(3);
                    break;
                case DialogueLine.dialogueType.BottomLeft:
                    ActivateGUIObject(4);
                    break;
                case DialogueLine.dialogueType.BottomRight:
                    ActivateGUIObject(5);
                    break;
                case DialogueLine.dialogueType.Top_AI:
                    ActivateGUIObject(6);
                    break;
                case DialogueLine.dialogueType.Top_Blinking:
                    ActivateGUIObject(7);
                    break;
            }
            _GUItype = value;
        }
    }
    public DialogueLine.dialogueImage GUIImage;

    //OBJETOS CON DISTINTOS TIPOS
    Dictionary<int, GameObject> objs = new Dictionary<int, GameObject>();
    public GameObject obj_top;
    public GameObject obj_bottom;
    public GameObject obj_fullscreen;
    public GameObject obj_bottom_left;
    public GameObject obj_bottom_right;
    public GameObject obj_top_ai;
    public GameObject obj_top_blinking;

    public DialogueLine currentDialogueLine;

    GameObject activeObject;

    //TEXTO
    //[System.NonSerialized] public string text;
    //[System.NonSerialized] public bool skippable;

    //IMAGENES
    public Sprite spr_David;
    public Sprite spr_Miembro1;
    public Sprite spr_Miembro2;
    public Sprite spr_Emma;

    public string text = "";
    public bool skippable = false;

    private void Awake()
    {
        StaticProperties.dialogueGUI = this;

    }

    private void Start()
    {
        objs.Add(1, obj_top);
        objs.Add(2, obj_bottom);
        objs.Add(3, obj_fullscreen);
        objs.Add(4, obj_bottom_left);
        objs.Add(5, obj_bottom_right);
        objs.Add(6, obj_top_ai);
        objs.Add(7, obj_top_blinking);

        foreach (GameObject g in objs.Values)
        {
            g.SetActive(false);
        }
    }


    public void ChangeGUI(DialogueLine dialogue)
    {
        currentDialogueLine = dialogue;
        GUItype = dialogue.type;
        UpdateSkipButton(dialogue.skipButton);
        UpdateText(dialogue.text);
        UpdateFont(dialogue.font);
        UpdateColor(dialogue);
        UpdateImage(dialogue.image);
        AudioManager.instance.Play("clic");
    }

    void ActivateGUIObject(int i)
    {
        foreach (GameObject g in objs.Values)
        {
            g.SetActive(false);
        }

        if (i != 0)
        {
            objs[i].SetActive(true);
            activeObject = objs[i];
        }        
    }

    void UpdateText(string s)
    {        
        text = s;
        activeObject.GetComponent<TextsReference>().obj_text.GetComponent<TMPro.TextMeshProUGUI>().text = s;       
    }

    void UpdateFont(TMPro.TMP_FontAsset f)
    {
        if (f != null)
        {

            activeObject.GetComponent<TextsReference>().obj_text.GetComponent<TMPro.TextMeshProUGUI>().font = f;

        }
    }
    void UpdateSkipButton(bool b)
    {
        skippable = b;


        activeObject.GetComponent<TextsReference>().obj_skip.GetComponent<BlinkingText>().invisible = !b;


    }

    void UpdateColor(DialogueLine d)
    {
        if (d.OverrideColor)
        {

            activeObject.GetComponent<TextsReference>().obj_text.GetComponent<TMPro.TextMeshProUGUI>().color = d.Color;

        }

    }

    void UpdateImage(DialogueLine.dialogueImage img)
    {
        if (activeObject.GetComponent<TextsReference>().obj_image != null)
        {
            activeObject.GetComponent<TextsReference>().obj_image.sprite = null;
            switch (img)
            {
                case DialogueLine.dialogueImage.David:
                    activeObject.GetComponent<TextsReference>().obj_image.sprite = spr_David;
                    break;
                case DialogueLine.dialogueImage.Miembro1:
                    activeObject.GetComponent<TextsReference>().obj_image.sprite = spr_Miembro1;
                    break;
                case DialogueLine.dialogueImage.Miembro2:
                    activeObject.GetComponent<TextsReference>().obj_image.sprite = spr_Miembro2;
                    break;
                case DialogueLine.dialogueImage.Emma:
                    activeObject.GetComponent<TextsReference>().obj_image.sprite = spr_Emma;
                    break;
                case DialogueLine.dialogueImage.None:
                    activeObject.GetComponent<TextsReference>().obj_image.sprite = null;
                    break;
            }
        }
    }
}
