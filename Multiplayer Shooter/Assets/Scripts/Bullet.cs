using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int bulletSpeed;
    public int bulletDamage;

    public GameObject originObject;
    private Vector3 originTransform;
    private Rigidbody rb;

    void Start(){
        originTransform = originObject.transform.forward;
        rb = GetComponent<Rigidbody>();
    }

    private void Update(){
        Move();
    }

    private void Move(){
        rb.velocity = originTransform * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other){
        Destroy(this.gameObject);
    }
}
