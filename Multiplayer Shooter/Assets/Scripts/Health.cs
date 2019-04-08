using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public bool isTarget;

    [SerializeField] private int maxHealth;

    private PlayerController player;
    private PlayerUIManager playerUI;

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
        playerUI.ChangeHealthUI(currentHealth);
        Debug.Log(currentHealth);

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
        player.SetRecieveInput(false);
        Debug.Log("Dead");
    }
}
