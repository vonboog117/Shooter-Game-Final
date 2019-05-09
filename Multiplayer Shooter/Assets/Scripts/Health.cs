﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public bool isTarget;

    [SerializeField] private int maxHealth;

    private PlayerController player;
    private PlayerUIManager playerUI;

    //SyncVar sends server values to clients
    [SyncVar]
    private int currentHealth;

    public int GetCurrentHealth(){
        return currentHealth;
    }

    private void Start(){
        currentHealth = maxHealth;
        player = GetComponent<PlayerController>();
        playerUI = player.GetUIManager();
        playerUI.SetStartingHealthUI(maxHealth, currentHealth);
    }

    public void TakeDamage(int damage){
        if (!isServer){return;}

        if (isTarget){
            Debug.Log(damage);
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0){
            currentHealth = 0;
            OnHeathZero();
        }

        playerUI.ChangeHealthUI(currentHealth);
    }

    public void Heal(int health){
        if (!isServer){
            return;
        }

        currentHealth += health;

        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

        playerUI.ChangeHealthUI(currentHealth);
    }

    private void OnHeathZero(){
        player.SetRecieveInput(false);
        Debug.Log("Dead");
    }
}
