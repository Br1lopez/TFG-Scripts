using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTorreta : MonoBehaviour
{
    [SerializeField] float Arco; //Ángulo en el que se moverá el cañón (a definir en el inspector de Unity)
    [SerializeField] float VelGiro; //Velocidad de giro del cañón (a definir en el inspector de Unity)
    public Rigidbody2D rb; //Se utiliza para controlar los movimientos y colisiones de un objeto en Unity (se enlaza en el inspector de Unity)
    public GameObject bala_obj; 
    [SerializeField] float velBala; //Velocidad de las balas que dispara (a definir en el inspector de Unity)
    [SerializeField] float alcanceBala; //Alcance de las balas que dispara (a definir en el inspector de Unity)
    private float AnguloActual;
    private bool CounterClock; //Verdadero si gira contrarreloj
    private float angle_offset; //Marca el ángulo inicial del cañón
    float f = 0;
    public float shootInterval; //Cadencia, intervalo de disparo (a definir en el inspector de Unity)
    public Transform salidaBala; //Lugar de salida de la bala (a definir en el inspector de Unity)
    public AudioSource audioSource; //Sonido de disparo (se enlaza en el inspector de Unity)

    //Start() se ejecuta una vez antes de dibujar el primer fotograma
    void Start()   {
        AnguloActual = 0;
        CounterClock = true;
        angle_offset = gameObject.transform.parent.transform.rotation.eulerAngles.z; //La rotación del objeto en Unity es declarada como ángulo inicial.
        shootInterval= shootInterval * 60; //Convierte el intervalo de disparo de segundos a fotogamas
    }

    //FixedUpdate() se ejecuta de manera uniforme y constante 60 veces por segundo
    void FixedUpdate()
    {
        CiclodeGiro(); //Se calcula el ángulo del cañón en este frame
        rb.rotation = AnguloActual + angle_offset; //Se dibuja el cañón en el ángulo correspondiente.

        //Disparar() se ejecuta una vez cada intervalo de disparo.
        if (f < shootInterval)
        {
            f++;
        }
        else
        {
            Disparar();
            f = 0;
        }
    }


    //CiclodeGiro() mueve el angulo del cañón en cada fotograma
    void CiclodeGiro()
    {
        if (CounterClock)
        {
            if (AnguloActual < Arco / 2) {
                AnguloActual += VelGiro/60;
            }else{
                CounterClock = false;
            }
        }
        else
        {
            if (AnguloActual > -Arco / 2)
            {
                AnguloActual -= VelGiro/60;
            }
            else
            {
                CounterClock = true;
            }

        }
    }

    //Disparar() reproduce un sonido y crea una instancia de una bala con ciertas propiedas (dirección, posición inicial, alcance y velocidad)
    void Disparar()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        var newobject = Instantiate(bala_obj);
        newobject.GetComponent<Bala>().angle = rb.rotation;
        newobject.GetComponent<Bala>().rb.position = salidaBala.position;
        newobject.GetComponent<Bala>().alcance = alcanceBala;
        newobject.GetComponent<Bala>().vel = velBala;
    }
}
