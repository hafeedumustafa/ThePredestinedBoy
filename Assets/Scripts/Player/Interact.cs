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

    private CanvasGroup CanvasAlpha;

    public TypeOfInteract typeOfInteract;

    void Start()
    {
        if(typeOfInteract == TypeOfInteract.None)
        {
            Debug.LogError("error CS1069: No Interact Object Set");
        }

    }

    public IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        CanvasAlpha = Canvas.GetComponent<CanvasGroup>();

        if(CanvasAlpha.alpha != 0 && CanvasAlpha.alpha != 1) {
            yield return new WaitForSeconds(0.13f);// 1 / 0.075 * 0.01
        }

        if (collider.CompareTag("Player") && GameManager.instance.PlayerCanInteract == true) {
            Canvas.SetActive(true);
            EnterCollisionBox = true;
            StartCoroutine(Alpha(0.075f, 1f));
            StartCoroutine(WaitForInput());
        }

    }

    IEnumerator Alpha(float AlphaIncrease, float FinalNumber)
    {
        yield return new WaitForSeconds(0.01f);// <-------------------------- change to bigger // maybe not
        CanvasAlpha.alpha += AlphaIncrease;
        if (CanvasAlpha.alpha != FinalNumber) {
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

    public IEnumerator OnTriggerExit2D(Collider2D collider)
    {
        CanvasAlpha = Canvas.GetComponent<CanvasGroup>();
        if(CanvasAlpha.alpha != 0 && CanvasAlpha.alpha != 1) {
            yield return new WaitForSeconds(0.15f);// 1 / 0.075 * 0.01
        }
        if (collider.CompareTag("Player") && GameManager.instance.PlayerCanInteract == true) {
            StartCoroutine(Alpha(-0.075f, 0f));
            EnterCollisionBox = false;
        }
    }

    public void touchinteract(bool pressing)
    {
        GameManager.instance.touchInput = pressing;
    }
}
