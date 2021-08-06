using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    void Update()
    {
        if(gameObject.GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
