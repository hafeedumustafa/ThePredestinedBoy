using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public string ZoomStatus = "not zoomed";

    public CinemachineVirtualCamera maincamera;
    public Transform player;
    public float speed; 
    public float LerpValueI;
    public float LerpValueII;
    float time = 0.0f;
    public Transform SavedCameraPosition;

    void Start()
    {
        LerpValueI = gameObject.GetComponent<Camera>().orthographicSize;
    }

    void LateUpdate() {
        if (ZoomStatus == "zooming in") {//5.0, 1.75
            maincamera.m_Lens.OrthographicSize = Mathf.Lerp(LerpValueI, LerpValueII, time);
            maincamera.transform.position = new Vector3( Mathf.Lerp(SavedCameraPosition.position.x, player.position.x, time), Mathf.Lerp(SavedCameraPosition.position.y, player.position.y, time), transform.position.z );

            time += speed;

            if(time > 1.05f) {
                time = 0.0f;
                ZoomStatus = "stopped zooming";
            }
        
        }

        else if(ZoomStatus == "zooming out") {//1.75, 5.0
            maincamera.m_Lens.OrthographicSize = Mathf.Lerp(LerpValueII, LerpValueI, time);
            maincamera.transform.position = new Vector3( Mathf.Lerp(player.position.x, SavedCameraPosition.position.x, time), Mathf.Lerp(player.position.y, SavedCameraPosition.position.y, time), transform.position.z );
            
            time += speed;

            if(time > 1.05f) {
                time = 0.0f;
                ZoomStatus = "not zoomed";
            }
            
        }
    }

    public void SaveCameraPosition() {
        SavedCameraPosition.position = transform.position;
    }
}
