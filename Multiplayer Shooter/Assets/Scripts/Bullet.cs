using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int bulletSpeed;
    public int bulletDamage;

    [SerializeField] private GameObject originObject;
    private Transform originTransform;
    private Rigidbody rb;

    void Start(){
        originTransform = originObject.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update(){
        Move();
    }

    private void Move(){
        rb.velocity = originTransform.forward * bulletSpeed;
    }
}
