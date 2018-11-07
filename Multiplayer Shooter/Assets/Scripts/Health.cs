using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private int maxHealth;
    private int currentHealth;

    public int GetCurrentHealth(){
        return currentHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        if (currentHealth <= 0){
            currentHealth = 0;
            OnHeathZero();
        }
    }

    public void Heal(int health){
        currentHealth += health;

        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
    }

    private void OnHeathZero(){

    }
}
