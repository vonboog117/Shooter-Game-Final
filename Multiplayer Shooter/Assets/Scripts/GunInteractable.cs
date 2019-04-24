using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class GunInteractable : Interactable {

    public bool startAsInteractable;
    public Vector3 gunPosition;
    public Vector3 gunRotation;
    public Vector3 dropPosition;
    public Vector3 dropRotation;

    private Collider triggerCollider;
    private Gun gun;

	void Start () {
        gun = GetComponent<Gun>();
        triggerCollider = GetComponentInChildren<Collider>();
        name = gun.gunName;
        canInteract = false;

        if (startAsInteractable){
            SwitchToInteractable();
        }else{
            if (transform.parent != null){
                Interact(transform.parent.gameObject);
            }
        }
    }
	
    public void SwitchToInteractable(){
        gun.enabled = false;
        triggerCollider.enabled = true;

        gameObject.transform.position = dropPosition;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = dropRotation;
        gameObject.transform.rotation = rotation;
        gameObject.transform.parent = null;

    }

    //public void SwitchToGun(GameObject player){
    //    gun.enabled = true;
    //    triggerCollider.enabled = false;

    //    gameObject.transform.parent = player.transform;
    //    gameObject.transform.position = gunPosition;
    //    Quaternion rotation = new Quaternion();
    //    rotation.eulerAngles = gunRotation;
    //    gameObject.transform.rotation = rotation;
    //}

    public override void Interact(GameObject player){
        gun.enabled = true;
        triggerCollider.enabled = false;

        gameObject.transform.parent = player.transform;
        gameObject.transform.position = gunPosition;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = gunRotation;
        gameObject.transform.rotation = rotation;

        canInteract = false;
    }
}
