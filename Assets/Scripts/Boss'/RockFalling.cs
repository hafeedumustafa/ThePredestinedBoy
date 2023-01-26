using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFalling : MonoBehaviour
{

    public GameObject Rock;
    public GameObject Explosion;
    public StoneProdigious Boss;

    void FixedUpdate()
    {
        gameObject.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, 3f));
        if(Boss.endAnims)
            Destroy(this.gameObject);
    }

    public void OnCollisionStay2D(Collision2D collider) {
        if(collider.gameObject.CompareTag("Player"))
        {
            //Player Decrease Health
            
            GameManager.instance.Souls.GetComponent<souls>().healthAdjusted(-1 * Boss.DamageMultiplier);
            SpawnParticles();
            Destroy(Rock);
        }
        
        if(collider.gameObject.layer == 8)
        {
            SpawnParticles();
            Destroy(Rock);
        }
    }

    private void SpawnParticles()
    {
        GameObject particles = Instantiate(Explosion, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
    }
}
