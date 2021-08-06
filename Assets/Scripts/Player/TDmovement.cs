using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDmovement : MonoBehaviour
{

    public float HorizontalMovement;
    public float VerticalMovement;
    public Animator animator;
    public Rigidbody2D rb;
    public float m_speed;
    public float m_InitialSpeed;
    [Range(-2, 2)]
    public int LastKeyPressed; // 0 means none, 1 means D & A, 2 means W, -2 means S

    public bool freezePosition = false;

    public GameObject[] objectsObtained;
    int ObjectFromChest;

    public CameraController cc;
    public bool CanMove = true;
    public bool InputByKeyboard;
    
    void Awake() {
        GameManager.instance.Player = this.gameObject;
    }

    void Update()
    {
        //movement + running
        if(InputByKeyboard) {
            if(CanMove) {
                HorizontalMovement = Input.GetAxisRaw("Horizontal");
                VerticalMovement = Input.GetAxisRaw("Vertical"); 

                if(VerticalMovement < 0) {LastKeyPressed = -2;} 
                else if(VerticalMovement > 0) {LastKeyPressed = 2;} 

                if(HorizontalMovement < 0) {
                    transform.localScale = new Vector3(-0.2f, 0.2f, 1);
                    LastKeyPressed = 1;
                } else if(HorizontalMovement > 0) {
                    transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    LastKeyPressed = 1;
                }


            }
            else {
                HorizontalMovement = 0;
                VerticalMovement = 0;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) { m_speed = 4.69f; }
            if (Input.GetKeyUp(KeyCode.LeftShift)) { m_speed = 2.75f; }

        } 

        //animations
        animator.SetInteger("Horizontal", (int) HorizontalMovement);
        animator.SetInteger("Vertical", (int) VerticalMovement);
        animator.SetInteger("LastKeyPressed", LastKeyPressed);
        animator.SetFloat("Speed", m_speed);

        /*movement mobile 
        if(HorizontalMovement != 0f) {
        animator.SetBool("WalksRight", true);
        } else {
            animator.SetBool("WalksRight", false);
        } 
        if(HorizontalMovement < -0.5f) {
        animator.SetBool("WalksLeft", true);
        } else {
            animator.SetBool("WalksLeft", false);
        }
        if(VerticalMovement > 0.5f) {
        animator.SetBool("WalksUp", true);
        } else {
            animator.SetBool("WalksUp", false);
        }
        if(VerticalMovement < -0.5f) {
        animator.SetBool("WalksDown", true);
        } else {
            animator.SetBool("WalksDown", false);
        }     */

        if (CanMove && Time.timeScale != 0f) {
        if(SaveManager.instance.activeSave.selectedWeapon == 1 && Input.GetMouseButtonDown(0)) {
                animator.SetBool("attack", true);
            }
        }

    }

    void FixedUpdate()
    {
        transform.Translate(HorizontalMovement * m_speed * Time.fixedDeltaTime, VerticalMovement * m_speed * Time.fixedDeltaTime, 0);
        
    }

    public void OnChestOpen(int objectFromChest) {
        CanMove = false;

        ObjectFromChest = objectFromChest;
        objectsObtained[ObjectFromChest].SetActive(true);
        
        animator.SetBool("openChest", true);
    }

    public void OnAnimationFinish() {
        CanMove = true;

        objectsObtained[ObjectFromChest].SetActive(false);
        animator.SetBool("openChest", false);

        cc.ZoomStatus = "zooming out";
    }

    public void animationAttack() {
        animator.SetBool("attack", false);
    }
}
