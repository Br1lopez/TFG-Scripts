using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : ActivationObject
{
    public Compuerta target;
    bool pressable = false;
    enum AnimState { left, goingleft, right, goingright}
    AnimState _animstate = AnimState.right;
    public Transform spr_palanca;
    float anglesRotated = 0;

    private void Start()
    {
        target.palanca = this;
        StaticProperties.controles.Interact.Interact.started += ctx => Press();
    }

    private void Update()
    {
        if (spr_palanca != null)
        {
            switch (_animstate)
            {
                case AnimState.goingleft:
                    if (anglesRotated<90)
                    {
                        spr_palanca.Rotate(0, 0, 180 * Time.deltaTime);
                        anglesRotated += 180 * Time.deltaTime;
                    }
                    else
                    {
                        _animstate = AnimState.left;
                    }
                    break;
                case AnimState.goingright:
                    if (anglesRotated > 0)
                    {
                        spr_palanca.Rotate(0, 0, -180 * Time.deltaTime);
                        anglesRotated -= 180 * Time.deltaTime;
                    }
                    else
                    {
                        _animstate = AnimState.right;   
                    }
                    break;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.HasComponent<PlayerAuto>())
        {
            pressable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.HasComponent<PlayerAuto>())
        {
            pressable = false;
        }
    }

    void Press()
    {
        if (pressable && !target.moving)
        {
            target.NextTarget();

            switch (_animstate)
            {
                case AnimState.left:
                    _animstate = AnimState.goingright;
                    break;
                case AnimState.right:
                    _animstate = AnimState.goingleft;
                    break;
                case AnimState.goingleft:
                    _animstate = AnimState.goingright;
                    break;
                case AnimState.goingright:
                    _animstate = AnimState.goingleft;
                    break;
            }
            
        }
    }

    private void OnDestroy()
    {
        StaticProperties.controles.Interact.Interact.started -= ctx => Press();
    }
}
