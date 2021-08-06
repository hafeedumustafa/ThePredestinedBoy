using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    
    private Scene scene;
    public GameObject fm1;
    public GameObject f0;
    public GameObject f1;
    public GameObject f2;

    void Awake() {
        scene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        if (scene.name == "gettingASword") {
            fm1.SetActive(false);
            f0.SetActive(false);
            f1.SetActive(false);
            f2.SetActive(false);
        }
        //check for all other scenes
    }


    void Update()
    {
        
    }
}
