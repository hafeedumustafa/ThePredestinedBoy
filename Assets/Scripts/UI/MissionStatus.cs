using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionStatus : MonoBehaviour
{

    public Image[] emblems;
    public Sprite[] knownEmblems;
    public TextMeshProUGUI currentWorld;

    public Image[] Souls;

    public int World;

    void Start()
    {
        currentWorld.text = "World " + World;

        for(int i = 0; i < SaveManager.instance.activeSave.Emblems; i++) {
            emblems[i].sprite = knownEmblems[i];
        }

        for(int i = 0; i < SaveManager.instance.activeSave.Souls; i++) {
            Souls[i].gameObject.SetActive(true);
        }
    }

    public void healthAdjusted(int value)
    {
        SaveManager.instance.activeSave.Souls += value;
        
        for(int i = 0; i < SaveManager.instance.activeSave.Souls; i++) {
            Souls[i].gameObject.SetActive(true);
        }
    }

}
