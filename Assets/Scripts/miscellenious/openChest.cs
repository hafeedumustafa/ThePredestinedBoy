using UnityEngine;
using System.Collections;

public class openChest : MonoBehaviour
{

    public Sprite chestopen;
    public TDmovement tdmovement;
    public inventory inventory;
    public CameraController CC;
    public GameObject CanInteract;

    public enum chestType 
    {
        Weapon,
        Key,
        Consumable
    }
    public chestType ChestType;

    public string chestContains;
    private bool isOpen;
    public int chestNo;
    public int weaponNo;

    void Start() {
        if(SaveManager.instance.activeSave.chestOpened.Exists(i => i == chestNo)) {
            isOpen = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = chestopen;
            CanInteract.SetActive(false);
        }
    }

    public void OpenChest() {

        if(isOpen == false) {
            isOpen = true;
            CanInteract.SetActive(false);

            SaveManager.instance.activeSave.chestOpened.Add(chestNo);
            SaveManager.instance.activeSave.chestOpened.Sort();

            gameObject.GetComponent<SpriteRenderer>().sprite = chestopen;
            tdmovement.OnChestOpen(chestNo);

            CC.SaveCameraPosition();
            CC.LerpValueII = 1.75f;
            CC.speed = 0.01f;
            CC.ZoomStatus = "zooming in";

            switch(ChestType) {
                case chestType.Weapon:
                inventory.ObtainingWeapon(weaponNo, chestContains);
                break;

                case chestType.Key:
                GameManager.instance.SetAOK(1);
                break;

            }
        }
    }
}
