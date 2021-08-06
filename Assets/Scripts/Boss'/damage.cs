using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    
    public StoneProdigious BossScript;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "weapon")
        {
            float damageTaken = Random.Range(10, 30);
            BossScript.health -= (int) damageTaken;
            print(BossScript.health);
            if(BossScript.health < 1) {
                BossScript.health = 0;
                BossScript.EndGame();
            }
        }
    }
}
