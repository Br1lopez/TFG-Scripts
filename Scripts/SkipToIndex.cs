using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipToIndex : MonoBehaviour
{
    public SceneReference JumpTo;
    private void Awake()
    {
        SceneManager.LoadScene(JumpTo, LoadSceneMode.Single);
    }
}
