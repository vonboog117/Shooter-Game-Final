using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour {

    public float verticalSensitivity;
    public float horizontalSensitivity;

    public float minVertRotation = -45.0f;
    public float maxVertRotation = 45.0f;

    public float xRotation = 0;
    public float yRotation = 0;

    private GameObject player;
    private GameObject playerGun;

    public void SetPlayer(GameObject p){player = p;}
    public void SetPlayerGun(GameObject g){playerGun = g;}

    private Vector3 firstPoint;
    private Vector3 secondPoint;

    private float xAngle;
    private float yAngle;
    private float xAngleTemp;
    private float yAngleTemp;

    private int trackedTouchID = -1;

    private void Start()
    {
        //Initialization our angles of camera
        xAngle = 0f;
        yAngle = 0f;
        this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0f);
    }

    private void LateUpdate(){
        if (!NetworkClient.active){
            return;
        }

        if (player != null && player.GetComponent<PlayerController>() != null && player.GetComponent<PlayerController>().isLocalPlayer && player.GetComponent<PlayerController>().GetReciveInput()){
            //Move();
            if (!SystemInfo.deviceModel.Contains("iPad")){
                Rotate();
            }else{
                if (trackedTouchID == -1){
                    FindTouchToTrack();
                }else{
                    RotateIOS();
                }
            }

            player.GetComponent<PlayerController>().cameraFingerID.text = "Camera ID: " + trackedTouchID;
        }
    }

    void Move(){
        if (player != null){
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z);
        }
    }

    void Rotate(){
        xRotation -= Input.GetAxis("Mouse Y") * verticalSensitivity;
        xRotation = Mathf.Clamp(xRotation, minVertRotation, maxVertRotation);

        float delta = Input.GetAxis("Mouse X") * horizontalSensitivity;
        yRotation += delta;

        transform.localEulerAngles = new Vector3(xRotation, 0, 0);

        if (player != null){
            player.transform.localEulerAngles = new Vector3(0, yRotation, 0);
        }
        if (playerGun != null){
            playerGun.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
        }
    }

    void RotateIOS(){
        Touch touch = new Touch();

        for (int i = 0; i < Input.touches.Length; i++){
            if (Input.touches[i].fingerId == trackedTouchID){
                touch = Input.touches[i];
            }else{
                trackedTouchID = -1;
            }
        }

        if (trackedTouchID != -1){
            if (touch.phase == TouchPhase.Began){
                firstPoint = touch.position;
                xAngleTemp = xAngle;
                yAngleTemp = yAngle;
            }if (touch.phase == TouchPhase.Moved){
                secondPoint = touch.position;

                xAngle = xAngleTemp + (secondPoint.x - firstPoint.x) * 180 / Screen.width;
                yAngle = yAngleTemp - (secondPoint.y - firstPoint.y) * 90 / Screen.height;

                yAngle = Mathf.Clamp(yAngle, minVertRotation, maxVertRotation);

                gameObject.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
            }
        }
    }

    private void FindTouchToTrack(){
        if (Input.touchCount > 0){
            for (int i = 0; i < Input.touches.Length; i++){
                if (Input.touches[i].fingerId != player.GetComponent<PlayerController>().GetJoystickTrackedTouchID()){
                    trackedTouchID = Input.touches[i].fingerId;
                }
            }
        }
    }

    public void SetPlayerVars(GameObject p, GameObject g){
        SetPlayer(p);
        SetPlayerGun(g);
    }
}
