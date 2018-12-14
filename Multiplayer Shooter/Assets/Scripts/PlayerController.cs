using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speed;
    public float rotationSpeed;
    public int playerNumber;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject playerGun;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private Texture2D crosshair;

    private CharacterController characterController;
    private Camera camera;
    private Health playerHealth;
    private Joystick joystick;

    private bool isOnLadder;
    private bool recieveInput = true;

    public bool GetReciveInput() { return recieveInput; }
    public void SetRecieveInput(bool shouldRecieveInput) { recieveInput = shouldRecieveInput; }
    public int GetJoystickTrackedTouchID() { return joystick.GetTrackedID(); }

    void Start(){
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<Health>();

        camera = FindObjectOfType<Camera>();
        joystick = FindObjectOfType<Joystick>();

        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
        camera.GetComponent<CameraController>().SetPlayerGun(playerGun);
        joystick.player = this;
    }

    void Update(){
       
        if (!isLocalPlayer){
            return;
        }

        if (recieveInput){
            float hor = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float vert = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            Move(hor, vert);

            if (Input.GetMouseButtonDown(0)){
                CmdSpawnBullet();
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
                Debug.Log(this.gameObject.transform.up * vert);
            }
            else if (vert < 0){
                movement = (camera.gameObject.transform.forward * vert) + (camera.gameObject.transform.right * hor) + new Vector3(0, -9.8f * Time.deltaTime, 0);
            }else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * hor;
            }else{
                movement = (this.gameObject.transform.up * -9.8f * Time.deltaTime) + (camera.gameObject.transform.right * hor);
            }
        }

        characterController.Move(movement);
   }

    [Command]
    public void CmdSpawnBullet(){
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Bullet>().originObject = playerGun;

        NetworkServer.Spawn(bullet);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = true;
        }else if (other.gameObject.GetComponent<Bullet>() != null){
            playerHealth.TakeDamage(other.gameObject.GetComponent<Bullet>().bulletDamage);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Ladder"){
            isOnLadder = false;
        }
    }

    
}
