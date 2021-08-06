using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingFog : MonoBehaviour
{
    
    public float _speed = 1;

    

    void FixedUpdate()
    {
        transform.Translate(_speed * Time.fixedDeltaTime, 0, 0);
    }
}
