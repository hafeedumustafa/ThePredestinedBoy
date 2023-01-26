using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDeathByFall : MonoBehaviour
{
    public float newDeathY;
    public float oldDeathY;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) {
            collider.gameObject.GetComponent<PlayerManagerFV>().deathY = newDeathY;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Player")) {
            collider.gameObject.GetComponent<PlayerManagerFV>().deathY = oldDeathY;
        }
    }
}
