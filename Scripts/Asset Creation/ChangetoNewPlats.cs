using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangetoNewPlats : MonoBehaviour
{
    public Sprite _sprite;
    public Material _material;
    public bool _finished = true;
    public GameClass.ColorEnum affectedColor = GameClass.ColorEnum.None;
    public bool changeMaterial;
    public bool changeSprite;
    public bool changeOthers;
    SpriteRenderer sr;
    public bool cambioDeTamañoEnElRojo;  

    void Update()
    {

        if (!_finished)
        {
            if (affectedColor == GameClass.ColorEnum.Main)
            {
                foreach (Transform t in transform)
                {
                    if (t.gameObject.name.Contains("plat_main"))
                    {
                        t.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x, 0.2f);
                        t.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.05f);

                        foreach (Transform t_child in t.transform)
                        {
                            if (t_child.name == "C")
                            {
                                sr = t_child.gameObject.GetComponent<SpriteRenderer>();
                                if (changeOthers)
                                {
                                    t_child.localScale = new Vector2(1, 1);
                                    t_child.localPosition = new Vector2(0, -0.1f);                                    
                                    sr.drawMode = SpriteDrawMode.Tiled;
                                    sr.tileMode = SpriteTileMode.Adaptive;
                                    sr.size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x + 0.1f, 0.4f);
                                    sr.sortingLayerName = "Plataformas";
                                }
                                if (changeSprite)
                                {
                                    sr.sprite = _sprite;
                                }
                                if (changeMaterial)
                                {
                                    sr.material = _material;
                                }
                                
                            }
                            else if (t_child.name == "L" || t_child.name == "R")
                            {
                                t_child.gameObject.SetActive(false);
                            }
                        }


                    }

                    if (t.gameObject.name.Contains("pinchos_main"))
                    {
                        foreach(Transform t2 in t)
                        {
                            foreach(Transform t3 in t2)
                            {
                                t3.gameObject.GetComponent<SpriteRenderer>().material = _material;
                            }
                        }
                    }

                    }
            }

            if (affectedColor == GameClass.ColorEnum.Red)
            {
                foreach (Transform t in transform)
                {
                    if (t.gameObject.name.Contains("plat_red"))
                    {
                        t.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x, 0.2f);
                        t.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.05f);
                        foreach (Transform t2 in t)
                        {
                            if (t2.gameObject.name == "ON")
                            {
                                foreach (Transform t_child in t2.transform)
                                {
                                    if (t_child.name == "C")

                                    {
                                        t_child.localScale = new Vector2(1, 1);
                                        t_child.localPosition = new Vector2(0, -0.05f);
                                        SpriteRenderer sr = t_child.gameObject.GetComponent<SpriteRenderer>();
                                        sr.drawMode = SpriteDrawMode.Tiled;
                                        sr.tileMode = SpriteTileMode.Adaptive;
                                        sr.size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x + 0.1f, 0.3f);
                                        sr.sprite = _sprite;
                                        sr.material = _material;
                                        sr.sortingLayerName = "Plataformas";

                                    }
                                    else if (t_child.name == "L" || t_child.name == "R")
                                    {
                                        t_child.gameObject.SetActive(false);
                                    }
                                }
                            }

                            if (t2.gameObject.name == "OFF")
                            {
                                t2.localPosition = new Vector2(0, -0.05f);

                                if (cambioDeTamañoEnElRojo)
                                {
                                    foreach (Transform t_child in t2.transform)
                                    {
                                        if (t_child.name == "C")

                                        {
                                            Vector2 newScale = new Vector2(1, 1);
                                            newScale.x = t_child.localScale.x - 1;
                                            t_child.localScale = newScale;
                                        }
                                        else if (t_child.name == "L" || t_child.name == "R")
                                        {
                                            Vector2 newPos = t_child.localPosition;
                                            newPos.x -= (Mathf.Sign(newPos.x) * 0.05f);
                                            t_child.localPosition = newPos;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (affectedColor == GameClass.ColorEnum.Blue)
            {
                foreach (Transform t in transform)
                {
                    if (t.gameObject.name.Contains("plat_blue"))
                    {
                        t.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x, 0.2f);
                        t.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.05f);
                        foreach (Transform t2 in t)
                        {
                            if (t2.gameObject.name == "ON")
                            {
                                foreach (Transform t_child in t2.transform)
                                {
                                    if (t_child.name == "C")

                                    {
                                        t_child.localScale = new Vector2(1, 1);
                                        t_child.localPosition = new Vector2(0, -0.05f);
                                        SpriteRenderer sr = t_child.gameObject.GetComponent<SpriteRenderer>();
                                        sr.drawMode = SpriteDrawMode.Tiled;
                                        sr.tileMode = SpriteTileMode.Adaptive;
                                        sr.size = new Vector2(t.gameObject.GetComponent<BoxCollider2D>().size.x + 0.1f, 0.3f);
                                        sr.sprite = _sprite;
                                        sr.material = _material;
                                        sr.sortingLayerName = "Plataformas";
                                    }
                                    else if (t_child.name == "L" || t_child.name == "R")
                                    {
                                        t_child.gameObject.SetActive(false);
                                    }
                                }
                            }

                            if (t2.gameObject.name == "OFF")
                            {
                                t2.localPosition = new Vector2(0, -0.05f);
                                if (cambioDeTamañoEnElRojo)
                                {
                                    foreach (Transform t_child in t2.transform)
                                    {
                                        if (t_child.name == "C")

                                        {
                                            Vector2 newScale = new Vector2(1, 1);
                                            newScale.x = t_child.localScale.x - 1;
                                            t_child.localScale = newScale;
                                        }
                                        else if (t_child.name == "L" || t_child.name == "R")
                                        {
                                            Vector2 newPos = t_child.localPosition;
                                            newPos.x -= (Mathf.Sign(newPos.x) * 0.05f);
                                            t_child.localPosition = newPos;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            _finished = true;
        }
    }

    
}
