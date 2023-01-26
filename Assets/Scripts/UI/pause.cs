using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    
    public GameObject blackBars;
    public GameObject _pause;
    public GameObject _unPause;
    public GameObject _options;
    public GameObject _map;
    public GameObject[] AutoSaveTime;
    public Toggle[] AutoSaveTimeToggle;
    public Toggle autosaveButton;
    public SelectedItemScript selectedItemScript;

    private bool OnOptions = false;
    private bool mapOpened = false;
    private bool isPaused = false;

    private int publicTime;
    
    bool grounded;

    public SpriteRenderer[] PlayerLayers;
    public Color32[] Colors;

    public GameObject[] weaponSlots;
    public Button[] wsb;

    void Awake() {
        try{
        GameManager.instance.blackbars = blackBars;} catch{}
    }

    void Start() {
        autosaveButton.isOn = SaveManager.instance.activeSave.autosave;
        
        for(int i = 0; i < AutoSaveTime.Length; i++) {
            AutoSaveTime[i].SetActive(SaveManager.instance.activeSave.autosave);
        }
        for (int i = 0; i < AutoSaveTimeToggle.Length; i++) {
            if(AutoSaveTimeToggle[i].GetComponent<GameObjectValues>().value == SaveManager.instance.activeSave.AutoSaveTime) {
                AutoSaveTimeToggle[i].isOn = true;
            }
        }

        selectedItemScript.SISprite.color = new Color32(0, 0, 0, 0);
        selectedItemScript.animator.SetInteger("ItemEquiped", SaveManager.instance.activeSave.selectedWeapon);

        //for(int i = 0; i < PlayerLayers.Length; i++) {
        //    PlayerLayers[i] = GameManager.instance.Player.transform.Find("layers").GetChild(i).GetComponent<SpriteRenderer>();
        //    PlayerLayers[i].color = Colors[SaveManager.instance.activeSave.color];
        //}
        //ChangeColor(SaveManager.instance.activeSave.color);
        
        for (int i = 0; i < SaveManager.instance.activeSave.weaponsObtained.Count; i++)
        {//future me this needs to be a double for loop so that it selects the correct item
            wsb[i].interactable = true;
            weaponSlots[i].GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);
        }
    }



    public void ChangeColor(int newColor)
    {
        for(int i = 0; i < PlayerLayers.Length; i++) {
            PlayerLayers[i] = GameManager.instance.Player.transform.Find("layers").GetChild(i).GetComponent<SpriteRenderer>();
            PlayerLayers[i].color = Colors[newColor];
        }
        SaveManager.instance.activeSave.color = newColor;
    }

    void Update()
    {
        try {
            grounded = GameManager.instance.Player.GetComponent<PlayerManagerFV>().m_isGrounded;
        } catch {
            grounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) 
            && !isPaused && !OnOptions && !mapOpened 
            && GameManager.instance.PlayerCanInteract 
            && grounded) {

            isPaused = true;
            _pause.SetActive(true);
            _unPause.SetActive(false);
            Time.timeScale = 0f;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused && !OnOptions && !mapOpened) {
            isPaused = false;
            _pause.SetActive(false);
            _unPause.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    public void Options() {
        _pause.SetActive(false);
        _unPause.SetActive(false);
        _options.SetActive(true);
        OnOptions = true;
        Time.timeScale = 0f;
    }

    public void Map() {
        _map.SetActive(true);
        _pause.SetActive(false);
        mapOpened = true;
    }
    public void backToPause() {
        isPaused = true;
        _pause.SetActive(true);
        _unPause.SetActive(false);
        _options.SetActive(false);
        _map.SetActive(false);
        mapOpened = false;
        OnOptions = false;
        Time.timeScale = 0f;
    }

    public void Save() {
        SaveManager.instance.Save();
    }

    public void ToggleTime() {
        if(Time.timeScale == 0f) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
    }

    
    public void ToggleAutoSave(bool state) {
            SaveManager.instance.activeSave.autosave = state;

            if (state == true) {
                SaveManager.instance.Save();

                for(int i = 0; i < AutoSaveTime.Length; i++) {
                    AutoSaveTime[i].SetActive(true);
                }
            } 
            else {
                for(int i = 0; i < AutoSaveTime.Length; i++) {
                    AutoSaveTime[i].SetActive(false);
                }
            }
    }

    public void SaveTime_1(int time) {
        if (AutoSaveTimeToggle[time].isOn == true) {
            publicTime = time;
        }
    }
    public void SaveTime_2(int time) {
        if (AutoSaveTimeToggle[publicTime].isOn == true) {
            SaveManager.instance.activeSave.AutoSaveTime = time;
            //SaveManager.instance.Save();
        }
    }
}
