using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool m_isGrounded;
    private const float k_groundDistance = 0.5f;
    public Transform m_groundCheck;
    public LayerMask m_whatIsGround;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float HorizontalMovement;
    public float VerticalMovement;
    public bool isJumping = false;
    public float JumpForce;
    public float JumpTimer;
    public float OriginalJumpTimer;
    public float OriginalJumpForce;
    public bool CanMove = true;
    public bool InputByKeyboard;
    public float initialspeed;
    public bool doublejump;
    private bool doublejumped = false;
    public ParticleSystem DoubleJumpParticles;
    public float deathY;
    
    void Awake() {
        GameManager.instance.Player = this.gameObject;
    }

    void Start()
    {
        initialspeed = speed;
        OriginalJumpTimer = JumpTimer;
        OriginalJumpForce = JumpForce;

    }
    
    void Update()
    {
        if(InputByKeyboard) {
            if(CanMove) {
                Jump();

                HorizontalMovement = Input.GetAxisRaw("Horizontal");
                VerticalMovement = Input.GetAxisRaw("Vertical");
                if(HorizontalMovement < 0) {
                    transform.localScale = new Vector3(-0.12f, 0.12f, 1);
                } else if(HorizontalMovement > 0) {
                    transform.localScale = new Vector3(0.12f, 0.12f, 1);
                }


            } else {HorizontalMovement = 0f; VerticalMovement = 0f;}
            if (Input.GetKeyDown(KeyCode.LeftShift)) {speed += 2;}
            if (Input.GetKeyUp(KeyCode.LeftShift)) {speed -= 2;}
        }

        if(transform.position.y <= deathY) {
            //restart game
            GameManager.instance.Death();
        }

        if (CanMove == true && Time.timeScale != 0f) {
            if(SaveManager.instance.activeSave.selectedWeapon == 1 && Input.GetMouseButtonDown(0)) {
                    animator.SetBool("Attack", true);
                }
        }


    }

    void FixedUpdate()
    {
        
        transform.Translate( HorizontalMovement * speed * Time.fixedDeltaTime, 0, 0);
        
        animator.SetFloat("speed", speed);
        animator.SetInteger("walking?", (int) HorizontalMovement);


        RaycastHit2D hit = Physics2D.Raycast(m_groundCheck.position, Vector2.down, k_groundDistance, m_whatIsGround);
        m_isGrounded = hit;
        animator.SetBool("Grounded?", m_isGrounded);
        try {
        if(hit.transform.gameObject.tag == "OneWayGround" && VerticalMovement == -1 && InputByKeyboard && CanMove)
        {
            hit.transform.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
            StartCoroutine(OneWaySwitch(hit));
        }
        } catch{}

        if(m_isGrounded && doublejumped) {
            doublejumped = false;
        }
        
        float DistanceFromGround = transform.position.y - hit.point.y;

        if(DistanceFromGround < 0.6 && DistanceFromGround > 0.3) 
        {
            transform.Translate(Vector2.down  * Time.deltaTime);
        }
    }

    void Jump(){
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded) {
            isJumping = true;
            animator.SetBool("hitSpace", true);
            rb.velocity = Vector2.up * JumpForce;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !m_isGrounded && doublejump && !doublejumped) {
            isJumping = true;
            doublejumped = true;
            animator.SetBool("hitSpace", true);
            DoubleJumpParticles.Play();
            rb.velocity = Vector2.up * JumpForce;
        }

        else if (Input.GetKey(KeyCode.Space) && isJumping) {
            if(JumpTimer > 0)
            {
                rb.velocity = Vector2.up * JumpForce;
                JumpTimer -= Time.deltaTime;
                JumpForce += 0.015f;
            } else {
            animator.SetBool("hitSpace", false);
            isJumping = false;
            JumpTimer = OriginalJumpTimer;
            JumpForce = OriginalJumpForce;
            }
            
        } 

        if (Input.GetKeyUp(KeyCode.Space) ) {
            animator.SetBool("hitSpace", false);
            isJumping = false;
            JumpTimer = OriginalJumpTimer;
            JumpForce = OriginalJumpForce;
        }
    }
    
    public void AnimationAttack() {
        animator.SetBool("Attack", false);
    }

    IEnumerator OneWaySwitch(RaycastHit2D hit)
    {
        yield return new WaitForSeconds(1);
        hit.transform.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
    }
}