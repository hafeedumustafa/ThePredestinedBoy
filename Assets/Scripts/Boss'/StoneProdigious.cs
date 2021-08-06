using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Rendering.Universal;

public class StoneProdigious : MonoBehaviour
{
    public int health = 200;
    public GameObject ufinishedgame;
    public TextMeshProUGUI bosshealthui;

    public GameObject Rock;
    public Transform[] RockInstantiateLocations;
    public GameObject[] bodys;
    public LayerMask Ground;

    public Movement Player;
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

    //private int previousMove = -1;

    // Start Boss Fight ----------------------------------------------------------------------------------------------------------------------------------------------

    void Start() 
    {
        StartBossFight();

        InitialNextArmLocations = new [] {
            NextRightArmLoc.transform.position,
            NextLeftArmLoc.transform.position
        };
    }

    public void StartBossFight()
    {
        StartCoroutine(StartMovement(false));
        StartCoroutine(WaitingTimeForEyeState());
    }

    IEnumerator WaitForNextMovement()
    {
        float RandomWaitingTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(RandomWaitingTime);
        NextBossMove();
    }

    void NextBossMove()
    {
        float RandomMoveF = Random.Range(0f, 5f);//0,5
        int RandomMove = (int) RandomMoveF;

        print(RandomMove);

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
            StartCoroutine(StartMovement(true)); }
            else {
            StartCoroutine(StartMovement(false)); }
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
    
    IEnumerator StartMovement(bool PredeterminedNextDirection)
    {
        if(!PredeterminedNextDirection) {
            float AmountOfMovementsF = Random.Range(1f, 3f);
            AmountOfMovements = (int) AmountOfMovementsF;

            float directionf = Random.Range(-2f, 2f);
            direction = (int) directionf;
            

        }
        
        if(direction == 0) {
            yield return new WaitForFixedUpdate();
            StartCoroutine(StartMovement(PredeterminedNextDirection));
            StopCoroutine(StartMovement(PredeterminedNextDirection));
        }

        float AmountOfStepsF = Random.Range(2f, 4f);
        AmountOfSteps = (int) AmountOfStepsF;

        animator.SetInteger("direction", direction);
    }

    public void NextStep()
    {
        transform.position += new Vector3( direction * 1.26f, 0f, 0f);
        AmountOfSteps--;
        if(AmountOfSteps <= 0)
        {
            animator.SetInteger("direction", 0);
            FinishedDirectionHeading();
        }
    }

    void FinishedDirectionHeading()
    {
        AmountOfMovements--;
        if(AmountOfMovements >= 1)
        {
            direction *= -1;
            StartCoroutine(StartMovement(true));
        } else {
            direction = 0;
            StartCoroutine(WaitForNextMovement());
        }
    } 

    // Strengthen --------------------------------------------------------------------------------------------------------------------------------------------

    void Strengthen()
    {
        // increase damage
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
    // Rock Attack 

    void SetArms(int TorF) => animator.SetInteger("LiftArms", TorF);
    void CameraShake(int TorF) => MainCameraAnimator.SetInteger("Shake", TorF);
    void LiftArmsLength(int speed) => animator.speed = speed;

    void StartRocksFalling()
    {
        float AmountOfRocksF = Random.Range(20f, 40f);
        int AmountOfRocks = (int) AmountOfRocksF;

        SetArms(1);
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
            LiftArmsLength(1);
            CameraShake(0);
            StartCoroutine(WaitForNextMovement());
        }
    }


    void instantiateRock()
    {
        Vector3 RockLocation = new Vector3(Random.Range(RockInstantiateLocations[0].position.x, RockInstantiateLocations[1].position.x), RockInstantiateLocations[0].position.y, 0f);
        Quaternion RockRotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject NewRock = Instantiate(Rock, RockLocation, RockRotation);    
    }

    // Stone Age -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator StoneAge()
    {
        Player.CanMove = false;

        Color32 previousPlayerColor = PlayerBody[0].gameObject.GetComponent<SpriteRenderer>().color;

        foreach(GameObject b in PlayerBody) {
            b.gameObject.GetComponent<SpriteRenderer>().color = new Color32(25, 25, 25, 200);
        }
        animator.SetBool("stunning", true);

        yield return new WaitForSeconds(5f);

        Player.CanMove = true;
        
        foreach(GameObject b in PlayerBody) {
            b.gameObject.GetComponent<SpriteRenderer>().color = previousPlayerColor;
        }

        animator.SetBool("stunning", false);
        StartCoroutine(WaitForNextMovement());
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
        print(time);

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
        print(time);

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
        animator.SetBool("rocksmaccing", true);
    }

    public IEnumerator RockSmacced()
    {
        if(Player.m_isGrounded)
        {
            Player.rb.AddForce(new Vector2(0f, 100f));
            // decrease health
        }

        GameObject[] particlez = new [] 
        {
            Instantiate(SmaccParticles, RightArm.transform.position - new Vector3(0f, 2f, 0f), Quaternion.Euler(0, 0, 0)),
            Instantiate(SmaccParticles, LeftArm.transform.position - new Vector3(0f, 2f, 0f), Quaternion.Euler(0, 0, 0))
        };

        StartCoroutine(Glitch(0f, 1));
        StartCoroutine(lensdistortion(0f, -1));

        CameraShake(1);
        yield return new WaitForSeconds(0.25f);
        CameraShake(0);
        
        yield return new WaitForSeconds(15f);
        Destroy(particlez[0]);
        Destroy(particlez[1]);

    }

    void EndRockSmacc()
    {
        animator.SetBool("rocksmaccing", false);
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
    public void EndGame()
    {
        ufinishedgame.SetActive(true);
        Destroy(this.gameObject);
    }

    void Update()
    {
        bosshealthui.text = health.ToString();
    }
}