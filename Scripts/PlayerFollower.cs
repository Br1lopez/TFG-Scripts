using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



public class PlayerFollower : MonoBehaviour
{
    public int index;
    bool IsFirstRun;
    bool TransitionFirstRun = true;
    float[] PerfWeight_Array = new float[8];
    float[] m_Weight_Array = new float[8];
 

    public float NormalMultiplier;
    public float NormalPower;


    public float LinearThreshold;
    public float ThresholdMultiplier;

    public float StopThreshold;
    
    float VariableDamping;

    int MyCamArrayLength;
    int NextCamIndex;

    int RunCounter = 0;

    float StartTime;

    float NextLevelWeight;

    [SerializeField] CinemachineMixingCamera MixingCamera;
    [SerializeField] CinemachineVirtualCamera FalseCam;

    void Start()
    {
        IsFirstRun = true;
        GetMyCamArrayLength();

    }

    void FixedUpdate()
    {
        //Solo se ejecuta si en Tranisitioner esta instancia es target.

            //Este objeto sigue al jugador
            if (GameClass.player != null)
            {
                gameObject.transform.position = GameClass.player.transform.position;
            }

            /*GameObject.FindWithTag("VCAM").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = GameObject.FindWithTag("FalseCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
            print(GameObject.FindWithTag("FalseCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition);*/
            CombinedCamera();
        

    }


    void GetMyCamArrayLength()
    {
        MyCamArrayLength = 0;
        int i = 0;
        
        //Cantidad de cámaras a tener en cuenta. Las llamadas NextCam van aparte.
        foreach (Transform t in MixingCamera.gameObject.transform)
        {
            if (t.gameObject.name != "NextCam")
            {
                MyCamArrayLength++;
            }
            else
            {
                NextCamIndex = i;
                MixingCamera.SetWeight(i,0);
            }

            
            if (i == 0)
            {
                MixingCamera.SetWeight(i, 1);
            }
            else
            {
                MixingCamera.SetWeight(i, 0);
            }
            
            i++;
        }
    }

    void CombinedCamera()
    {        
        float path = FalseCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;

        if (Transitioner.IsTransitionHappening)
        {
            if (Transitioner.TransitionFirstRun)
            {
                StartTime = Time.time;
                NextLevelWeight = 0;
                Transitioner.TransitionFirstRun = false;
            }

            //Mientras la NextCam interpola de 0 a 1...
            //NextLevelWeight = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, (Time.time - StartTime)/Transitioner.TransitionDuration)));
            AssignWeightsWithDamping(NextLevelWeight, NextCamIndex);

            for(int i=0; i<MyCamArrayLength; i++)
            {
                //...el resto de cámaras interpolan de su valor a 0 (al mismo ritmo).
                float w = MixingCamera.GetWeight(i);
                float thisWeight = Mathf.SmoothStep(w, 0, (Mathf.SmoothStep(w, 0, (Time.time - StartTime) / Transitioner.TransitionDuration)));                
                AssignWeightsWithDamping(thisWeight, i);
            }
            
            if ((Time.time - StartTime)> Transitioner.TransitionDuration)
            {
                Transitioner.IsTransitionHappening = false;
                Transitioner.TransitionJustFinished = true;
                Transitioner.TransitionFirstRun = true;
            }
            //print(TransitionCurve.Velocity);


        }
        else if (!Transitioner.TransitionTriggered){
            // Este bucle reparte los pesos ideales según el segmento del path en el que esté el jugador. 
            for (int i = 0; i < MyCamArrayLength; i++)
            {

                //i=0->j=1, i=1->j=3, i=2->j=5, i=3->j=7, etc.
                int j = ((i + 1) * 2) - 1;

                //Segmento anterior, tiende a 1 según se acerca.
                if ((j - 2) < path && path < (j - 1))
                {
                    PerfWeight_Array[i] = path - Mathf.Floor(path);
                }
                //Segmento del peso, es 1.
                else if ((j - 1) <= path && path <= j)
                {
                    PerfWeight_Array[i] = 1;

                }
                //Segmento siguiente, tiende a 0 según se aleja. 
                else if (j < path && path < (j + 1))
                {
                    PerfWeight_Array[i] = 1 - (path - Mathf.Floor(path));
                }
                //Resto de casos es 0.
                else
                {
                    PerfWeight_Array[i] = 0;
                }

            }


            //Calculamos pesos a partir de los ideales:
            if (!IsFirstRun)
            {
                for (int i = 0; i < MyCamArrayLength; i++)
                {

                    AssignWeightsWithDamping(PerfWeight_Array[i], StaticProperties.CameraDamping, i);
                }

            }
            else
            {
                for (int i = 0; i < MyCamArrayLength; i++)
                {

                    AssignWeightsWithDamping(PerfWeight_Array[i], i);
                }

                if (RunCounter >= 2)
                {
                    IsFirstRun = false;
                }
                if (RunCounter < 2)
                {
                    RunCounter++;
                }
            }
        }
    }

