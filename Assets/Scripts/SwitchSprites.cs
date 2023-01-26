using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprites : MonoBehaviour
{
    
    public SpriteRenderer[] ObjectsOfAnim;
    public Sprite[] Sprites;
    public float FramesDifference;
    private float time = 0;
    private int currentSpriteOn = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time++;

        if(FramesDifference < time) {
            time = 0;

            if(currentSpriteOn < Sprites.Length - 1)
                currentSpriteOn++;
            else
                currentSpriteOn = 0;
            
            for(int i = 0; i < ObjectsOfAnim.Length; i++)
            {
                ObjectsOfAnim[i].sprite = Sprites[currentSpriteOn];
            }

        }

    }
}
