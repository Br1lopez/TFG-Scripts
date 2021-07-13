using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Tilemaps
{

    public class pruebasprites : MonoBehaviour
    {

        public Sprite sprite1;
        public Object[] Sprites;
        public Sprite[] SpriteArray;
        public int numSprites = 0;
        public int numOtros = 0;
        public string Path = "grav/up";


        // Start is called before the first frame update
        void Start()
        {

            Sprites = Resources.LoadAll("grav/up");
            SpriteArray = new Sprite[Sprites.Length];

            foreach (Object i in Sprites)
            {
                System.Type tipo = i.GetType();

                if (tipo == typeof(Sprite))
                {
                    SpriteArray[numSprites] = i as Sprite;
                    numSprites++;
                }
                else
                {
                    numOtros++;
                }

                //Debug.Log(numSprites);

            }
            //print("Objetos: " + Sprites.Length);
            //print("Sprites" + SpriteArray.Length);
           


            //Sprites = Resources.LoadAll("grav/up", typeof(Sprite));
            sprite1 = SpriteArray[2];

        }


        
    }



}