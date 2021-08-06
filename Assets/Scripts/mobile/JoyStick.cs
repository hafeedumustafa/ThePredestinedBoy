using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    public RectTransform Stick;
    private const float Radius = 69f;
    private Vector3 joystickPos;
    private Vector3 StickPos;
    Vector3 NewStickPos;
    private GameObject Player;
    private bool speedIncreased = false;
    private Vector3 Direction;
    float JoystickArea;
    Vector2 MousePos;

    public Transform OriginalMousePosition;

    void Start() 
    {
        joystickPos = GetComponent<RectTransform>().position;
        Player = GameManager.instance.Player;
    }

    void Update()
    {
    }
    
    public void DragStick()
    {
        Direction = Input.mousePosition - joystickPos;

        NewStickPos = Vector3.ClampMagnitude(Direction, Radius);

        MousePos = new Vector2(Camera.main.pixelWidth / 10 + Input.mousePosition.x, Camera.main.pixelHeight / 2 + Input.mousePosition.y);
        JoystickArea = Camera.main.pixelWidth / 2;

        if(MousePos.x < JoystickArea) {
        Stick.position = joystickPos + new Vector3(NewStickPos.x, NewStickPos.y, 0f);

        if (Player.TryGetComponent(out Movement movement)) {
                if(NewStickPos != ClampMagnitude(5f)) {
                    if(NewStickPos.x > 0) {
                        movement.HorizontalMovement = 1;
                    } else if(NewStickPos.x < 0) {
                        movement.HorizontalMovement = -1;
                    }
                    if(NewStickPos.y > 0) {
                        movement.VerticalMovement = 1;
                    } else if(NewStickPos.y < 0) {
                        movement.VerticalMovement = -1;
                    }
                if (NewStickPos != ClampMagnitude(40f) && speedIncreased == false)
                {
                    speedIncreased = true;
                    movement.speed += 2;
                } else if(NewStickPos == ClampMagnitude(40f) && speedIncreased == true){
                    speedIncreased = false;
                    movement.speed -= 2;
                }
            }
        }

        Vector3 TopdownMove = Vector3.ClampMagnitude(NewStickPos, 1);

        if (Player.TryGetComponent(out TDmovement tdmovement)) {
                
            tdmovement.HorizontalMovement = TopdownMove.x;
            tdmovement.VerticalMovement = TopdownMove.y;
        
        if (NewStickPos != ClampMagnitude(60f) && speedIncreased == false){
            speedIncreased = true;
            tdmovement.m_speed += 2;
        } else if(NewStickPos == ClampMagnitude(60f) && speedIncreased == true){
            speedIncreased = false;
            tdmovement.m_speed -= 2;
            }
        }
    
        }   
    }

    
/*
    private bool StickLessRad() {
        Mathf.Clamp();
    }*/

    private Vector3 ClampMagnitude(float Radius) => Vector3.ClampMagnitude(Direction, Radius);
    // reference for radius:
    // ClampMagnituding 40f == WalkingArea
    // ClampMagnituding 5f == DeadZone 
    

    public void DropStick()
    {
        Stick.position = joystickPos;
        NewStickPos = new Vector3(0f, 0f, 0f);

        if (Player.TryGetComponent(out Movement movement)) {

            movement.speed = movement.initialspeed;
            speedIncreased = false;

            movement.HorizontalMovement = 0;
            movement.VerticalMovement = 0;
        }
        if (Player.TryGetComponent(out TDmovement tdmovement)) {
            tdmovement.m_speed = tdmovement.m_InitialSpeed;
            speedIncreased = false;

            tdmovement.HorizontalMovement = 0;
            tdmovement.VerticalMovement = 0;
        }
    }
}
