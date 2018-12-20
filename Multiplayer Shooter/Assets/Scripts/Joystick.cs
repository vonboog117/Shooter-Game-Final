using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Joystick : MonoBehaviour {

    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject stick;
    [SerializeField] private GameObject fireButton;

    private RectTransform joystickRect;
    private RectTransform stickRect;
    private RectTransform fireRect;

    public PlayerController player;
    public Text fingerID;
    //public Text joystickConfig;

    private int trackedTouchID = -1;

    private bool trackMousePosition = false;

    public int GetTrackedID() { return trackedTouchID; }

    void Start(){
        if (!SystemInfo.deviceModel.Contains("iPad")){
            joystick.SetActive(false);
            fireButton.SetActive(false);
        }

        joystickRect = joystick.GetComponent<RectTransform>();
        joystickRect.sizeDelta = new Vector2(Screen.height / 6, Screen.height / 6);

        stickRect = stick.GetComponent<RectTransform>();
        stickRect.sizeDelta = joystickRect.sizeDelta / 2;

        fireRect = fireButton.GetComponent<RectTransform>();
        fireRect.sizeDelta = new Vector2(Screen.height / 6, Screen.height / 6);

        joystickRect.anchoredPosition = new Vector2(110 + (joystickRect.sizeDelta.x / 2), 100 + (joystickRect.sizeDelta.x / 2));
        fireRect.anchoredPosition = new Vector2(-110 - (fireRect.sizeDelta.x / 2), 100 + (fireRect.sizeDelta.x / 2));
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

        fingerID.text = "Joystick ID: " + trackedTouchID;

        if (trackMousePosition || trackedTouchID != -1){

            float x = stickRect.localPosition.x;
            float y = stickRect.localPosition.y;

            float hor = x / (joystickRect.sizeDelta.x / 2);
            float vert = y / (joystickRect.sizeDelta.y / 2);

            hor = hor * player.speed * Time.deltaTime;
            vert = vert * player.speed * Time.deltaTime;

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
            }else{
                trackedTouchID = -1;
            }
        }

        if (trackedTouchID == -1){
            trackedTouch.position = new Vector3(joystick.transform.position.x, joystick.transform.position.y);
        }else{
            if (trackedTouch.position.x < joystick.transform.position.x - (joystickRect.sizeDelta.x / 2)){
                if (trackedTouch.position.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
                }else if (trackedTouch.position.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
                }else{
                    stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), trackedTouch.position.y, 0);
                }
            }else if (trackedTouch.position.x > joystick.transform.position.x + (joystickRect.sizeDelta.x / 2)){
                if (trackedTouch.position.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
                }else if (trackedTouch.position.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
                }else{
                    stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), trackedTouch.position.y, 0);
                }
            }else{
                if (trackedTouch.position.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(trackedTouch.position.x, joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
                }else if (trackedTouch.position.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                    stick.transform.position = new Vector3(trackedTouch.position.x, joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
                }else{
                    stick.transform.position = new Vector3(trackedTouch.position.x, trackedTouch.position.y, 0);
                }
            }
        }
    }

    private void MoveStick(){
        if (Input.mousePosition.x < joystick.transform.position.x - (joystickRect.sizeDelta.x / 2)){
            if (Input.mousePosition.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x - (joystickRect.sizeDelta.x / 2), Input.mousePosition.y, 0);
            }
        }else if (Input.mousePosition.x > joystick.transform.position.x + (joystickRect.sizeDelta.x / 2)){
            if (Input.mousePosition.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
            }else{
                stick.transform.position = new Vector3(joystick.transform.position.x + (joystickRect.sizeDelta.x / 2), Input.mousePosition.y, 0);
            }
        }else{
            if (Input.mousePosition.y < joystick.transform.position.y - (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(Input.mousePosition.x, joystick.transform.position.y - (joystickRect.sizeDelta.x / 2), 0);
            }else if (Input.mousePosition.y > joystick.transform.position.y + (joystickRect.sizeDelta.x / 2)){
                stick.transform.position = new Vector3(Input.mousePosition.x, joystick.transform.position.y + (joystickRect.sizeDelta.x / 2), 0);
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

        if ((pos.x > joystick.transform.position.x - (joystickRect.sizeDelta.x / 2) && pos.x < joystick.transform.position.x + (joystickRect.sizeDelta.x / 2)) && (pos.y > joystick.transform.position.y - (joystickRect.sizeDelta.x / 2) && pos.y < joystick.transform.position.y + (joystickRect.sizeDelta.x / 2))){
            isOnJoystick = true;
        }

        return isOnJoystick;
    }
}
