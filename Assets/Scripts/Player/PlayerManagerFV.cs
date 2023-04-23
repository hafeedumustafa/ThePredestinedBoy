using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManagerFV : MonoBehaviour
{
    //ground
    public bool m_isGrounded;
    private const float k_groundDistance = 0.5f;
    public Transform m_groundCheck;
    public LayerMask m_whatIsGround;
    //others
    public Rigidbody2D rb;
    public Animator animator;
    public float deathY;
    public GameObject ArrowPref;
    public GameObject Arrow;
    public int weaponEquiped; // 0 is none, 1 is sword, 2 is bow
    //movement
    public float speed, initialspeed;
    public float HorizontalMovement, VerticalMovement;
    private Vector3 oldPosition;
    public Vector2 Velocity;
    //jumping
    public bool isJumping = false;
    public float JumpForce;
    public float JumpTimer;
    public float OriginalJumpTimer;
    public float OriginalJumpForce;
    public bool doublejump;
    private bool doublejumped = false;
    public ParticleSystem DoubleJumpParticles;
    //inputs
    public bool CanMove = true;
    public bool InputByKeyboard;
    
    
    void Awake() {
        try {GameManager.instance.Player = this.gameObject;} catch{}
    }

    void Start()
    {
        initialspeed = speed;
        OriginalJumpTimer = JumpTimer;
        OriginalJumpForce = JumpForce;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("VineBone")) {
            Transform vine = collider.gameObject.transform;
            while(vine.gameObject.CompareTag("VineBone")) {
                vine = vine.parent;
            }

            vine Vine = vine.GetComponent<vine>();
            Vine.ClosestBones.Add(collider.gameObject);

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("VineBone")) {
            Transform vine = collider.gameObject.transform;
            while(vine.gameObject.CompareTag("VineBone")) {
                vine = vine.parent;
            }

            vine Vine = vine.GetComponent<vine>();
            Vine.ClosestBones.Remove(collider.gameObject);

        }
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
            if (Input.GetKeyDown(KeyCode.LeftShift)) {speed = initialspeed + 2;}
            if (Input.GetKeyUp(KeyCode.LeftShift)) {speed = initialspeed;}
        }

        if(transform.position.y <= deathY) {
            //restart game
            GameManager.instance.Death();
        }

        if (Input.GetMouseButtonDown(0) && weaponEquiped != 0 && CanMove == true && InputByKeyboard && Time.timeScale != 0f) {
            animator.SetBool("Attack", true);
        }
        
        weaponEquiped = SaveManager.instance.activeSave.selectedWeapon;

    }

    IEnumerator WaitForMousePressUp() {
        animator.speed = 0;
        yield return new WaitForFixedUpdate();
        if(Input.GetMouseButton(0)) StartCoroutine(WaitForMousePressUp());
        else{
            animator.speed = 1;
            if(SaveManager.instance.activeSave.arrows > 0){
                SaveManager.instance.activeSave.arrows -= 1;
                animator.SetInteger("Arrows", SaveManager.instance.activeSave.arrows);
                GameObject arrow = 
                Instantiate(ArrowPref, Arrow.transform.position, Arrow.transform.rotation);

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 VectorDistance = mousePos - transform.position;
                print(VectorDistance);
                float arrowAngle = Mathf.Atan2(VectorDistance.y, VectorDistance.x);
                arrow.GetComponent<Arrow>().initialRotation = arrowAngle;
            }
        }
    }

    void FixedUpdate()
    {
        Velocity = new Vector2(oldPosition.x - transform.position.x, oldPosition.y - transform.position.y);
        oldPosition = transform.position;
        transform.Translate( HorizontalMovement * speed * Time.fixedDeltaTime, 0, 0);
        
        animator.SetFloat("speed", speed);
        animator.SetInteger("walking?", (int) HorizontalMovement);


        RaycastHit2D hit = Physics2D.Raycast(m_groundCheck.position, Vector2.down, k_groundDistance, m_whatIsGround);
        m_isGrounded = hit;
        animator.SetBool("Grounded?", m_isGrounded);
        try {
        if(hit.transform.gameObject.CompareTag("OneWayGround") && VerticalMovement == -1 && InputByKeyboard && CanMove)
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