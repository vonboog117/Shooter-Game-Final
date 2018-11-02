using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    private CharacterController characterController;
    private Camera camera;

    private bool isOnLadder;

	void Start () {
        characterController = GetComponent<CharacterController>();
        camera = FindObjectOfType<Camera>();
        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
	}

    void Update(){
        if (!isOnLadder) { 
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            Vector3 movement = ((camera.gameObject.transform.forward * z) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0));

            characterController.Move(movement);
        }else{
            float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            Vector3 movement;

            if (y != 0){
                movement = (this.gameObject.transform.up * y) + (camera.gameObject.transform.right * x);
            }else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * x;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = true;
        }
    }
    
}
