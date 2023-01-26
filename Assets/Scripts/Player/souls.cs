using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.IO;

public class souls : MonoBehaviour
{

    public List<GameObject> soulObject = new List<GameObject>();
    public GameObject deathScreen;
    public GameObject FullSoul;
    public GameObject HalfSoul;
    public GameObject EmptySoul;
    
    void Awake() { // settting keys text from game manager
        try {
        GameManager.instance.KeysText = GetComponent<RectTransform>().Find("Keys").Find("NumberOfKeys").gameObject;
        GameManager.instance.Souls = this.gameObject;} catch{}
    }

    void Start()
    {
        
        float souls = SaveManager.instance.activeSave.Souls;
        if(souls <= 0) {
            SaveManager.instance.activeSave.Souls = ((float) SaveManager.instance.activeSave.maxSouls) / 2;
        }
        
        changeSouls();
    }

    void changeSouls()
    {
        for(int i = 0; i < SaveManager.instance.activeSave.Souls; i++) 
        {
            if(SaveManager.instance.activeSave.Souls - i == 0.5f) {
                GameObject Soul = Instantiate(HalfSoul, GetComponent<RectTransform>());
                Soul.GetComponent<RectTransform>().localPosition += new Vector3(i * 40, 0, 0);
                soulObject.Add(Soul);
            } else {
                GameObject Soul = Instantiate(FullSoul, GetComponent<RectTransform>());
                Soul.GetComponent<RectTransform>().localPosition += new Vector3(i * 40, 0, 0);
                soulObject.Add(Soul);
            }
        }

        for(
            int i = (int) Math.Round(SaveManager.instance.activeSave.Souls, MidpointRounding.AwayFromZero); 
            i < SaveManager.instance.activeSave.maxSouls; i++)
        {

            GameObject emptySoul = Instantiate(EmptySoul, GetComponent<RectTransform>());
            emptySoul.GetComponent<RectTransform>().localPosition += new Vector3(i * 40, 0, 0);
            soulObject.Add(emptySoul);

        }
    }

    void DeleteSouls()
    {
        for(int i = 0; i < soulObject.Count; i++)
        {
            Destroy(soulObject[i]);
        }
        soulObject.Clear();
    }

    public void healthAdjusted(float value)
    {
        if(value % 0.5 != 0) 
            value = Mathf.Round(value * 2) / 2;
        
        print(value + "damage awarded");
        
        SaveManager.instance.activeSave.Souls += value;

        if(SaveManager.instance.activeSave.Souls > SaveManager.instance.activeSave.maxSouls) 
            SaveManager.instance.activeSave.Souls = SaveManager.instance.activeSave.maxSouls;
        
        
        DeleteSouls();

        if(SaveManager.instance.activeSave.Souls > 0) 
            changeSouls();
        else 
            GameManager.instance.Death();
                
        if(SaveManager.instance.activeSave.Souls <= 1)
        {
            //beating noises + red vignette //search how to make good art
        }

        
    }

}
