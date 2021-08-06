using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPhone : MonoBehaviour
{

    public Movement movement;

    public void click()
    {
        if (movement.m_isGrounded) {
            movement.isJumping = true;
            movement.rb.velocity = Vector2.up * movement.JumpForce;
            StartCoroutine(Jump());
        }
    }

    public void unclick()
    {
        StopAllCoroutines();

        movement.animator.SetBool("hitSpace", false);
        movement.isJumping = false;
        movement.JumpTimer = movement.OriginalJumpTimer;
        movement.JumpForce = movement.OriginalJumpForce;
    } 

    IEnumerator Jump()
    {

        if (movement.isJumping) {
            if(movement.JumpTimer > 0)
            {
                movement.rb.velocity = Vector2.up * movement.JumpForce;
                movement.JumpTimer -= Time.deltaTime;
                movement.JumpForce += 0.15f;
            } else {
            movement.animator.SetBool("hitSpace", false);
            movement.isJumping = false;
            movement.JumpTimer = movement.OriginalJumpTimer;
            movement.JumpForce = movement.OriginalJumpForce;
            }
            
        } 
        yield return new WaitForFixedUpdate();
        StartCoroutine(Jump());
    }
    
}
