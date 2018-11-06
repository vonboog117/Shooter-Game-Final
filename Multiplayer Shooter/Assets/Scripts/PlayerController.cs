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
        Vector3 movement = Vector3.zero;

        if (!isOnLadder) { 
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            movement = ((camera.gameObject.transform.forward * z) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0));

        }else{
            float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            if (y != 0){
                movement = (this.gameObject.transform.up * y) + (camera.gameObject.transform.right * x);
            }else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * x;
            }else{
                movement = (this.gameObject.transform.up * -9.8f * Time.deltaTime) + (camera.gameObject.transform.right * x);
            }
        }

        characterController.Move(movement);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = false;
        }
    }

}
