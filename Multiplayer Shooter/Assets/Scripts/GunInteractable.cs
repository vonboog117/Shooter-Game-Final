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
    [SerializeField] private GameObject physicalObject;


    void Start () {
        gun = GetComponent<Gun>();
        triggerCollider = GetComponent<Collider>();
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
        gameObject.tag = "Interactable";
        physicalObject.tag = "Interactable";

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
        gameObject.tag = "Untagged";
        physicalObject.tag = "Untagged";

        gameObject.transform.parent = player.transform;
        gameObject.transform.position = player.transform.position + gunPosition;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = gunRotation;
        gameObject.transform.rotation = rotation;

        canInteract = false;
    }
}
