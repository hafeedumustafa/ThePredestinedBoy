using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class StoneProdigious : MonoBehaviour
{

    // AttackNumber in animation correlates to: text
    // 0 : null
    // 1 : start
    // 2 : end
    // 3 : idle
    // 4 : walk
    // 5 : rock smack
    // 6 : stun
    // 7 : Lift Arms / Rocks falling // perhaps not appliable aka delete if works without
    // 8 : 
    // 9 : 

    public float health = 200;
    public Image bosshealthui;
    private bool wasAutoSave;
    private bool started;
    public float DamageMultiplier = 1;
    public GameObject Eye;
    public bool endAnims;
    bool stunPlayer;

    public GameObject Rock;
    public Transform[] RockInstantiateLocations;
    public GameObject[] bodys;
    public LayerMask Ground;

    public PlayerManagerFV Player;
    public GameObject[] PlayerBody;

    public Animator MainCameraAnimator;
    public Animator animator;

    public ParticleSystem StrengthenEffect;
    public GameObject SmaccParticles;

    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject NextLeftArmLoc;
    public GameObject NextRightArmLoc;

    public Volume postproccessing;
    private ChromaticAberration glitch;
    private LensDistortion lensDistortion;

    // Current body enabled meaning:
    // if 1, closed.
    //if 0, opened.
    private int CurrentBodyEnabled = 1;

    private int AmountOfMovements;
    private int AmountOfSteps;
    private int direction;

    private bool[] ArmAnimFinish = new [] {false, false};
    public Vector3[] InitialArmLocations;
    public Vector3[] InitialNextArmLocations;
    //public Vector3[] OriganalArmLocations;

        /*
        InitialArmLocations[0] = LeftArm.transform.position;
        InitialArmLocations[1] = RightArm.transform.position;

        InitialNextArmLocations[0] = NextLeftArmLoc.transform.position;
        InitialNextArmLocations[1] = NextRightArmLoc.transform.position;*/

    //private int previousMove = -1; print

    // Start Boss Fight ----------------------------------------------------------------------------------------------------------------------------------------------


    void Update()
    {

        if(!started && Player.m_isGrounded) {
            started = true;
            ChangeBlackBarsState(true);
            animator.SetInteger("AttackNum", 1);
        }

    }

    void Start() 
    {
        if(SaveManager.instance.activeSave.autosave) {
            SaveManager.instance.activeSave.autosave = false;
            wasAutoSave = true;

        }


        InitialNextArmLocations = new [] {
            NextRightArmLoc.transform.position,
            NextLeftArmLoc.transform.position
        };
    }

    public void FinishStart()
    {
        animator.SetInteger("AttackNum", 3);
        ChangeBlackBarsState(false);
        StartCoroutine(StartBossFight());
    }

    public IEnumerator StartBossFight()
    {
        yield return new WaitForSeconds(3);
        StartMovement(false);
        StartCoroutine(WaitingTimeForEyeState());
    }

    public void bossDamaged(float healthReduced) {
        health -= healthReduced;
        if(health <= 0) {
            bosshealthui.fillAmount = 0;
            ChangeBlackBarsState(true);
            endAnims = true;
            animator.SetInteger("AttackNum", 2);

        } else {
            bosshealthui.fillAmount = health / 200;
        }
    }

    void ChangeBlackBarsState(bool state) 
    => GameManager.instance.changeBlackBarsState(state, 0, 0.05f);

    IEnumerator WaitForNextMovement()
    {
        if(!endAnims){
            float RandomWaitingTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(RandomWaitingTime);
            NextBossMove();
        }
    }

    void NextBossMove()
    {
        float RandomMoveF = Random.Range(0f, 5f);//0,5
        int RandomMove = (int) RandomMoveF;

        if(endAnims)
            return;

        if(transform.position.x > 2)
        {   
            direction = -1;
        } else if(transform.position.x < -2)
        {
            direction = 1;
        }

        switch(RandomMove)
        {
            case 0:
            if(direction != 0) {
            StartMovement(true); }
            else {
            StartMovement(false); }
            break;
            
            case 1:
            StartRocksFalling();
            break;
            
            case 2:
            StartCoroutine(StoneAge());
            break;
            
            case 3:
            StartRockWacc();
            break;
            
            case 4:
            StartRockSmacc();
            break;

            default:
            Debug.LogError("PANICC");
            break;
        }
    }

    // Boss Movement ----------------------------------------------------------------------------------------------------------------------------------------------
    
    void StartMovement(bool PredeterminedNextDirection)
    {
        if(!PredeterminedNextDirection) {
            float AmountOfMovementsF = Random.Range(1f, 3f);
            AmountOfMovements = (int) AmountOfMovementsF;

            float directionf = Random.Range(-2f, 2f);

            while ((int) directionf == 0) {
                directionf = Random.Range(-2f, 2f);
            }

            direction = (int) directionf;
            

        }

        float AmountOfStepsF = Random.Range(2f, 4f);
        AmountOfSteps = (int) AmountOfStepsF;

        animator.SetInteger("direction", direction);
        animator.SetInteger("AttackNum", 4);
    }

    public void NextStep()
    {
        transform.position += new Vector3( direction * 0.95f, 0f, 0f);
        AmountOfSteps--;
        if(AmountOfSteps <= 0)
        {
            FinishedDirectionHeading();
        }
    }

    void FinishedDirectionHeading()
    {
        AmountOfMovements--;
        animator.SetInteger("direction", 0);
        animator.SetInteger("AttackNum", 3);
        if(AmountOfMovements >= 1)
        {
            direction *= -1;
            StartMovement(true);
        } else {
            direction = 0;
            StartCoroutine(WaitForNextMovement());
        }
    } 


    // Strengthen --------------------------------------------------------------------------------------------------------------------------------------------

    void Strengthen()
    {
        // increase damage done
        DamageMultiplier = 1.5f;
        StrengthenEffect.Play(); 
    }

    // Eye stuff ----------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator WaitingTimeForEyeState()
    {  
        float RandomWaitingTime = Random.Range(10f, 20f);
        yield return new WaitForSeconds(RandomWaitingTime);
        StartCoroutine(ChangeEyeState());

        float RandomWaitingTimeTwo = Random.Range(3f, 5f);
        yield return new WaitForSeconds(RandomWaitingTimeTwo);
        StartCoroutine(ChangeEyeState());

        StartCoroutine(WaitingTimeForEyeState());
    }

    IEnumerator ChangeEyeState()
    {
        switch(CurrentBodyEnabled)
        {
            
            case 0:
            bodys[1].SetActive(true);
            bodys[0].SetActive(false);

            yield return new WaitForSeconds(0.2f);
            bodys[2].SetActive(true);
            bodys[1].SetActive(false);

            CurrentBodyEnabled = 1;
            break;

            case 1:
            bodys[1].SetActive(true);
            bodys[2].SetActive(false);

            yield return new WaitForSeconds(0.2f);
            bodys[0].SetActive(true);
            bodys[1].SetActive(false);

            CurrentBodyEnabled = 0;
            break;
        }
    }

    // Attacks: ----------------------------------------------------------------------------------------------------------------------------------------------
    // Rock Attack transform

    void SetArms(int TorF) => animator.SetInteger("LiftArms", TorF); //true or false || 1 or 0
    void CameraShake(int TorF) => MainCameraAnimator.SetInteger("Shake", TorF); // true or false || 1 or 0
    void SetAnimatorSpeed(float speed) => animator.speed = speed;

    void StartRocksFalling()
    {
        float AmountOfRocksF = Random.Range(20f, 40f);
        int AmountOfRocks = (int) AmountOfRocksF;

        SetArms(1);
        animator.SetInteger("AttackNum", 7);
        CameraShake(1);

        StartCoroutine(RocksFalling(AmountOfRocks));
    }


    IEnumerator RocksFalling(int AmountOfRocks)
    {
        yield return new WaitForSeconds(0.5f);
        instantiateRock();
        AmountOfRocks--;
        if(AmountOfRocks > 0) {
            StartCoroutine(RocksFalling(AmountOfRocks));
        } else {
            Debug.Log("Rocks stopped falling");
            SetAnimatorSpeed(1);
            CameraShake(0);
            animator.SetInteger("AttackNum", 3);
            StartCoroutine(WaitForNextMovement());
        }
    }


    void instantiateRock()
    {
        Vector3 RockLocation = new Vector3(Random.Range(RockInstantiateLocations[0].position.x, RockInstantiateLocations[1].position.x), RockInstantiateLocations[0].position.y, 0f);
        Quaternion RockRotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject NewRock = Instantiate(Rock, RockLocation, RockRotation);    
        NewRock.transform.GetChild(0).GetComponent<RockFalling>().Boss = this;
    }

    // Stone Age -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator StoneAge()
    {
        yield return new WaitForFixedUpdate();
        Player.HorizontalMovement = 0;
        Player.VerticalMovement = 0;

        Color32 previousPlayerColor = PlayerBody[0].gameObject.GetComponent<SpriteRenderer>().color;

        foreach(GameObject b in PlayerBody) {
            b.gameObject.GetComponent<SpriteRenderer>().color = new Color32(25, 25, 25, 200);
        }
        animator.SetInteger("AttackNum", 6);

        stunPlayer = true;
        yield return new WaitForSeconds(5f);
        stunPlayer = false;

        
        foreach(GameObject b in PlayerBody) {
            b.gameObject.GetComponent<SpriteRenderer>().color = previousPlayerColor;
        }

        animator.SetInteger("AttackNum", 3);
        StartCoroutine(WaitForNextMovement());
    }

    void LateUpdate() {
        if(stunPlayer) {
            Player.HorizontalMovement = 0;
            Player.VerticalMovement = 0;
        }

    }

    // rock wacc -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    void StartRockWacc()
    {
        animator.enabled = !animator.enabled;

        InitialArmLocations[0] = RightArm.transform.position;
        InitialArmLocations[1] = LeftArm.transform.position;

        StartCoroutine(FinishArmWacc());

        StartCoroutine(ArmToLocation(RightArm, InitialNextArmLocations[0], 0f, 0, false));
        StartCoroutine(ArmToLocation(LeftArm, InitialNextArmLocations[1], 0f, 1, false));
    }
    
    //finish animation is whether the arms is going back to initial locations
    //IArmL is InitialNextArmLocations

    IEnumerator ArmToLocation(GameObject arm, Vector3 nextArm, float time, int IArmL, bool FinishAnimation)
    {
        if(!FinishAnimation) {
        arm.transform.position = new Vector3(
            Mathf.Lerp(InitialArmLocations[IArmL].x, nextArm.x, time / 100), 
            Mathf.Lerp(InitialArmLocations[IArmL].y, nextArm.y, time / 100), 
            0f );
        }
        else {
        arm.transform.position = new Vector3(
            Mathf.Lerp(nextArm.x, InitialArmLocations[IArmL].x, time / 100), 
            Mathf.Lerp(nextArm.y, InitialArmLocations[IArmL].y, time / 100), 
            0f );
        }

        yield return new WaitForFixedUpdate();
        time++;

        if(time / 100 > 1f && nextArm == InitialNextArmLocations[0] && !FinishAnimation)
        {
                StartCoroutine(ArmToOtherSide(arm, NextLeftArmLoc, 0f, IArmL));
        } 
        else if(time / 100 > 1f && nextArm == InitialNextArmLocations[1] && !FinishAnimation) {
                StartCoroutine(ArmToOtherSide(arm, NextRightArmLoc, 0f, IArmL));
        } 
        else if(time / 100 > 1f && FinishAnimation)
        {
            ArmAnimFinish[IArmL] = true;
        }
        else {
            StartCoroutine(ArmToLocation(arm, nextArm, time, IArmL, FinishAnimation)); 
        }

    }

    IEnumerator ArmToOtherSide(GameObject arm, GameObject NextLocation, float time, int IArmL)
    {
        arm.transform.position = new Vector3(Mathf.Lerp(InitialNextArmLocations[IArmL].x, NextLocation.transform.position.x, time / 60), arm.transform.position.y, 0f);

        yield return new WaitForFixedUpdate();
        time++;

        if(time / 60 > 1) {
            StartCoroutine(ArmToLocation(arm, NextLocation.transform.position, 0, IArmL, true));
        } else {
            StartCoroutine(ArmToOtherSide(arm, NextLocation, time, IArmL));
        }
    }

    public IEnumerator FinishArmWacc()
    {
        if(ArmAnimFinish[0] == true && ArmAnimFinish[1] == true) {
            ArmAnimFinish[0] = false;
            ArmAnimFinish[1] = false;

            animator.enabled = true;
            StartCoroutine(WaitForNextMovement());
        } else {
            yield return new WaitForFixedUpdate();
            StartCoroutine(FinishArmWacc());
        }
    }

    // Rock Smacc -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    void StartRockSmacc()
    {
        animator.SetInteger("AttackNum", 5);
    }

    public IEnumerator RockSmacced()
    {
        if(Player.m_isGrounded)
        {
            Player.rb.AddForce(new Vector2(0f, 500f));
            // decrease health
        }

        GameObject[] particlez = new [] 
        {
            Instantiate(SmaccParticles, RightArm.transform.position - new Vector3(0f, 2f, 0f), Quaternion.Euler(0, 0, 0)),
            Instantiate(SmaccParticles, LeftArm.transform.position - new Vector3(0f, 2f, 0f), Quaternion.Euler(0, 0, 0))
        };

        StartCoroutine(Glitch(0f, 1));
        StartCoroutine(lensdistortion(0f, -1));

        yield return new WaitForSeconds(0.25f);
        
        yield return new WaitForSeconds(15f);
        Destroy(particlez[0]);
        Destroy(particlez[1]);

    }

    void EndRockSmacc()
    {
        animator.SetInteger("AttackNum", 3);
        StartCoroutine(WaitForNextMovement());
    }

    IEnumerator Glitch(float time, int OnorOff)
    {
        if(postproccessing.profile.TryGet<ChromaticAberration>(out glitch))
        {
            glitch.intensity.value += 0.01f * OnorOff;
            yield return new WaitForFixedUpdate();
            time++;
            if(time / 100 <= 1f)
            {
                StartCoroutine(Glitch(time, OnorOff));
            } else if (OnorOff == 1) {
                StartCoroutine(Glitch(0f, -1));
            } else {
                glitch.intensity.value = 0f;
            }
        } else {
            Debug.LogError("CS4269: glitch effect not found");
        }
    }

    IEnumerator lensdistortion(float time, int OnorOff)
    {
        if(postproccessing.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value += 0.02f * OnorOff;
            yield return new WaitForFixedUpdate();
            time++;

            if(time / 25f < 1f)
            {
                StartCoroutine(lensdistortion(time, OnorOff));
            } else if (OnorOff == -1) {
                StartCoroutine(lensdistortion(0f, 1));
            } else {
                lensDistortion.intensity.value = 0f;
            }
        } else {
            Debug.LogError("CS4269: distortion effect not found");
        }

    }

    //endgame -------------------------------------------------------------------------------------------------------------------------------

    void FinishBossFight() {
        Eye.transform.parent = null;
        Destroy(this.gameObject);
        if(wasAutoSave) 
            SaveManager.instance.activeSave.autosave = true;
        
    }
    

}