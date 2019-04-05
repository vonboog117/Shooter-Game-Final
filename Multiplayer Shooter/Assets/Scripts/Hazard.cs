using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

    public int damage;
    public float damageRepeat;
    public bool canKill;

    private void OnTriggerStay(Collider other){
        Debug.Log("Ahhhhh Lava!");
    }
}
