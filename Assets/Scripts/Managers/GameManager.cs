using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.IO;
using System.Xml;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    static public int health = 4;
    public GameObject deathScreen;
    public GameObject Souls;
    public GameObject KeysText;
    public GameObject Player;
    public VectorValue vectorValue;
    public bool PlayerCanInteract = true;
    public bool touchInput = false;
    public GameObject blackbars;
    
    //start
    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    public void NewSceneStart() {
        string dataPath = Application.persistentDataPath;
        if (!System.IO.File.Exists(dataPath + "/" + SaveManager.instance.activeSave.SaveName + ".savefile") || SaveManager.instance.switchedScene) {
            Player.transform.position = vectorValue.startValue; 
            print("playerpos completed");
        }

    }


    //(en)(dis)able player interacting e.g. walking, touching etc.
    public void InteractingPlayer(bool OnOrOff)
    {
        PlayerCanInteract = OnOrOff;
        if(Player.TryGetComponent(out PlayerManagerFV movement)) {
            movement.CanMove = OnOrOff;
        } 
        else if(Player.TryGetComponent(out PlayerManagerTD tdmovement)) {
            tdmovement.CanMove = OnOrOff;
        }
    }

    //saving data values
    public void LoadingValues() {
        Player.transform.position = SaveManager.instance.activeSave.PlayersPosition;

        float souls = SaveManager.instance.activeSave.Souls;
        if(souls <= 0) {
            SaveManager.instance.activeSave.Souls = (float) SaveManager.instance.activeSave.maxSouls / 2f;
        }
        print("LOADED VALUES");

    }
    public void SavingValues() {
        SaveManager.instance.activeSave.PlayersPosition = Player.transform.position;
        SaveManager.instance.activeSave.Scene = SceneManager.GetActiveScene().name;
    }

    //death
    public void Death() {
        Player.GetComponent<PlayerManagerFV>().rb.gravityScale = 0f;
        SaveManager.instance.activeSave.Souls = 0;
        deathScreen.SetActive(true);
        StartCoroutine(DeathScreenActivation(0.02f, 1));
        Destroy(Player.gameObject);

    }

    IEnumerator DeathScreenActivation(float IncrementValue, float finalValue)
    {
        
        if(deathScreen.GetComponent<CanvasGroup>().alpha >= finalValue) {
            deathScreen.GetComponent<CanvasGroup>().alpha = finalValue;
            Time.timeScale = 0f;
        } else {
            yield return new WaitForFixedUpdate();
            
            deathScreen.GetComponent<CanvasGroup>().alpha += IncrementValue;
            StartCoroutine(DeathScreenActivation(IncrementValue, finalValue));
        }
    }

    public void SetAOK(int Key) {
        SaveManager.instance.activeSave.keys += Key;
        KeysText.GetComponent<TextMeshProUGUI>().text = SaveManager.instance.activeSave.keys.ToString();
    }

    //black bars
    
    public void changeBlackBarsState(bool enable, float time, float increaseTime)
    {
        print(".");
        StartCoroutine(ChangeBlackBarsState(enable, time, increaseTime));
    }

    IEnumerator ChangeBlackBarsState(bool enable, float time, float increaseTime) {
        time += increaseTime;
        if(enable) {
            blackbars.SetActive(true);
            blackbars.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
        } else {
            blackbars.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, time);
        }

        if(time >= 1) {if(!enable) {enable = false;}}
        else { 
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(ChangeBlackBarsState(enable, time, increaseTime));
        }

    }

}