using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

    public int damage;
    public float damageRepeat;
    public bool canKill;

    private bool canDamage = true;

    private void OnTriggerStay(Collider other){
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null){
            if (!canKill && otherHealth.GetCurrentHealth() <= 5) { return; }
            StartCoroutine(DoDamage(otherHealth));
        }
    }

    private IEnumerator DoDamage(Health damagedObject){
        if (canDamage){
            damagedObject.TakeDamage(damage);
            canDamage = false;
            yield return new WaitForSeconds(damageRepeat);
            canDamage = true;
        }
    }
}