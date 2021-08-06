using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGextreme : MonoBehaviour// THIS IS FOR MAIN MENU WHEN CHANGING BETWEEN XTREME AND EZ
{
    
    public GameObject[] bg;
    public GameObject sun;

    void Start()
    {
        if(SaveManager.instance.activeSave.XTREME)
        {
            foreach(GameObject b in bg)
            {
                b.GetComponent<SpriteRenderer>().color = new Color32(255, 30, 30, 255);
            }
            sun.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        }
    }

}
