﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Despawner))]
public abstract class Interactable: MonoBehaviour {

    protected string name;
    protected bool canInteract;
    protected Despawner despawner;

    void Start(){
        despawner = GetComponent<Despawner>();
    }

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
