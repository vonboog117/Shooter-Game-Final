using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour {

    private GunInteractable parent;

	void Start () {
        parent = GetComponentInParent<GunInteractable>();	
	}

    private void OnTriggerEnter(Collider other){
        parent.OnInteractableTriggerEnter(other);
    }
}
