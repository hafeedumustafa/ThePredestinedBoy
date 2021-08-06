using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffectOnBackground : MonoBehaviour
{
    
    float length, startPosition;
    public float ParralaxEffect;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().size.x;
    }

    void Update()//camera
    {
        float temp = (Camera.main.transform.position.x * (1 - ParralaxEffect));
        float distance = (Camera.main.transform.position.x * ParralaxEffect);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);


        if (temp > startPosition + length) {startPosition = startPosition + length * 2;}
        else if (temp < startPosition - length) {startPosition -= length * 2;}
        

    }
}
