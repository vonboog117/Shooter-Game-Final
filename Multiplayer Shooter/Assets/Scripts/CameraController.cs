using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float verticalSensitivity;
    public float horizontalSensitivity;

    public float minVertRotation = -45.0f;
    public float maxVertRotation = 45.0f;

    public float xRotation = 0;

    private GameObject player;
    public void SetPlayer(GameObject p){
        player = p;
    }

    private void LateUpdate()
    {
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
        float rotationY = transform.localEulerAngles.y + delta;

        transform.localEulerAngles = new Vector3(xRotation, rotationY, 0);
    }
}
