using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cinemachine;

public class Interact : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject interact;

    public bool EnterCollisionBox = false;

    public enum TypeOfInteract 
    {
        None,
        Speaking,
        OpeningChest,
        SwitchDirection,

    }


    public TypeOfInteract typeOfInteract;

    void Start()
    {
        if(typeOfInteract == TypeOfInteract.None)
        {
            Debug.LogError("error CS1069: No Interact Object Set");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && GameManager.instance.PlayerCanInteract == true) {
            Canvas.SetActive(true);
            EnterCollisionBox = true;
            StartCoroutine(Alpha(0.075f, 1f));
            StartCoroutine(WaitForInput());
        }

    }

    IEnumerator Alpha(float AlphaIncrease, float FinalNumber)
    {
        yield return new WaitForSeconds(0.01f);
        Canvas.GetComponent<CanvasGroup>().alpha += AlphaIncrease;
        if (Canvas.GetComponent<CanvasGroup>().alpha != FinalNumber) {
            StartCoroutine(Alpha(AlphaIncrease, FinalNumber));
        }
        else if(AlphaIncrease < 0) {
            Canvas.SetActive(false);
        }
    }

    public void ClickedUI() => StartCoroutine(Alpha(-0.075f, 0f));

    public void StartWaitingForInput()
    {
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput() {
        if (Input.GetKeyDown(KeyCode.E)) {
            switch(typeOfInteract)
            {
                case TypeOfInteract.Speaking:
                gameObject.GetComponent<Speak>().StartConversation();
                StartCoroutine(Alpha(-0.075f, 0f));
                break;

                case TypeOfInteract.SwitchDirection:
                gameObject.GetComponent<PathSwitcher>().StartSwitchDirection();
                break;

                case TypeOfInteract.OpeningChest:
                gameObject.GetComponent<openChest>().OpenChest();
                break;

            }

        } else if(EnterCollisionBox){
            yield return 0;
            StartCoroutine(WaitForInput());
        }

    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player" && GameManager.instance.PlayerCanInteract == true) {
            StartCoroutine(Alpha(-0.075f, 0f));
            EnterCollisionBox = false;
        }
    }

    public void touchinteract(bool pressing)
    {
        GameManager.instance.touchInput = pressing;
    }
}
