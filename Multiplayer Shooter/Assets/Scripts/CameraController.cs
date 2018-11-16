using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

    public float verticalSensitivity;
    public float horizontalSensitivity;

    public float minVertRotation = -45.0f;
    public float maxVertRotation = 45.0f;

    public float xRotation = 0;

    private GameObject player;
    private GameObject playerGun;
    public void SetPlayer(GameObject p){
        player = p;
    }
    public void SetPlayerGun(GameObject g){
        playerGun = g;
    }

    private void LateUpdate()
    {
        if (!NetworkClient.active){
            return;
        }
        Move();
        Rotate();
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
        float yRotation = transform.localEulerAngles.y + delta;

        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);

        player.transform.localEulerAngles = new Vector3(0, yRotation, 0);
        playerGun.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
    }
}
