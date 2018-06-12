using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameObjectRider
    {
        GameObject gameObject;
        public GameObjectRider()
        {
            gameObject = new GameObject();
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
       
    }
}

