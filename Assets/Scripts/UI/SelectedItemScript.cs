using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemScript : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI panelName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI ammo;
    public TextMeshProUGUI damage;

    public Image SISprite;
    public RectTransform SItransform;
    public GameObject SIB;

    private Inventory PCSI;

    public void OnClickOnWS(Inventory currentSelectedItem) // SI = selected item// CSI = Currently selected item //PCSI = public currently selected item
    {
        panelName.text = null;
        description.text = null;
        ammo.text = null;
        damage.text = null;
        SISprite.sprite = null;
        SISprite.color = new Color32(0, 0, 0, 0);
        SItransform.localScale = new Vector3(0.128f, 0.035f, 1f);
        SItransform.eulerAngles = new Vector3(0, 0, 0);

        PCSI = currentSelectedItem;
        panelName.text = "Name: " + currentSelectedItem.WName;
        description.text = "Description: " + currentSelectedItem.description;
        ammo.text = "Ammo: " + currentSelectedItem.NeedsAmmo + "\nCurrent Ammo: " + currentSelectedItem.currentAmmo + "\nMaxAmmo: " + currentSelectedItem.maxAmmo;
        damage.text = "Damage: " + currentSelectedItem.amountOfDamage;
        SISprite.sprite = currentSelectedItem.sprite;
        SISprite.color = new Color32(255, 255, 255, 255);
        SItransform.localScale = new Vector3(currentSelectedItem.scaleX, currentSelectedItem.scaleY, 1f);
        SItransform.eulerAngles = new Vector3(0, 0, currentSelectedItem.rotation);
        SIB.SetActive(true);
    }

    public void onClickSI() {
        SaveManager.instance.activeSave.selectedWeapon = PCSI.numWeapon;
        animator.SetInteger("ItemEquiped", PCSI.numWeapon);
    }
}
