using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : FallingSloth.SingletonBehaviour<GameManager>
{


    void Update()
    {
        if (Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {

                }
            }
        }
    }

    bool CastRay(Touch touch)
    {

    }
}