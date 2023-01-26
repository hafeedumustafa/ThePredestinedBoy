using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
        try{GameManager.instance.deathScreen = this.gameObject;} catch{}
        gameObject.SetActive(false);
    }
}
