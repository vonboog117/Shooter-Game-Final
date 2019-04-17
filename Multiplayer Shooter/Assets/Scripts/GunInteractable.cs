using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(Interactable))]
public class GunInteractable : MonoBehaviour {

    public bool startAsInteractable;
    public Vector3 gunPosition;
    public Vector3 gunRotation;
    public Vector3 dropPosition;
    public Vector3 dropRotation;

    private Collider triggerCollider;
    private Gun gun;
    private Interactable interactable;

	void Start () {
        gun = GetComponent<Gun>();
        interactable = GetComponent<Interactable>();
        triggerCollider = GetComponentInChildren<Collider>();

        if (startAsInteractable){
            SwitchToInteractable();
        }else{
            if (transform.parent != null){
                SwitchToGun(transform.parent.gameObject);
            }
        }
    }
	
    public void SwitchToInteractable(){
        gun.enabled = false;
        interactable.enabled = true;
        triggerCollider.enabled = true;

        gameObject.transform.position = dropPosition;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = dropRotation;
        gameObject.transform.rotation = rotation;
        gameObject.transform.parent = null;

    }

    public void SwitchToGun(GameObject player){
        gun.enabled = true;
        interactable.enabled = false;
        triggerCollider.enabled = false;

        gameObject.transform.parent = player.transform;
        gameObject.transform.position = gunPosition;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = gunRotation;
        gameObject.transform.rotation = rotation;
    }
}
