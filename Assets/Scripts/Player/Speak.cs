using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;

public class Speak : MonoBehaviour
{

    public CameraController CamCon;
    public Interact interact;

    public float LerpValueII;
    public float speed;

    [TextArea(3, 7)]
    public string[] sentences;
    private char[] LettersInSentence;

    private int currentsentence;
    private int currentChar = 0;

    private bool convoState = false;

    public TextMeshProUGUI Text;
    public GameObject TextBox;
    public GameObject BlackBar;
    public GameObject UnpausedUI;
    public GameObject NextSentenceIndicator;
    public Animator[] animations;

    //start conversation v
    public void StartConversation()
    {
        convoState = true;
        CameraZoom("zooming in");

        TextBox.SetActive(true);
        StartCoroutine(Alpha(TextBox, 0.15f, 1f));

        BlackBar.SetActive(true);
        StartCoroutine(Alpha(BlackBar, 0.15f, 1f));

        UnpausedUI.SetActive(false);

        GameManager.instance.InteractingPlayer(false);
        
        foreach(Animator animation in animations) {
            animation.SetBool("Play", true);
        }

        print("starting convo");
        StartCoroutine(PrintText(sentences[0]));
        StartCoroutine(WaitForUpdate());
        
    }


    void CameraZoom(string zoomstat)
    {
        CamCon.LerpValueII = LerpValueII;
        CamCon.speed = speed;
        if(zoomstat == "zooming in") {
            CamCon.SaveCameraPosition();
        }
        CamCon.ZoomStatus = zoomstat;

    }

    IEnumerator Alpha(GameObject AlphaObject, float AlphaIncrease, float FinalNumber)
    {
        yield return new WaitForSeconds(0.04f);
        AlphaObject.GetComponent<CanvasGroup>().alpha += AlphaIncrease;
        if (AlphaObject.GetComponent<CanvasGroup>().alpha != FinalNumber) {
            StartCoroutine(Alpha(AlphaObject, AlphaIncrease, FinalNumber));
        }
        else if(AlphaIncrease < 0) {
            AlphaObject.SetActive(false);
        }
    }

    //during conversation v
    
    IEnumerator WaitForUpdate()
    {
        yield return 0;
        
        if(Input.anyKeyDown && currentChar < LettersInSentence.Length)
        {
            currentChar = LettersInSentence.Length;
            Text.text = sentences[currentsentence];
        } else if(convoState){
            StartCoroutine(WaitForUpdate());
        }
    }
    

    IEnumerator PrintText(string sentence)
    {
        
        LettersInSentence = sentence.ToCharArray();
        yield return new WaitForSeconds(0.05f);
        if(LettersInSentence.Length > currentChar) {
            Text.text += LettersInSentence[currentChar];
            currentChar++;

            StartCoroutine(PrintText(sentence));
        } 
        else if(sentences.Length - 1 > currentsentence) {
            NextSentenceIndicator.SetActive(true);
            StartCoroutine(WaitForInput(false));

        } else {
            StartCoroutine(WaitForInput(true));
        }
    }

    private IEnumerator WaitForInput(bool EndOfSentence)
    {
        yield return 0;
        if(Input.anyKeyDown){
                LettersInSentence = null;
                currentChar = 0;
                currentsentence++;
                Text.text = null;
                NextSentenceIndicator.SetActive(false);
                if(EndOfSentence == false) {
                    StartCoroutine(PrintText(sentences[currentsentence]));
                    StartCoroutine(WaitForUpdate());
                } else {
                    FinishConversation();
            }
        }
        else {
            StartCoroutine(WaitForInput(EndOfSentence));
        }
    }

    //finish conversation v

    void FinishConversation()
    {
        print("convo ended");
        convoState = false;
        currentsentence = 0;
        CameraZoom("zooming out");
        StartCoroutine(Alpha(TextBox, -0.15f, 0f));
        StartCoroutine(Alpha(BlackBar, -0.15f, 0f));
        UnpausedUI.SetActive(true);

        GameManager.instance.InteractingPlayer(true);

    }
}
