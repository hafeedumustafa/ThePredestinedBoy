using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vine : MonoBehaviour
{
    
    private bool OnVine = false;
    [SerializeField]private bool vinejump = false;
    [SerializeField]public bool TouchingVine = false;
    public bool Swingable;

    public PlayerManagerFV playerManagerFV;
    [Range(0, 2f)]
    [SerializeField]private float speed;
    public Animator animator;
    public GameObject[] Bones;
    public Transform BoneOfCollision;
    public List<GameObject> ClosestBones = new List<GameObject>();
    public TargetJoint2D targetJoint;
    private bool waitUntilHitGround = false;
    private float Direction;
    private float oldRotation;
    private float zRotationVelocity;
    public float highestFrameInAnimation;
    [SerializeField]private float time;
    [SerializeField]private float addTime;
    int initialDirection;
    

    void FixedUpdate()
    {
        if(TouchingVine == true)
        {
            //ClimbingVine
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                OnVine = true;

                playerManagerFV.animator.SetFloat("Climbing", playerManagerFV.VerticalMovement);
                playerManagerFV.animator.SetBool("OnVine", true);

                playerManagerFV.rb.gravityScale = 0f;
                playerManagerFV.rb.velocity = new Vector2(0f, 0f);
                playerManagerFV.transform.Translate(0f, playerManagerFV.VerticalMovement * 3 * Time.deltaTime, 0f);

                
            } else if (OnVine == true) {
                playerManagerFV.animator.SetFloat("Climbing", playerManagerFV.VerticalMovement);
            }


            if(!Swingable)
                JumpOnVine();

            //Swinging Vine

            if(OnVine && Swingable && !animator.GetBool("VineIsSwinging")) {
                animator.SetBool("VineIsSwinging", true);
                playerManagerFV.CanMove = false;
                //targetJoint.enabled = true;

                float closestBone = 10000;
                for(int i = 0; i < ClosestBones.Count; i++) {
                    if(Vector3.Distance(ClosestBones[i].transform.position, playerManagerFV.gameObject.transform.position) < closestBone)
                    {
                        BoneOfCollision = ClosestBones[i].transform;
                    }
                }


                if(playerManagerFV.HorizontalMovement < 0) {
                    initialDirection = -1;
                    oldRotation = 0.1f;
                } else {
                    initialDirection = 1;
                    oldRotation = -0.1f;

                }

                animator.Play("3SwingingVines", -1, 0.5f);

                float playerVel = playerManagerFV.Velocity.x * 7.5f;
                if(playerVel > 1)
                    playerVel = 1;
                
                speed = playerVel * -initialDirection;


            }


        }

        // Swinging Vine

        if(Swingable && animator.GetBool("VineIsSwinging"))
        {
            //set target
            //targetJoint.target = new Vector2(BoneOfCollision.position.x, BoneOfCollision.position.y);

            //get direction of vine

            time += initialDirection * 2f;
            float sinTime = Mathf.Cos(time * Mathf.PI/180);
            print(sinTime);

            
            animator.SetFloat("speed", sinTime);

            
            if(zRotationVelocity < 0) 
                Direction = -1;
            else if(zRotationVelocity > 0) 
                Direction = 1;
            
            float rotationZ = Bones[0].transform.rotation.z;
            zRotationVelocity = rotationZ - oldRotation;
            oldRotation = rotationZ;

            // check whether player is going correct direcion
            if(Input.GetAxisRaw("Horizontal") == Direction) {
                if(initialDirection > 0 && speed <= 2f){
                    speed += 0.01f;
                    //animator.SetFloat("speed", animator.GetFloat("speed") * speed);
                    
                } else if(initialDirection < 0 && speed >= -2f){
                    speed -= 0.01f;
                    //animator.SetFloat("speed", animator.GetFloat("speed") * speed);
                }

            } else {

                if(initialDirection > 0 && speed > 0.06f){
                    speed -= 0.01f;
                    //animator.SetFloat("speed", animator.GetFloat("speed") * speed);
                    
                } else if(initialDirection < 0 && speed < -0.06f){
                    speed += 0.01f;
                    //animator.SetFloat("speed", animator.GetFloat("speed") * speed);
                }

            }

            animator.SetFloat("speed", animator.GetFloat("speed") * speed);

            print(speed + " " + Input.GetAxisRaw("Horizontal"));
            

            //set rotation of player
            playerManagerFV.gameObject.transform.eulerAngles = 
            new Vector3(0, 0, BoneOfCollision.eulerAngles.z + 90);

            //set position of player
            playerManagerFV.gameObject.transform.position = 
            new Vector3(BoneOfCollision.position.x, BoneOfCollision.position.y, 0f);
            

            if(Input.GetKeyDown(KeyCode.Space)) {
                OffSwingingVine();
            } 

        }

        if(waitUntilHitGround && playerManagerFV.m_isGrounded) {
            waitUntilHitGround = false;
            if(playerManagerFV.speed == playerManagerFV.initialspeed)
                playerManagerFV.speed = playerManagerFV.initialspeed;
        }
    }

    private void OffSwingingVine() {

        animator.SetBool("VineIsSwinging", false);// <- after
        playerManagerFV.CanMove = true;
        speed = 0;
        time = 0;

        //targetJoint.enabled = false;

        if(playerManagerFV.speed == playerManagerFV.initialspeed) 
            playerManagerFV.speed +=2;

        playerManagerFV.gameObject.transform.eulerAngles = 
        new Vector3(0, 0, 0);

        waitUntilHitGround = true;

    }

    private void VineStoppedMoving() {
        animator.SetBool("VineIsSwinging", false);
    }



    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player")) {
            TouchingVine = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) {
            TouchingVine = false;
            OnVine = false;
            playerManagerFV.rb.gravityScale = 1f;
            playerManagerFV.animator.SetBool("OnVine", false);
        }

    }


    void JumpOnVine()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !vinejump) {
            playerManagerFV.rb.velocity = Vector2.up * playerManagerFV.JumpForce;
            vinejump = true;
        }

        else if (Input.GetKey(KeyCode.Space) && vinejump) {
            if(playerManagerFV.JumpTimer > 0)
            {
                playerManagerFV.rb.velocity = Vector2.up * playerManagerFV.JumpForce;
                playerManagerFV.JumpTimer -= Time.deltaTime;
                playerManagerFV.JumpForce += 0.015f;
            } else {
            vinejump = false;
            playerManagerFV.JumpTimer = playerManagerFV.OriginalJumpTimer;
            playerManagerFV.JumpForce = playerManagerFV.OriginalJumpForce;
            }
            
        } 

        if (Input.GetKeyUp(KeyCode.Space)) {
            vinejump = false;
            playerManagerFV.JumpTimer = playerManagerFV.OriginalJumpTimer;
            playerManagerFV.JumpForce = playerManagerFV.OriginalJumpForce;
        }
    }

    //void OnTriggerEnter2D(Collider2D collider)
    //{
      //  if(collider.gameObject.tag == "Player" && SwingingVine)
        //{
      //      OnVine = true;
      //      StartCoroutine(StartMoving(true));
     //   }
    //}
    /*


    int Direction;
    float angle;
    float angleAdd;


    IEnumerator StartMoving(bool start)
    {
        if(start){
            Direction = (int) movement.HorizontalMovement;
            RopeSegments[0] = new Vector3( 0.1f * Player.GetComponent<Movement>().HorizontalMovement, 0);
        }
        else {
            RopeSegments[0] = new Vector3( 0.25f * angle, 0);
        }
        angleAdd += Direction;
        angle = Mathf.Sin(angleAdd * Mathf.PI/180);
        print(angle);
        float InitialAngle = RopeSegments[0].x;
        
        for(int i = 1; i < RopeSegments.Count; i++)
        {
            if(i != 0) {
            RopeSegments[i] = new Vector3 
            (RopeSegments[i-1].x * 2,
            -1 * Mathf.Sqrt( Mathf.Pow( RopeSegments[i-1].y, 2) - Mathf.Pow(RopeSegments[i-1].x, 2) ),
            0f ); }
            
            
            VineAesthetics[i].transform.position = lineRenderer.GetPosition(i);
        }
        

        if(OnVine) {
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(StartMoving(false));}


        for(int i = 0; i < NumberOfRopeSeg; i++) 
            lineRenderer.SetPosition(i, RopeSegments[i]);
        
    }*/


    /*
    public bool SwingingVine;
    public LineRenderer lineRenderer;
    public GameObject Player;
    List<Vector3> RopeSegments = new List<Vector3>();
    [Range(1,6)]
    public int NumberOfRopeSeg;
    public GameObject VineAestheticsPrefab;
    public List<GameObject> VineAesthetics;*/
/*
    void Start()
    {
        if(SwingingVine){
            for(int i = 0; i < NumberOfRopeSeg; i++)
            {
                RopeSegments.Add(new Vector3(0, i * -1, 0));
                lineRenderer.SetPosition(i, RopeSegments[i]);
                GameObject VineAesth = Instantiate(VineAestheticsPrefab, transform);

                if(i%2 == 0) {
                    VineAesth.GetComponent<SpriteRenderer>().flipX = true;
                    VineAesth.transform.position += new Vector3(0.2f, -i-0.15f, 0f);
                } else {
                    VineAesth.transform.position += new Vector3(-0.2f, -i-0.15f, 0f);
                }
                
                VineAesthetics.Add(VineAesth);
            }
        }
    }*/

}
