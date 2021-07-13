using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConfig : MonoBehaviour
{
    public SceneReference MusicScene;

//También Es el Spawn Inicial
    void OnEnable()
    {
        StaticProperties.shooting = true;
        StaticProperties.blocked = false;
        //Si no existe base
        if (!StaticProperties.BaseAlreadyExists)
        {
            //Carga la escena Base con controles y jugador
            SceneManager.LoadScene("BASE_SCENE_3", LoadSceneMode.Additive);


            //Marcar que ahora existe base
            StaticProperties.BaseAlreadyExists = true;
        }


        //Si hay configurada una escena con la música...
        if (MusicScene!= null && !StaticProperties.MusicSceneLoaded)
        {
            //...cargarla
            SceneManager.LoadScene(MusicScene, LoadSceneMode.Additive);

            //Marcar que está cargada
            StaticProperties.MusicSceneLoaded = true;
            
        }
        
    }

    private void Start()
    {
        //Nuevo Spawn del Jugadoren caso de no haberlo o de transición por corte
        if (StaticProperties.SpawnPos == Vector3.zero || Transitioner.NewSpawn)
        {
            StaticProperties.SpawnPos = gameObject.transform.position;
            Transitioner.NewSpawn = false;
        }

        //Colocar al jugador en la posición de Spawn:
        if (GameClass.player != null)
        {
            GameClass.player.transform.localPosition = StaticProperties.SpawnPos;
        }

        StaticProperties.grav_dir = GameClass.Direction.Down;
    }
}