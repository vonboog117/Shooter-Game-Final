using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour {

    protected string name;
    protected bool canInteract;

    //protected abstract void Init();

    public abstract void Interact(GameObject player);

    public void OnInteractableTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<PlayerController>() != null){
            canInteract = true;
        }
    }

    public void OnInteractableTriggerExit(Collider other){
        if (other.gameObject.GetComponent<PlayerController>() != null){
            canInteract = false;
        }
    }
}
