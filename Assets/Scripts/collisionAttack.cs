using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionAttack : MonoBehaviour
{
    public float SoulsAwarded;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player") {
            GameManager.instance.Souls.GetComponent<souls>().healthAdjusted(SoulsAwarded);
        }
    }
}
