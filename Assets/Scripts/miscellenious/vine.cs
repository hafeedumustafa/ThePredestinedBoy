using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vine : MonoBehaviour
{
    
    public bool swingable;
    private bool OnVine = false;
    private bool TouchingVine = false;
    [SerializeField]private bool vinejump = false;
    public Movement movement;
    [SerializeField]private float distancePlayerToTopVine;


    void Update()
    {
        if(TouchingVine == true)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                OnVine = true;

                movement.animator.SetFloat("Climbing", movement.VerticalMovement);
                movement.animator.SetBool("OnVine", true);

                if (swingable == true) {
                    distancePlayerToTopVine = transform.position.y - movement.transform.position.y;
                }
                movement.rb.gravityScale = 0f;
                movement.rb.velocity = new Vector2(0f, 0f);
                movement.transform.Translate(0f, movement.VerticalMovement * 3 * Time.deltaTime, 0f);

                
            } else if (OnVine == true)
            {
                movement.animator.SetFloat("Climbing", movement.VerticalMovement);
            }
            JumpOnVine();
        }
    }

    void JumpOnVine()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !vinejump) {
            movement.rb.velocity = Vector2.up * movement.JumpForce;
            vinejump = true;
        }

        else if (Input.GetKey(KeyCode.Space) && vinejump) {
            if(movement.JumpTimer > 0)
            {
                movement.rb.velocity = Vector2.up * movement.JumpForce;
                movement.JumpTimer -= Time.deltaTime;
                movement.JumpForce += 0.015f;
            } else {
            vinejump = false;
            movement.JumpTimer = movement.OriginalJumpTimer;
            movement.JumpForce = movement.OriginalJumpForce;
            }
            
        } 

        if (Input.GetKeyUp(KeyCode.Space)) {
            vinejump = false;
            movement.JumpTimer = movement.OriginalJumpTimer;
            movement.JumpForce = movement.OriginalJumpForce;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        TouchingVine = true;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        TouchingVine = false;
        OnVine = false;
        movement.rb.gravityScale = 1f;
        movement.animator.SetBool("OnVine", false);
    }

}
