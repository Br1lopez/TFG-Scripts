using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Collider2D[] colPiesArray = new Collider2D[32];
    public Collider2D[] colPlayerArray = new Collider2D[32];
    ContactFilter2D cf = new ContactFilter2D();

    public LayerMask lm = new LayerMask();
    public LayerMask _lm_r = new LayerMask();
    public LayerMask _lm_g = new LayerMask();
    public LayerMask _lm_b = new LayerMask();
    public LayerMask _lm_main = new LayerMask();

    public Collider2D colPies = GameClass.colPies;
    public Collider2D player_col = GameClass.player_col;

    bool groundResult;
    public bool groundCollision
    {
        get
        {
            groundResult = false;
            foreach (Collider2D col in colPiesArray)
            {
                if (col != null && col.gameObject.tag == "ground")
                {
                    groundResult = true;
                }
            }
            return groundResult;
        }        
    }

    public bool movingPlatCollision
    {
        get
        {
            bool result = false;
            foreach (Collider2D col in colPiesArray)
            {
                if (col != null && col.gameObject.HasComponent<MovingPlatform>())
                {
                    collidingMovPlat = col.gameObject.GetComponent<MovingPlatform>();
                    result = true;
                }
            }
            return result;
        }
    }

    public bool GravFieldCollision
    {
        get
        {
            bool result = false;
            foreach (Collider2D col in colPlayerArray)
            {
                if (col != null && col.gameObject.HasComponent<GravityFied>())
                {
                    result = true;
                }
            }
            return result;
        }
    }

    public bool ObstaculoCollision
    {
        get
        {
            bool result = false;
            foreach (Collider2D col in colPiesArray)
            {
                if (col != null && col.gameObject.HasComponent<Obstaculo>())
                {
                    result = true;
                    return result;
                }
            }
            foreach (Collider2D col in colPlayerArray)
            {
                if (col != null && col.gameObject.HasComponent<Obstaculo>())
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }
    }


    public MovingPlatform collidingMovPlat;


    Collider2D[] _colArray = new Collider2D[16];
    public Collider2D[] GetColArray(Collider2D collider)
    {
        System.Array.Clear(_colArray, 0, 16);
            collider.OverlapCollider(cf, _colArray);
            return _colArray;
    }

    
    void GetContactFilter()
    {
        if (StaticProperties.red.Value)
        {
            lm = _lm_r;
        }
        else if (StaticProperties.green.Value)
        {
            lm = _lm_g;
        }
        else if (StaticProperties.blue.Value)
        {
            lm = _lm_b;
        }
        else
        {
            lm = _lm_main;
        }

        cf.SetLayerMask(lm);
        cf.useTriggers = true;
    }

    void GetCollisionArrays()
    {
        System.Array.Clear(colPiesArray, 0, 16);
        colPies.OverlapCollider(cf, colPiesArray);
        System.Array.Clear(colPlayerArray, 0, 16);
        player_col.OverlapCollider(cf, colPlayerArray);
    }

    public void FixedUpdate()
    {
        GetContactFilter();
        GetCollisionArrays();
    }


    private void Start()
    {
        StaticMethods.SetDefaultInterpolation(colPies.gameObject);
        //Se setean 3 layermask ahora para evitar localizaciones de memoria:
        _lm_r = LayerMask.GetMask("BLACK_ground", "RED_ground");
        _lm_g = LayerMask.GetMask("BLACK_ground", "GREEN_ground");
        _lm_b = LayerMask.GetMask("BLACK_ground", "BLUE_ground");
        _lm_main = LayerMask.GetMask("BLACK_ground");
    }
}
