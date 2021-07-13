using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : GameClass
{
    // Desde aquí sólo se maneja el Trigger de la transición y el nuevo Spawn.
    // El resto se controla desde Transitioner.cs
    public bool ChangeByCut = false;
    public SceneReference EscenaActual;
    public SceneReference EscenaSiguiente;


    void Start()
    {
        StaticProperties.CurrentScene = EscenaActual;
    }

    public void DisplayLevel(SceneReference scene)
    {
        GUILayout.Label(new GUIContent("Scene name Path: " + scene));
        if (GUILayout.Button("Load " + scene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }

    void Update()
    {

        if (Transitioner.TransitionJustFinished)
        {
            Transitioner.TransitionJustFinished = false;
            Transitioner.OnTransitionFinished(EscenaActual);                        

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Cuando toquen este punto:
        if (other.gameObject.tag == "Player" && !Transitioner.IsTransitionHappening && !Transitioner.TransitionTriggered)        {
            AudioManager.instance.Play("CP");
            StaticProperties.cortinilla.On();

            StartCoroutine(WaitAndCut());
        }

    }

    IEnumerator WaitAndCut()
    {
        yield return new WaitForSecondsRealtime(1.6f);
        //El resto del código ocurre en Transitioner.cs:
        if (!ChangeByCut)
        {
            Transitioner.OnTransitionStart(EscenaSiguiente);
        }
        else
        {
            Transitioner.OnCut(EscenaSiguiente);
        }
    }



    
}