    //Utiliza el index para saber a quién referenciar.
    void AssignWeightsWithDamping(float PesoOrigen, float damping, int IndexPeso)
    {
        switch (IndexPeso)
        {
            case 0:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight0);
                break;
            case 1:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight1);
                break;
            case 2:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight2);
                break;
            case 3:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight3);
                break;
            case 4:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight4);
                break;
            case 5:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight5);
                break;
            case 6:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight6);
                break;
            case 7:
                AssignWeightsViaReference(PesoOrigen, damping, ref MixingCamera.m_Weight7);
                break;
            default:
                print("Casos insuficientes en la función AssignWeightsWithDamping en PlayerFollower.cs");
                break;
        }
    }

    //Lo mismo que su tocayo, pero sin damping.
    void AssignWeightsWithDamping(float PesoOrigen, int IndexPeso)
    {
        switch (IndexPeso)
        {
            case 0:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight0);
                break;
            case 1:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight1);
                break;
            case 2:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight2);
                break;
            case 3:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight3);
                break;
            case 4:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight4);
                break;
            case 5:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight5);
                break;
            case 6:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight6);
                break;
            case 7:
                AssignWeightsViaReference(PesoOrigen,  ref MixingCamera.m_Weight7);
                break;
            default:
                print("Casos insuficientes en la función AssignWeightsWithDamping en PlayerFollower.cs");
                break;
        }
        //print("Peso numero: " + IndexPeso + "  Valor: " + PesoOrigen);
    }

    //Asigna los pesos.
    void AssignWeightsViaReference(float _pesoOrigen, float _damping, ref float _pesoDestino)
    {       
            //La variable damping, pero dependiendo de la diferencia entre peso ideal y peso real.
            VariableDamping = NormalMultiplier * Mathf.Pow(Mathf.Abs(_pesoDestino - _pesoOrigen), NormalPower);

            if (Mathf.Abs(_pesoDestino - _pesoOrigen) < LinearThreshold)
            {
                VariableDamping = ThresholdMultiplier * Mathf.Pow(LinearThreshold, NormalPower);
            }

            //print("dif: " + (PesoDestino - PesoOrigen)+"    var: "+ VariableDamping);
            if ((Mathf.Abs(_pesoDestino - _pesoOrigen) <= StopThreshold) || (Mathf.Abs(_pesoDestino - _pesoOrigen) <= VariableDamping))
            {
                _pesoDestino = _pesoOrigen;
            }
            else


            //Si el peso ideal es menor, restar. Se resta más rapido cuanta más diferencia haya.
            if (_pesoOrigen < _pesoDestino)
            {
                _pesoDestino -= VariableDamping;

                if (_pesoOrigen > _pesoDestino)
                {
                    _pesoDestino = _pesoOrigen;
                }


            }

            //Si el peso ideal es mayor, sumar. Se suma más rapido cuanta más diferencia haya.
            else if (_pesoOrigen > _pesoDestino)
            {
                _pesoDestino += VariableDamping;

                if (_pesoOrigen < _pesoDestino)
                {
                    _pesoDestino = _pesoOrigen;
                }
            }
        
    }
    
    //Lo mismo que su tocayo, pero sin damping.
    void AssignWeightsViaReference(float _pesoOrigen, ref float _pesoDestino)
    {
        _pesoDestino = _pesoOrigen;       
    }

}
