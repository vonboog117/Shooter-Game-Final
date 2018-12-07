using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Joystick : MonoBehaviour {

    [SerializeField] GameObject joystick;
    [SerializeField] GameObject stick;
    [SerializeField] GameObject fireButton;

    public PlayerController player;

    private bool touchBeganOnStick = false;

    void Start(){
        if (!SystemInfo.deviceModel.Contains("iPad")){
            //joystick.SetActive(false);
            //fireButton.SetActive(false);
        }
    }

    void Update(){
        if (/*player.GetReciveInput() &&*/ joystick.activeSelf && player != null){
            JoystickMove();
        }
    }

    private void JoystickMove(){
        Touch[] touches = Input.touches;
        Touch touch = new Touch();

        if (!SystemInfo.deviceModel.Contains("iPad")){
            if (touches.Length != 0){
                for (int i = 0; i < touches.Length; i++){
                    //if (touches[i].phase == TouchPhase.Began || touchBeganOnStick)
                    if ((touches[i].position.x < joystick.transform.position.x + 50 && touches[i].position.x > joystick.transform.position.x - 50) && (touches[i].position.y < joystick.transform.position.y + 50 && touches[i].position.y > joystick.transform.position.y - 50)){
                        stick.transform.position = touches[i].position;
                    }
                    //}
                }
            }
        }else{
            Debug.Log(Input.mousePosition);
        }

        RectTransform stickTransform = stick.GetComponent<RectTransform>();

        float x = stickTransform.localPosition.x;
        float y = stickTransform.localPosition.y;

        float hor = x / 50;
        float vert = y / 50;

        player.Move(hor, vert);
    }
}
