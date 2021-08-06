using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEssentials : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.instance.NewSceneStart();
        GameManager.instance.NewSceneStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
