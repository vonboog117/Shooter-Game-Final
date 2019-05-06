using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInteractable : Interactable {

    public int health;

    public override void Interact(GameObject player){
        despawner.StartDespawnTimer();
        Health pHealth = player.GetComponent<Health>();
        if (pHealth != null){
            pHealth.Heal(health);
        }
    }
}
