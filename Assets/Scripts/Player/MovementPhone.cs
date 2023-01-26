using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPhone : MonoBehaviour
{

    public PlayerManagerFV playerManagerFV;

    public void click()
    {
        if (playerManagerFV.m_isGrounded) {
            playerManagerFV.isJumping = true;
            playerManagerFV.rb.velocity = Vector2.up * playerManagerFV.JumpForce;
            StartCoroutine(Jump());
        }
    }

    public void unclick()
    {
        StopAllCoroutines();

        playerManagerFV.animator.SetBool("hitSpace", false);
        playerManagerFV.isJumping = false;
        playerManagerFV.JumpTimer = playerManagerFV.OriginalJumpTimer;
        playerManagerFV.JumpForce = playerManagerFV.OriginalJumpForce;
    } 

    IEnumerator Jump()
    {

        if (playerManagerFV.isJumping) {
            if(playerManagerFV.JumpTimer > 0)
            {
                playerManagerFV.rb.velocity = Vector2.up * playerManagerFV.JumpForce;
                playerManagerFV.JumpTimer -= Time.deltaTime;
                playerManagerFV.JumpForce += 0.15f;
            } else {
            playerManagerFV.animator.SetBool("hitSpace", false);
            playerManagerFV.isJumping = false;
            playerManagerFV.JumpTimer = playerManagerFV.OriginalJumpTimer;
            playerManagerFV.JumpForce = playerManagerFV.OriginalJumpForce;
            }
            
        } 
        yield return new WaitForFixedUpdate();
        StartCoroutine(Jump());
    }
    
}
