using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    DialogueLine[] dialogueArray;
    int currentIndex;
    public bool Spawn = false;

    private void Start()
    {
        SetDialogueLineArray();
        StaticProperties.controles.Interact.Next.started += ctx => NextLine();
    }



    void SetDialogueLineArray()
    {
        dialogueArray = new DialogueLine[transform.childCount];
        foreach (Transform t in transform)
        {
            dialogueArray[t.GetSiblingIndex()] = t.gameObject.GetComponent<DialogueLine>();
        }
    }

    public void StartDialogue()
    {
        if (Spawn)
            StartCoroutine(WaitAndStartDialogue(0.5f));
        else
            StartCoroutine(WaitAndStartDialogue(0));

    }
    public void EndDialogue()
    {
        PushNullDialogue();        
    }

    public void PrevLine()
    {
        if (StaticProperties.dialogueGUI.skippable && StaticProperties.PausedByMenu == false)
        {
            currentIndex--;
            PushCurrentDialogue();
        }
    }
    public void NextLine()
    {
        if (StaticProperties.dialogueGUI.skippable && StaticProperties.PausedByMenu == false)
        {
            currentIndex++;
            PushCurrentDialogue();
        }
    }

    void PushCurrentDialogue()
    {
        StartCoroutine(WaitAndPushCurrentDialogue());
    }
    IEnumerator WaitAndPushCurrentDialogue()
    {
        yield return new WaitForSecondsRealtime(0f);
        if (currentIndex < (dialogueArray.Length))
        {
            StaticProperties.dialogueGUI.ChangeGUI(dialogueArray[currentIndex]);

            if (dialogueArray[currentIndex].skipButton)
            {
                StaticProperties.PausedByDialogue = true;
                StaticMethods.FreezeGame();
            }
            else
            {
                StaticProperties.PausedByDialogue = false;
                StaticMethods.UnfreezeGame();
            }
        }
        else
        {
            PushNullDialogue();
        }
    }

    void PushNullDialogue()
    {
        StaticProperties.dialogueGUI.ChangeGUI(StaticProperties.nullDialogue);
        StaticProperties.PausedByDialogue = false;
        StaticMethods.UnfreezeGame();
    }

    IEnumerator WaitAndStartDialogue(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        currentIndex = 0;
        PushCurrentDialogue();
    }
}
