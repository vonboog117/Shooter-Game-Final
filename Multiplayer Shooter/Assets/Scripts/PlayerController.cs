﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float rotationSpeed;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerGun;
    [SerializeField] private GameObject bulletSpawn;

    private CharacterController characterController;
    private Camera camera;
    private Health playerHealth;

    private bool isOnLadder;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        camera = FindObjectOfType<Camera>();
        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
        camera.GetComponent<CameraController>().SetPlayerGun(playerGun);
        playerHealth = GetComponent<Health>();
	}

    void Update(){
        Move();

        if (Input.GetMouseButtonDown(0)){
            SpawnBullet();
        }
    }

    private void Move(){
        Vector3 movement = Vector3.zero;

        if (!isOnLadder){
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            movement = ((camera.gameObject.transform.forward * z) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0));

        }
        else{
            float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            if (y > 0){
                movement = (this.gameObject.transform.up * y) + (camera.gameObject.transform.right * x);
            }
            else if (y < 0){
                movement = (camera.gameObject.transform.forward * y) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * x;
            }
            else{
                movement = (this.gameObject.transform.up * -9.8f * Time.deltaTime) + (camera.gameObject.transform.right * x);
            }
        }

        characterController.Move(movement);
    }

    private void SpawnBullet(){
        if (bulletPrefab != null){
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;
            bullet.GetComponent<Bullet>().originObject = playerGun;
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = true;
        }else if (other.gameObject.GetComponent<Bullet>() != null){
            playerHealth.TakeDamage(other.gameObject.GetComponent<Bullet>().bulletDamage);
            Debug.Log(playerHealth.GetCurrentHealth());
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = false;
        }
    }
    
}
