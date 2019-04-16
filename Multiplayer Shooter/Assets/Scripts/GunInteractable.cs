using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(Interactable))]
public class GunInteractable : MonoBehaviour {

    private Transform gunPosition;
    private Transform dropPosition;
    private Collider triggerCollider;
    private Gun gun;
    private Interactable interactable;

	void Start () {
        gun = GetComponent<Gun>();
        interactable = GetComponent<Interactable>();
        triggerCollider = GetComponentInChildren<Collider>();
	}
	
    public void SwitchToInteractable(){
        gun.enabled = false;
        interactable.enabled = true;
        triggerCollider.enabled = true;
        gameObject.transform.position = dropPosition.position;
        gameObject.transform.rotation = dropPosition.rotation;
    }

    public void SwitchToGun(){
        gun.enabled = true;
        interactable.enabled = false;
        triggerCollider.enabled = false;
        gameObject.transform.position = gunPosition.position;
        gameObject.transform.rotation = gunPosition.rotation;
    }
}
