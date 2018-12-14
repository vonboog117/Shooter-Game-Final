using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Joystick : MonoBehaviour {

    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject fireButton;
    [SerializeField] public Text ipText;

    public PlayerController player;

    private int trackedTouchID = -1;

    private bool trackMousePosition = false;

    public int GetTrackedID() { return trackedTouchID; }

    void Start(){
        if (!SystemInfo.deviceModel.Contains("iPad")){
            joystick.SetActive(false);
            fireButton.SetActive(false);
        }
    }

    void Update(){
        if (player.GetReciveInput() && joystick.activeSelf && player != null){
            JoystickMove();
        }

    }

    private void JoystickMove(){
        Touch[] touches = Input.touches;
        if (SystemInfo.deviceModel.Contains("iPad")){
            if (trackedTouchID == -1){
                FindTouchOnJoystick(touches);
            }else{
                MoveStickIOS(touches);
            }
        }else{
            if (!trackMousePosition){
                if (Input.GetMouseButton(0) && GetIsOnJoystick(Input.mousePosition)){
                    trackMousePosition = true;
                }
            }else{
                MoveStick();
            }
        }

        if (trackMousePosition || trackedTouchID != -1){
            RectTransform stickTransform = stick.GetComponent<RectTransform>();

            float x = stickTransform.localPosition.x;
            float y = stickTransform.localPosition.y;

            float hor = x / 50;
            float vert = y / 50;


            player.Move(hor, vert);
        }
    }

    private void FindTouchOnJoystick(Touch[] touches){
        if (touches.Length != 0){
            for (int i = 0; i < touches.Length; i++){
                if (GetIsOnJoystick(touches[i].position)){
                    if (touches[i].phase == TouchPhase.Began){
                        trackedTouchID = touches[i].fingerId;
                    }
                }
            }
        }
    }

    private void MoveStickIOS(Touch[] touches){
        Touch trackedTouch = new Touch();

        for (int i = 0; i < touches.Length; i++){
            if (touches[i].fingerId == trackedTouchID){
                trackedTouch = touches[i];
            }
        }

        if (trackedTouch.fingerId != trackedTouchID){
            trackedTouchID = -1;
            trackedTouch.position = new Vector3(joystick.transform.position.x, joystick.transform.position.y);
        }

        if (trackedTouch.position.x < joystick.transform.position.x - 50){
            if (trackedTouch.position.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, joystick.transform.position.y - 50, 0);
            }else if (trackedTouch.position.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, trackedTouch.position.y, 0);
            }
        }
        else if (trackedTouch.position.x > joystick.transform.position.x + 50){
            if (trackedTouch.position.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, joystick.transform.position.y - 50, 0);
            }else if (trackedTouch.position.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, trackedTouch.position.y, 0);
            }
        }
        else{
            if (trackedTouch.position.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(trackedTouch.position.x, joystick.transform.position.y - 50, 0);
            }else if (trackedTouch.position.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(trackedTouch.position.x, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(trackedTouch.position.x, trackedTouch.position.y, 0);
            }
        }
    }

    private void MoveStick(){
        if (Input.mousePosition.x < joystick.transform.position.x - 50){
            if (Input.mousePosition.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, joystick.transform.position.y - 50, 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x - 50, Input.mousePosition.y, 0);
            }
        }else if (Input.mousePosition.x > joystick.transform.position.x + 50){
            if (Input.mousePosition.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, joystick.transform.position.y - 50, 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x + 50, Input.mousePosition.y, 0);
            }
        }else{
            if (Input.mousePosition.y < joystick.transform.position.y - 50){
                stick.transform.position = new Vector3(Input.mousePosition.x, joystick.transform.position.y - 50, 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + 50){
                stick.transform.position = new Vector3(Input.mousePosition.x, joystick.transform.position.y + 50, 0);
            }else{
                stick.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

        if (Input.GetMouseButtonUp(0)){
            trackMousePosition = false;
            stick.transform.position = new Vector3(joystick.transform.position.x, joystick.transform.position.y, 0);
        }
    }

    private bool GetIsOnJoystick(Vector3 pos){
        bool isOnJoystick = false;

        if ((pos.x > joystick.transform.position.x - 50 && pos.x < joystick.transform.position.x + 50) && (pos.y > joystick.transform.position.y - 50 && pos.y < joystick.transform.position.y + 50)){
            isOnJoystick = true;
        }

        return isOnJoystick;
    }
}
