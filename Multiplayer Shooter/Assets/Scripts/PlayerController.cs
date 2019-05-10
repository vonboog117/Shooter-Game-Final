using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

    public float speed;
    public float rotationSpeed;
    public int playerNumber;
    public Text cameraFingerID;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerGun;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private Texture2D crosshair;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text gunText;

    private CharacterController characterController;
    private Camera camera;
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

    void Start(){
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<Health>();
        gun = GetComponentInChildren<Gun>();

        playerUIManager = new PlayerUIManager(healthSlider, healthText, ammoText, gunText, gun);

        camera = GetComponentInChildren<Camera>();
        joystick = FindObjectOfType<Joystick>();

        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
        camera.GetComponent<CameraController>().SetPlayerGun(gun.gameObject);

        if (joystick != null){
            joystick.player = this;
        }
    }

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
                //This statement needs to be generalized to allow for other types of interactables to be used
                //All the gun specific code needs to either be move to its own fuction or moved to the gun or guninteractable scripts
                activeInteractable.Interact(gameObject);

                if (activeInteractable.gameObject.GetComponent<Gun>() != null){
                    PickUpGun();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q) && gun != null){
                DropGun();
            }
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

    private void OnGUI(){
        if (isLocalPlayer){
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), crosshair);
        }
    }

    public override void OnStartLocalPlayer(){
        if (isLocalPlayer){
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(true);
        }else{
            camera.gameObject.SetActive(false);
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
    [Command]
    public void CmdSpawnBullet(){
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Bullet>().originObject = playerGun;

        NetworkServer.Spawn(bullet);
    }

    //There is only one camera in the scene, because the server is the one calling this the raycast will always go where the server's camera is facing
    //Either add cameras as children of players, or find a way for clients to create the raycast
    [Command]
    public void Cmd_Fire(){
        gun.Fire(camera);
    }

    public void PickUpGun(){
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

    public void DropGun(){
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
