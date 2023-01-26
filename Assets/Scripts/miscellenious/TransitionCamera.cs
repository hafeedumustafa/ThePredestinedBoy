using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    public Vector3 newPositionA, newPositionB;
    public Vector3 pushPlayerA, pushPlayerB;
    public CameraController CC;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider) {

        if(collider.gameObject.CompareTag("Player") && collider.isTrigger == false){
            collider.gameObject.GetComponent<PlayerManagerTD>().CanMove = false;

            if(CC.maincamera.transform.position == newPositionA){ 
                CC.TransitionCamera(newPositionB);

                StartCoroutine
                    (pushPlayer(player.transform.position + pushPlayerB, player.transform.position, 0));

            } else {
                CC.TransitionCamera(newPositionA);

                StartCoroutine
                    (pushPlayer(player.transform.position + pushPlayerA, player.transform.position, 0));

            }
        }

    }

    IEnumerator pushPlayer(Vector3 finalPos, Vector3 initialPos, float time) {

        time += 0.05f;
        player.transform.position = 
        new Vector3(
            Mathf.Lerp(initialPos.x, finalPos.x, time), 
            Mathf.Lerp(initialPos.y, finalPos.y, time), 
            player.transform.position.z);

        if(time < 1) {
            yield return new WaitForFixedUpdate();
            StartCoroutine(pushPlayer(finalPos, initialPos, time));
        } else {
            
        }

    }


}
