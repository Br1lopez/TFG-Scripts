using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersOnDialogueLine : Orders
{
    public DialogueLine dialogueLine;
    bool usable = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (StaticProperties.dialogueGUI.currentDialogueLine == dialogueLine && usable)
        {
            RunOrders();
            usable = false;
        }
    }
}
