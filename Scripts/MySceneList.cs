using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneList : MonoBehaviour
{
    public SceneReference[]misEscenas;


    private void OnGUI()
    {
    for(int i = 0; i < misEscenas.Length; i++)
        {
            DisplayLevel(misEscenas[i]);
        }
    }

    public void DisplayLevel(SceneReference scene)
    {
        GUILayout.Label(new GUIContent("Scene name Path: " + scene));
        if (GUILayout.Button("Load " + scene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }


}
