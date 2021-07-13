using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Transitioner
{


    static public bool TransitionTriggered = false;
    static public bool IsTransitionHappening = false;
    static public bool TransitionFirstRun = true;

    static public float TransitionDuration = 2f;

    static public bool TransitionJustFinished = false;

    static public bool NewSpawn = false;

    public static void OnCut(SceneReference SceneToLoad)
    {
        //Para que la música reaparezca:
        StaticProperties.MusicSceneLoaded = false;

        //Este bool se vuelve true para que el jugador reaparezca
        StaticProperties.BaseAlreadyExists = false;

        //Se define nuevo Spawn para SceneConfig
        NewSpawn = true;

        //Se carga la nueva escena en modo NO aditivo
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        
    }

    public static void OnTransitionStart(SceneReference SceneToLoad)
    {

        //Se define nuevo Spawn para SceneConfig
        NewSpawn = true;

        //Se carga la nueva escena
        SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);

        //Se bloquea al jugador
        //GameClass.BlockPlayer();

        StaticCoroutine.DoCoroutine(Esperar(TransitionDuration));

    }

    public static void OnTransitionFinished(SceneReference SceneToUnload)
    {
        //Se cambia el target script:      

        //GameClass.UnblockPlayer();  
        SceneManager.UnloadSceneAsync(SceneToUnload);
        //Resources.UnloadUnusedAssets();
        TransitionTriggered = false;
        IsTransitionHappening = false;
    }

    static IEnumerator Esperar(float f)
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        yield return new WaitForSecondsRealtime(f);
        //Para que ocurra solo una vez:
        TransitionTriggered = true;

        //Comienza la transición (en PlayerFollower.cs)
        IsTransitionHappening = true;

    }
}


