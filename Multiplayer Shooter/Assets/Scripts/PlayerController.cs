using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//This PlayerController class handles just about everything to do with the player, from movement, to UI, to shooting
//I think this script became too all-encompassing, I attempted to move some responsibility away into scripts like PlayerUIManager, but this script was still required to get UI elements
//All references to joystick or fingers are meant to be in a mobile application context, thought I don't think I finished successfully implementing it

public class PlayerController : NetworkBehaviour {

    public float speed;
    public float rotationSpeed;
    public int playerNumber;
    public Text cameraFingerID;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerGun;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Texture2D crosshair;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text gunText;
    [SerializeField] private Camera camera;

    private CharacterController characterController;
    private Health playerHealth;
    private Gun gun;
    private PlayerUIManager playerUIManager;
    private Joystick joystick;
    private Interactable activeInteractable = null;

    private bool isOnLadder;
    private bool recieveInput = true;

    public bool GetReciveInput() { return recieveInput; }
    public int GetJoystickTrackedTouchID() { return joystick.GetTrackedID(); }
    public PlayerUIManager GetUIManager() { return playerUIManager; }

    public void SetRecieveInput(bool shouldRecieveInput) { recieveInput = shouldRecieveInput; }

    //Called once when the script is created
    void Start(){
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<Health>();
        gun = GetComponentInChildren<Gun>();

        playerUIManager = new PlayerUIManager(healthSlider, healthText, ammoText, gunText, controlsPanel, gun);

        joystick = FindObjectOfType<Joystick>();

        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
        camera.GetComponent<CameraController>().SetPlayerGun(gun.gameObject);

        if (joystick != null){
            joystick.player = this;
        }
    }

    //Called once every frame
    void Update(){
        if (!isLocalPlayer){
            return;
        }
        
        if (recieveInput){
            float hor = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float vert = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            Move(hor, vert);

            if (Input.GetMouseButton(0)){
                //CmdSpawnBullet();
                if (gun != null){
                    Cmd_Fire();
                }
            }
            if (Input.GetKeyDown(KeyCode.R)){
                gun.Reload();
            }
            if (Input.GetKeyDown(KeyCode.E) && activeInteractable != null){
                activeInteractable.Interact(gameObject);

                if (activeInteractable.gameObject.GetComponent<Gun>() != null){
                    Cmd_PickUpGun();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q) && gun != null){
                Cmd_DropGun();
            }
        }

        if (Input.GetKeyDown(KeyCode.O)){
            recieveInput = !recieveInput;
            playerUIManager.ToggleControlsPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
            recieveInput = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) && Cursor.lockState.Equals(CursorLockMode.None)){
            Cursor.lockState = CursorLockMode.Locked;
            recieveInput = true;

            camera.GetComponent<CameraController>().SetPlayerVars(this.gameObject, playerGun);
        }
    }

    //Creates the crosshair for the player
    private void OnGUI(){
        if (isLocalPlayer){
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), crosshair);
        }
    }

    //Called when the client run by this instance is created
    //Clients are instances of the same version of a game that is connected a server or host instance, this can either be a second instance run on the same computer, or an instance run on a different computer
    //A host is considered both a client and a server, so it shares an instance with the server
    public override void OnStartLocalPlayer(){
        if (isLocalPlayer){
            camera.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(true);
        }

        //joystick.ipText.text = NetworkManager.singleton.networkAddress;
        playerNumber = NetworkServer.connections.Count;
    }

    public void Move(float hor, float vert){
        Vector3 movement = Vector3.zero;

        if (!isOnLadder){
            //float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            //float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            movement = ((camera.gameObject.transform.forward * vert) + (camera.gameObject.transform.right * hor) + new Vector3(0, -9.8f * Time.deltaTime, 0));

        }else{
            //float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            //float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            if (vert > 0){
                movement = (this.gameObject.transform.up * vert) + (camera.gameObject.transform.right * hor);
            }else if (vert < 0){
                movement = (camera.gameObject.transform.forward * vert) + (camera.gameObject.transform.right * hor) + new Vector3(0, -9.8f * Time.deltaTime, 0);
            }else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * hor;
            }else{
                movement = (this.gameObject.transform.up * -9.8f * Time.deltaTime) + (camera.gameObject.transform.right * hor);
            }
        }

        characterController.Move(movement);
    }

    //Commands are called by clients and are only ran on servers
    //ClientRPCs are called by servers and are ran on clients
    //This function is not currently used. It shoots by spawning an instance of the bullet prefab
    //From what I remember this function was successfully networked, but simply reverting this version of the project will likely not work. An earlier project from github would be best
    [Command]
    public void CmdSpawnBullet(){
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Bullet>().originObject = playerGun;

        NetworkServer.Spawn(bullet);
    }

    //This function shoots using raycasts effectively as hitscan, as opposed to spawning bullets
    [Command]
    public void Cmd_Fire(){
        //This is only  called on the server, so the host is the only instance that sees any shooting
        //The problem is that I don't know how to "spawn" raycasts nor line renderers as I did with the bullet in CmdSpawnBullet
        gun.Fire(camera);
    }

    //This function picks up the gun
    //In order to pick up the gun this function calls functions in the Gun component
    //I should have tied the Gun script and the GunInteractable script more closely together
    [Command]
    public void Cmd_PickUpGun(){
        if (gun != null){
            gun.Drop();
            gun = null;
            GetComponent<NetworkTransformChild>().enabled = false;
            playerGun.GetComponent<Despawner>().StartDespawnTimer();
            camera.GetComponent<CameraController>().SetPlayerGun(null);
        }

        activeInteractable.gameObject.GetComponent<Despawner>().StopDespawn();
        gun = activeInteractable.gameObject.GetComponent<Gun>();
        gun.Equip();
        playerGun = activeInteractable.gameObject;
        activeInteractable = null;
        GetComponent<NetworkTransformChild>().target = playerGun.transform;
        GetComponent<NetworkTransformChild>().enabled = true;
        camera.GetComponent<CameraController>().SetPlayerGun(gun.gameObject);
    }

    //This function drops the gun as well as starts the despawner
    [Command]
    public void Cmd_DropGun(){
        gun.Drop();
        gun = null;
        GetComponent<NetworkTransformChild>().enabled = false;
        playerGun.GetComponent<Despawner>().StartDespawnTimer();
        camera.GetComponent<CameraController>().SetPlayerGun(null);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = true;
        }else if (other.gameObject.GetComponent<Bullet>() != null){
            playerHealth.TakeDamage(other.gameObject.GetComponent<Bullet>().bulletDamage);
        }else if (other.gameObject.tag == "Interactable"){
            activeInteractable = other.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = false;
        }else if (other.gameObject.tag == "Interactable"){
            activeInteractable = null;
        }
    }

    
}
