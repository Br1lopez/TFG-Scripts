using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActivateThroughButton : MonoBehaviour
{
    public GameObject sourceGameObject;
    public GameObject[] targetGameObjects;
    bool tempButtonValue;

    GameButton SourceScript
    {
        get
        {
            GameObject _go = sourceGameObject;

            if (_go.GetComponent<GameButton>() != null)
            {

                return _go.GetComponent<GameButton>();
            }
            else
            {
                return null;
            }
        }
    }
    ActivableObject TargetScript(GameObject _go)
    {           

            if (_go.GetComponent<ActivableObject>() != null)
            {
                return _go.GetComponent<ActivableObject>();
            }else
            {
                return null;
            }
        
    }

    private void Start()
    {
        foreach (GameObject g in targetGameObjects)
        {
            TargetScript(g).ButtonActivated = SourceScript.pressed;
        }
        tempButtonValue = SourceScript.pressed;
    }

    void Update()
    {
        if (SourceScript.pressed != tempButtonValue)
        {
            foreach (GameObject g in targetGameObjects)
            {
                TargetScript(g).ButtonActivated = SourceScript.pressed;
            }            
            tempButtonValue = SourceScript.pressed;
        }
    }
}

