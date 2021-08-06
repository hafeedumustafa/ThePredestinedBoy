using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathSwitcher : MonoBehaviour
{
    [SerializeField]private float time = 0;
    [SerializeField]private int PathOn;
    public path[] Paths;
    public Transform arrow;
    public Color32 SwitcherColor;
    public Interact interact;

    public GameObject[] SwitchColorsObject;
    public int PreviousPathOn;

    void Start()
    {
        for(int i = 0; Paths.Length > i; i++)
        {
            if(Paths[i].enabled == true) {
                PathOn = i;
            }
        }


    }

    public void StartSwitchDirection()
    {
        StopAllCoroutines();

        Paths[PathOn].enabled = false;
        SwitcherColor = Paths[PathOn].Color;
        PreviousPathOn = PathOn;


        if(PathOn + 1 < Paths.Length){
            PathOn++;

            StartCoroutine(SwitchDirection());
        } else {
            PathOn = 0;

            StartCoroutine(SwitchDirection());
        }

        Paths[PathOn].enabled = true;

        Paths[PathOn].Path.SetActive(true);
        Paths[PreviousPathOn].Path.SetActive(false);
    }

    public IEnumerator SwitchDirection() {

        arrow.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(Paths[PreviousPathOn].clockwiseangle, Paths[PathOn].clockwiseangle, time));

        float rFloat = Mathf.Lerp(Paths[PreviousPathOn].Color.r, Paths[PathOn].Color.r, time);
        float gFloat = Mathf.Lerp(Paths[PreviousPathOn].Color.g, Paths[PathOn].Color.g, time);
        float bFloat = Mathf.Lerp(Paths[PreviousPathOn].Color.b, Paths[PathOn].Color.b, time);

        byte r = Convert.ToByte(rFloat);
        byte b = Convert.ToByte(bFloat);
        byte g = Convert.ToByte(gFloat);

        SwitcherColor = new Color32(r, g, b, 255);

        foreach (GameObject SCO in SwitchColorsObject)
        {
            SCO.GetComponent<SpriteRenderer>().color = SwitcherColor;
        }


        if (time <= 1) { 
            time += 0.1f;
            yield return new WaitForFixedUpdate();
            StartCoroutine(SwitchDirection());
        } else {
            time = 0;
            yield return new WaitForFixedUpdate();
            interact.StartWaitingForInput();
        }
    }
}
