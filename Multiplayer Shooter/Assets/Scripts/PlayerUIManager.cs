using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    private Slider healthSlider;
    private Text healthText;
    private Text ammoText;
    private Gun playerGun;

    public PlayerUIManager(Slider hs, Text ht, Text at, Gun pg){
        healthSlider = hs;
        healthText = ht;
        ammoText = at;
        playerGun = pg;
    }

    public void SetStartingHealthUI(int maxHealth, int startingHealth){
        healthSlider.maxValue = maxHealth;
        ChangeHealthUI(startingHealth);
    }

    public void ChangeHealthUI(int health){
        healthSlider.value = health;
        healthText.text = "Health: " + health + " / " + healthSlider.maxValue;
    }

    public void ChangeGunUI(int ammo){

    }
}
