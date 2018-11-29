using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    [SerializeField] private int maxHealth;

    [SyncVar]
    private int currentHealth;

    public int GetCurrentHealth(){
        return currentHealth;
    }

    private void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        if (!isServer){
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0){
            currentHealth = 0;
            OnHeathZero();
        }
    }

    public void Heal(int health){
        if (!isServer){
            return;
        }

        currentHealth += health;

        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
    }

    private void OnHeathZero(){
        Debug.Log("Player Dead");
    }
}
