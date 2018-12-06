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

    private bool isOnLadder;
    private bool recieveInput = true;

    public bool GetReciveInput() { return recieveInput; }
    public void SetRecieveInput(bool shouldRecieveInput) { recieveInput = shouldRecieveInput; }

    void Start(){
        characterController = GetComponent<CharacterController>();
        camera = FindObjectOfType<Camera>();
        camera.GetComponent<CameraController>().SetPlayer(this.gameObject);
        camera.GetComponent<CameraController>().SetPlayerGun(playerGun);
        playerHealth = GetComponent<Health>();
    }

    void Update(){
       
        if (!isLocalPlayer){
            return;
        }

        if (recieveInput){
            Move();

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

        playerNumber = NetworkServer.connections.Count;
    }

    private void Move(){
        Vector3 movement = Vector3.zero;

        if (!isOnLadder){
            if (!SystemInfo.deviceModel.Contains("iPad")){
                float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
                float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

                movement = ((camera.gameObject.transform.forward * z) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0));
            }else{

            }
        }else{
            float y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            if (y > 0){
                movement = (this.gameObject.transform.up * y) + (camera.gameObject.transform.right * x);
            }else if (y < 0){
                movement = (camera.gameObject.transform.forward * y) + (camera.gameObject.transform.right * x) + new Vector3(0, -9.8f * Time.deltaTime, 0);
            }else if (Input.GetKey(KeyCode.LeftShift)){
                movement = camera.gameObject.transform.right * x;
            }else{
                movement = (this.gameObject.transform.up * -9.8f * Time.deltaTime) + (camera.gameObject.transform.right * x);
            }
        }

        characterController.Move(movement);
    }

    [Command]
    void CmdSpawnBullet(){
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
