using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GroundExtreme : MonoBehaviour// THIS IS FOR REGULAR GROUND
{

    public List<GameObject> Ground = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        Ground = FindGameObjectsWithLayer(8, "Props");
        Color32 grass_color = new Color32(80, 7, 7, 255);
        bool xtreme = SaveManager.instance.activeSave.XTREME;


        foreach(GameObject g in Ground)
        {
            SpriteShapeRenderer ssr = g.GetComponent<SpriteShapeRenderer>();
            
            switch(xtreme)
            {
                case true when g.layer == 8 && ssr:
                    g.GetComponent<SpriteShapeRenderer>().color = grass_color;
                    break;
                case true when g.layer == 8 && !ssr:
                    g.GetComponent<SpriteRenderer>().color = grass_color;
                    break;
                case true when g.CompareTag("Props") && !ssr:
                    g.GetComponent<SpriteRenderer>().color = new Color32(50, 7, 7, 255);
                    break;
                case true when g.CompareTag("Props") && ssr:
                    float H,S,V;
                    Color.RGBToHSV(ssr.color, out H, out S, out V);
                    g.GetComponent<SpriteShapeRenderer>().color = Color.HSVToRGB(0, S, V, true);
                    break;
                case false:
                    break;
                default:
                    g.GetComponent<SpriteRenderer>().color = grass_color;
                    break;

            }

        }
    }

    List<GameObject> FindGameObjectsWithLayer (int wantedlayer, string wantedTag){
        List<GameObject> Grounds = new List<GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(true) ;


        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].layer == wantedlayer || allObjects[i].CompareTag(wantedTag) ) {
                Grounds.Add(allObjects[i]);
            }
        }

        if (Grounds.Count == 0) {
            return null;
        }

        return Grounds;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
