using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    private bool controlsActive = false;
    private Slider healthSlider;
    private Text healthText;
    private Text ammoText;
    private Text gunText;
    private GameObject controlsPanel;
    private Gun playerGun;

    public PlayerUIManager(Slider hs, Text ht, Text at, Text gt, GameObject cp, Gun pg){
        healthSlider = hs;
        healthText = ht;
        ammoText = at;
        gunText = gt;
        controlsPanel = cp;
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

    public void ChangeGunAmmoUI(int ammo, int maxAmmo){
        ammoText.text = "Ammo: " + ammo + " / " + maxAmmo;
    }

    public void ChangeGunUI(string gunName){
        gunText.text = gunName;
    }

    public void ToggleControlsPanel(){
        controlsActive = !controlsActive;
        if (controlsActive){
            controlsPanel.SetActive(true);
        }else{
            controlsPanel.SetActive(false);
        }
    }
}
