﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

    public bool isTarget;
    public Text deadText;

    [SerializeField] private int maxHealth;

    private PlayerController player;
    private PlayerUIManager playerUI;
    private Text targetDamageText;

    //SyncVar sends server values to clients
    [SyncVar]
    private int currentHealth;

    public int GetCurrentHealth(){
        return currentHealth;
    }

    private void Start(){
        currentHealth = maxHealth;

        if (!isTarget){
            player = GetComponent<PlayerController>();
            playerUI = player.GetUIManager();
            playerUI.SetStartingHealthUI(maxHealth, currentHealth);
        }else{
            Text[] targetTexts = GetComponentsInChildren<Text>();
            for (int i = 0; i < targetTexts.Length; i++){
                if (targetTexts[i].gameObject.name == "DamageText"){
                    targetDamageText = targetTexts[i];
                }
            }
        }
    }

    public void TakeDamage(int damage){
        if (!isServer){return;}

        if (isTarget){
            Debug.Log(damage);
            targetDamageText.text = "Dmg: " + damage;
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
        //player.gameObject.transform.localScale = new Vector3(1f, .5f, 1f);
        Debug.Log("Dead");
    }
}
