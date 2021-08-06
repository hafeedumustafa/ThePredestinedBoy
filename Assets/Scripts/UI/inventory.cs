using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{

    public GameObject[] weaponSlots;
    public Button[] wsb;

    public void inventorySlotBigger(GameObject weapon) {
        RectTransform rt = weapon.GetComponent<RectTransform>();
        rt.localScale = new Vector3(rt.localScale.x + 0.035f, rt.localScale.y + 0.035f, 1f);
    }

    public void inventorySlotSmaller(GameObject weapon) {
        RectTransform rt = weapon.GetComponent<RectTransform>();
        rt.localScale = new Vector3(rt.localScale.x - 0.035f, rt.localScale.y - 0.035f, 1f);
    }

    public void ObtainingWeapon(int weaponNo, string weaponName) {
        wsb[weaponNo].interactable = true;
        weaponSlots[weaponNo].GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);
        SaveManager.instance.activeSave.weaponsObtained.Add(weaponName);
    }
}
