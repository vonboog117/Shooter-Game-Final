using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] GameObject joystick;
    [SerializeField] GameObject fireButton;

    public PlayerController player;

	void Start () {
        if (!SystemInfo.deviceModel.Contains("iPad")){
            joystick.SetActive(false);
            fireButton.SetActive(false);
        }
	}
	
	void Update () {
        if (player.GetReciveInput()){
            Joystick();
        }
	}

    private void Joystick(){
    }
}
