using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFied : ActivableObject
{
    
    
    [SerializeField] public GameClass.Direction GravFieldValue;
    bool tempColision;
    bool tempActive;
    bool _fieldEnabled;
    bool fieldEnabled
    {        
        get { return _fieldEnabled; }
        set {
            if (value == true)
            {
                StaticProperties.GravityFieldsWorking++;
                StaticProperties.activeGravField = this;
            }
            else {
                StaticProperties.GravityFieldsWorking--;
            }

            _fieldEnabled = value;
            }
    }
    bool anyTargetCollidingThis
    {
        get
        {
            foreach(Collider2D c in _colArray)
            {
                if (c != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
    bool active
    {
        get
        {
            if (StaticProperties.CurrentLayerIndex == gameObject.layer&&ButtonActivated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
    ContactFilter2D cf = new ContactFilter2D();
    Collider2D[] _colArray = new Collider2D[512];
    int target_array_counter;
    Movement[] target_array = new Movement[64];
    GameClass.Direction CurrentGravDir;

    void RotateTargets()
    {
        foreach (Movement m in target_array)
        {
            if (m != null)
            {
                m.ActualizarGiro();
            }
        }
    }

    void RotateControls()
    {

    }

    private void Start()
    {
        UpdateTempBools();

        StaticProperties.AreControlsHorizontal = true; 
        if (anyTargetCollidingThis && active)
            GravityChange(true);     

    }

    private void FixedUpdate()
    {
        GetObjectArray();
        if (tempColision != anyTargetCollidingThis|| tempActive != active)
            //Cuando hay algun cambio en el estado de la colision o del activo o está en cola:
        {            
            if (anyTargetCollidingThis&&active)
                //Si está en colisión y activado
            {
                print(active);
            GravityChange(true);

            }
            else if (fieldEnabled)
                //Si no:
            {
                GravityChange(false);
            }
        }

        UpdateTempBools();
    }

    void GravityChange(bool isEnabled)
    {
        if (isEnabled&&StaticProperties.GravityFieldsWorking > 0)
            //Si ya hay algún campo activado
        {
            StaticProperties.activeGravField.fieldEnabled = false;            //Desactivarlo sin inercia (ya se aplica aquí)

        }
        //Se crea una inercia (que luego se disipará) de igual fuerza y dirección que la anterior gravedad.
        //La gravedad se pone a 0 y se ajusta a la nueva dirección.
        CurrentGravDir = (isEnabled) ? GravFieldValue : GameClass.Direction.Down;

        foreach (Movement m in target_array)
        {
            if (m != null)
            {
                m.InerciaGrav_fuerza = m.grav_fuerza;
                m.grav_fuerza = 0;
                m.InerciaGrav_dir = StaticProperties.grav_dir_vector;
            }
        }
        StaticProperties.grav_dir = CurrentGravDir;

        //Se gira al jugador:
        RotateTargets();

        //Se activan/desactivan los controles horizontales:
        StaticProperties.AreControlsHorizontal = StaticMethods.IsVectorVertical(StaticMethods.Dir2Vec(CurrentGravDir));

        //Se marca como activo/inactivo el campo gravitatorio:
        fieldEnabled = isEnabled;
    }

    void UpdateTempBools()
    {
        tempColision = anyTargetCollidingThis;
        tempActive = active;
    }
    public float ChildrenSpeed
    {
        get{
            foreach (Transform child in gameObject.transform) {
                if (child.gameObject.GetComponent<Animator>() != null)
                {
                    return child.gameObject.GetComponent<Animator>().speed;

                }

                foreach (Transform c in child)
                {
                    if (c.gameObject.GetComponent<Animator>() != null)
                    {
                        return c.gameObject.GetComponent<Animator>().speed;
                    }
                }
            }
            return 1;
        }

        set
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.GetComponent<Animator>() != null)
                {
                    child.gameObject.GetComponent<Animator>().speed = value;

                }

                foreach (Transform c in child)
                {
                    if (c.gameObject.GetComponent<Animator>() != null)
                    {
                        c.gameObject.GetComponent<Animator>().speed = value;
                    }
                }
            }           
        }

    }

    private void GetObjectArray()
    {
        target_array_counter = 0;
        System.Array.Clear(_colArray, 0, 16);
        System.Array.Clear(target_array, 0, 16);
        gameObject.GetComponent<Collider2D>().OverlapCollider(cf, _colArray);

        int aaa = 0;
        foreach (Collider2D c in _colArray){
            
            if (c != null)
            {
                aaa++;
            }

            if (c!=null&&c.gameObject.HasComponent<Movement>())
            {
                target_array[target_array_counter] = c.gameObject.GetComponent<Movement>();
                target_array_counter++;
            }
        }

        print(target_array_counter + " " + aaa);
    }    
}
