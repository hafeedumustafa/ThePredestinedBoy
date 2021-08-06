using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public string WName;
    public string description;
    public int amountOfDamage;
    public int numWeapon;
    public bool hasWeapon;

    //ammo vv
    public string maxAmmo;
    public string currentAmmo;
    public string NeedsAmmo;

    //sprite vv
    public Sprite sprite;
    public float scaleX;
    public float scaleY;
    public float rotation;

}
