using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    
    public StoneProdigious BossScript;
    public List<GameObject> wires = new List<GameObject>();
    public List<float> RemoveWireAtHealth = new List<float>();
        // Conditions:
        // must be in decending order e.g. 180 140 100 not 40 70 120 170
        // and this[].len == wires.count
        // and this shouldnt have small amounts between each element

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("weapon"))
        {
            float damageTaken = Random.Range(3, 10);
            if(SaveManager.instance.activeSave.XTREME) {
                damageTaken *= 0.8f;
            }
            BossScript.bossDamaged(damageTaken);

            float bossHealth = BossScript.health;

            if(RemoveWireAtHealth[0] > bossHealth) 
            {
                Destroy(wires[0]);
                wires.RemoveAt(0);
                RemoveWireAtHealth.RemoveAt(0);
            }

        }
    }
}
