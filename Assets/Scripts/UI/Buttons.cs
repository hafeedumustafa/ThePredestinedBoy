using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public Animator fade;
    public GameObject newGame;
    public GameObject newGame1;
    public GameObject back;
    public GameObject Load;
    public GameObject Xtreme;
    public GameObject ez;
    public GameObject background;
    public float timer = 1f;

    public GameObject pm;
    public GameObject om;
    
    public AudioMixer audioMixer;

    public Color32 EasyColor;
    public Color32 NormalColor;
    public Color32 HardColor;

    public Sprite audiolow;
    public Sprite audiomedium;
    public Sprite audiohigh;
    public Sprite audiomute;

    Color32 HardColors;

    void Start() {
        string dataPath = Application.persistentDataPath;
        if (!System.IO.File.Exists(dataPath + "/" + SaveManager.instance.activeSave.SaveName + ".savefile") && Load != null)
            Load.SetActive(false);
        
    }

    // main menu -->

    public void HardMode()
    {
        //background.GetComponent<Image>().color = new Color32(255, 50, 50, 255);
        HardColors = background.GetComponent<Image>().color;
        SaveManager.instance.activeSave.XTREME = true;
        StartCoroutine(SwitchHardColors(255, 50, 50, 0f));
        ez.SetActive(true);
        Xtreme.SetActive(false);
    }

    IEnumerator SwitchHardColors(int a, int b, int c, float t)
    {
        t += 0.05f;
        background.GetComponent<Image>().color = new Color32(
            (byte) Mathf.Lerp(HardColors.r, a, t), 
            (byte) Mathf.Lerp(HardColors.g, b, t), 
            (byte) Mathf.Lerp(HardColors.b, c, t), 
            255);

        yield return new WaitForFixedUpdate();

        if(t < 1) {
            StartCoroutine(SwitchHardColors(a, b, c, t));
        }
        
    }

    public void Ez()
    {
        //background.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        HardColors = background.GetComponent<Image>().color;
        SaveManager.instance.activeSave.XTREME = false;
        StartCoroutine(SwitchHardColors(255, 255, 255, 0));
        Xtreme.SetActive(true);
        ez.SetActive(false);
    }

    public void Play(string NextSceneName)
    {
        SaveManager.instance.switchedScene = true;
        StartCoroutine(PlayNextLevel(NextSceneName));
    }
    
    IEnumerator PlayNextLevel(string levelName)
    {
        fade.SetTrigger("Start");

        yield return new WaitForSeconds(timer); 

        SceneManager.LoadSceneAsync(levelName);

    }

    public void StartGame()
    {
        SaveManager.instance.switchedScene = false;
        StartCoroutine(PlayNextLevel(SaveManager.instance.activeSave.Scene));
    }

    public void NewGame() 
    {
        back.SetActive(true);
        newGame1.SetActive(true);
        Xtreme.SetActive(true);
        Load.SetActive(false);
        newGame.SetActive(false);

    }

    public void Back()
    {
        SaveManager.instance.activeSave.XTREME = false;
        background.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Xtreme.SetActive(false);
        ez.SetActive(false);

        back.SetActive(false);
        newGame1.SetActive(false);
        Load.SetActive(true);
        newGame.SetActive(true);
    }

    public void CreateNewGame()
    {
        if(Xtreme.activeSelf)
        {
            SaveManager.instance.activeSave.XTREME = false;
        } else {
            SaveManager.instance.activeSave.XTREME = true;
        }

        SaveManager.instance.DeleteData(SaveManager.instance.activeSave.XTREME);
        SaveManager.instance.switchedScene = false;
        StartCoroutine(PlayNextLevel("W1-main"));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit!");
    }

    // options menu -->

    public void PlayOptions() {
        pm.SetActive(false);
        om.SetActive(true);
    }


    public void SetVolume_(float volume) {
        audioMixer.SetFloat("MasterVolume", volume);
        
        if(volume == -80f) {pm.GetComponent<Image>().sprite = audiomute;}
        else if(volume <= -60f) {pm.GetComponent<Image>().sprite = audiolow;}
        else if(volume <= -30f) {pm.GetComponent<Image>().sprite = audiomedium;}
        else if(volume == 0f) {pm.GetComponent<Image>().sprite = audiohigh;}
    }
    
    public void LOLtext(string nextGame) {
        if (nextGame != "TitleScreen") {
        SaveManager.instance.Save();
        SaveManager.instance.switchedScene = true;
        }
        SceneManager.LoadScene(nextGame);
    }
}
