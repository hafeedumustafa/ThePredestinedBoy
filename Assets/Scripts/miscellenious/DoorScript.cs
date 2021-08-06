using UnityEngine;

public class DoorScript : MonoBehaviour
{
    
    private GameObject pubCollider = null;
    private bool isLocked = false;
    private bool isOpen = false;
    public Sprite normalDoor;
    public Sprite openedDoor;
    public new Collider2D collider;
    public GameObject player;
    public int DoorID;


    void Awake() {
        if(gameObject.tag == "LockedDoor") { isLocked = true;}
        if(SaveManager.instance.activeSave.keyDoorOpened.Exists(i => i == DoorID)) {
            gameObject.GetComponent<SpriteRenderer>().sprite = normalDoor;
            isLocked = false;
        }
    }

    void Update()
    {
        if(pubCollider != null) {
        if (pubCollider.tag == "Player" && Input.GetButtonDown("Interact") && isLocked == true && SaveManager.instance.activeSave.keys >= 1) {
            GameManager.instance.SetAOK(-1);
            gameObject.GetComponent<SpriteRenderer>().sprite = normalDoor;

            isLocked = false;
            SaveManager.instance.activeSave.keyDoorOpened.Add(DoorID);
        }
        else if(pubCollider.tag == "Player" && Input.GetButtonDown("Interact") && isLocked != true && isOpen == false) {
            gameObject.GetComponent<SpriteRenderer>().sprite = openedDoor;
            isOpen = true;
            collider.enabled = !collider.enabled;
            OnTriggerEnter2D(player.GetComponent<CapsuleCollider2D>());
        }
        else if(pubCollider.tag == "Player" && Input.GetButtonDown("Interact") && isLocked != true && isOpen == true) {
            gameObject.GetComponent<SpriteRenderer>().sprite = normalDoor;
            isOpen = false;
            collider.enabled = !collider.enabled;
        }
        }

    }

    void OnTriggerExit2D(Collider2D collider) {
        pubCollider = null;
    }
    void OnTriggerEnter2D(Collider2D collider) {
        pubCollider = collider.gameObject;
    }

}
