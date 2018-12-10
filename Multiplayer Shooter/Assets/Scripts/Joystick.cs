using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Joystick : MonoBehaviour {

    [SerializeField] GameObject joystick;
    [SerializeField] GameObject stick;
    [SerializeField] GameObject fireButton;

    public PlayerController player;

    private int trackedTouchID = -1;

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

        if (!SystemInfo.deviceModel.Contains("iPad")){
            if (trackedTouchID == -1){
                FindTouchOnJoystick(touches);
            }else{
                MoveStick(touches);
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

    private void FindTouchOnJoystick(Touch[] touches){
        if (touches.Length != 0){
            for (int i = 0; i < touches.Length; i++){
                if ((touches[i].position.x < joystick.transform.position.x + 50 && touches[i].position.x > joystick.transform.position.x - 50) && (touches[i].position.y < joystick.transform.position.y + 50 && touches[i].position.y > joystick.transform.position.y - 50)){
                    if (touches[i].phase == TouchPhase.Began){
                        trackedTouchID = touches[i].fingerId;
                    }
                }
            }
        }
    }

    private void MoveStick(Touch[] touches)
    {
        Touch trackedTouch = new Touch();

        for (int i = 0; i < touches.Length; i++)
        {
            if (touches[i].fingerId == trackedTouchID)
            {
                trackedTouch = touches[i];
            }
        }

        int config = 0;

        if ((trackedTouch.position.x < joystick.transform.position.x + 50 && trackedTouch.position.x > joystick.transform.position.x - 50)){

        }else if(trackedTouch.position.x > joystick.transform.position.x + 50){
            config = 1;
        }else if (trackedTouch.position.x < joystick.transform.position.x - 50){
            config = 2;
        }

        if ((trackedTouch.position.y < joystick.transform.position.y + 50 && trackedTouch.position.y > joystick.transform.position.y - 50)){
            yOn = true;
        }else{
            yOn = false;
        }

       

    }
}
