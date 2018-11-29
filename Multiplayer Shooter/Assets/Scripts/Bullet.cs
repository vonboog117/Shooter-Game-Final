using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public int bulletSpeed;
    public int bulletDamage;

    public float secondsBeforeDestroyed;

    public GameObject originObject;
    private Vector3 originTransform;
    private Rigidbody rb;

    void Start(){
        originTransform = originObject.transform.forward;
        rb = GetComponent<Rigidbody>();

        //NetworkServer.Spawn(this.gameObject);

        StartCoroutine(DestroyBullet());
    }

    private void Update(){
        CmdMove();
    }

    //[Command]
    private void CmdMove(){
        rb.velocity = originTransform * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other){
        NetworkServer.Destroy(this.gameObject);
    }

    private IEnumerator DestroyBullet(){
        yield return new WaitForSeconds(secondsBeforeDestroyed);

        Destroy(this.gameObject);
    }
}
