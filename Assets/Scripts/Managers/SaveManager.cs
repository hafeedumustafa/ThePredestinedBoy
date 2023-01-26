using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    public SaveData activeSave;

    public bool switchedScene = false;


    void Awake() {

        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Load();

        if (activeSave.autosave){
            StartCoroutine(AutoSave());
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Home))
        {
            DeleteData(false);
        }
    }

    public void NewSceneStart() {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + activeSave.SaveName + ".savefile")) {
            if (switchedScene == false) {
                GameManager.instance.LoadingValues();
            }
        }
        Save();
    }

    
    public void Save() {
        GameManager.instance.SavingValues();
        
        string dataPath = Application.persistentDataPath;
        var Serializer = new XmlSerializer(typeof(SaveData));
        var Stream = new FileStream(dataPath + "/" + activeSave.SaveName + ".savefile", FileMode.Create);

        Serializer.Serialize(Stream, activeSave);
        Stream.Close();

        print("saved");
    }

    public void Load() {
        string dataPath = Application.persistentDataPath;
        if (System.IO.File.Exists(dataPath + "/" + activeSave.SaveName + ".savefile")) {   
            if(GameManager.instance){
                GameManager.instance.LoadingValues();
            }
            var Serializer = new XmlSerializer(typeof(SaveData));
            var Stream = new FileStream(dataPath + "/" + activeSave.SaveName + ".savefile", FileMode.Open);

            activeSave = Serializer.Deserialize(Stream) as SaveData;
            Stream.Close();

            print("loaded");
            
        } else {
            activeSave.Init(false, false);
        }

    }

    public void DeleteData(bool reset) {
        string dataPath = Application.persistentDataPath;
        
        if (System.IO.File.Exists(dataPath + "/" + activeSave.SaveName + ".savefile")) {   
            File.Delete(Application.persistentDataPath + "/" + activeSave.SaveName + ".savefile");
            print("DATA DELETED");
            activeSave.Init(reset, true);
        }
    }

    IEnumerator AutoSave() {
        yield return new WaitForSeconds(activeSave.AutoSaveTime);
        if (activeSave.autosave == true) {
            print("auto");
            Save();
            StartCoroutine(AutoSave());
        }
    }

}

[System.Serializable]
public class SaveData
{
    public string SaveName;

    public List<int> chestOpened = new List<int>();
    public List<int> keyDoorOpened = new List<int>();

    public int Emblems;
    public string Scene;

    public Vector3 PlayersPosition;
    public float Souls;
    public int maxSouls; // max sould you can get at the time
    public int maxExtraSouls;//max extra souls that can be gained by boss' etc. is 15 for normal, 1 for Xtreme (+1 for boss fights)
    public int keys; // regular keys for doors, chests etc.
    public int Powerkeys; // for massive walls
    public int Masterkeys; // for dungeon final key, big chests
    public List<int> Specialkeys = new List<int>(); // special keys for specific chests, with ID

    public float dungeonVignette;
    public float dungeonBloom;
    public bool XTREME;//difficulty
    public int color;

    public List<string> weaponsObtained = new List<string>();
    public int selectedWeapon;

    public int arrows;
    public int maxArrows;

    public int AutoSaveTime;
    public bool autosave;

    public void Init(bool reset, bool newGame)
    {
        chestOpened = new List<int>();
        keyDoorOpened = new List<int>();
        Emblems = 0;
        Scene = null;
        PlayersPosition = new Vector3(0, 0, 0);
        keys = 0;
        Powerkeys = 0;
        Masterkeys = 0;
        Specialkeys = new List<int>();;
        dungeonVignette = 0.22f;
        dungeonBloom = 0.3f;
        XTREME = reset;
        color = 0;
        weaponsObtained = new List<string>();
        selectedWeapon = 0;
        AutoSaveTime = 300;
        autosave = true;
        arrows = 0;
        maxArrows = 0;


        if(reset) { 
            maxSouls = 1;
            maxExtraSouls = 1;
            Souls = 1;
        }
        else {
            maxSouls = 7;
            maxExtraSouls = 15;
            Souls = 4;
        }
    }

}