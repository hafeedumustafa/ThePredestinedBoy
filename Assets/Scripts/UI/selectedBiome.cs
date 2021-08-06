using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectedBiome : MonoBehaviour
{

    [SerializeField]
    private int currentBiome = 0;
    public Sprite[] Biomes;
    public Vector3[] scalesForCB;
    private RectTransform rectTransform;

    void Start() {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnClickMap() {
        if(currentBiome < 8){
            currentBiome++;
        } else { currentBiome = 0; }
        for (int i = 0; i < Biomes.Length;i++) {
            if(currentBiome == i) {
                gameObject.GetComponent<Image>().sprite = Biomes[i];
                rectTransform.localScale = scalesForCB[i];
            }
        }
    }
}
