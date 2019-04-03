﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour{

    public int maxDamage;
    public float maxRange;
    public float fireRate;
    public float clipAmmo;
    //public float ammo;
    public float reloadTime;
    public float shotDuration;
    public Gradient damageFalloff;

    private bool canFire;
    private LineRenderer shotLine;

    void Start(){
        shotLine = GetComponent<LineRenderer>();

        canFire = true;
    }

    public void Fire(Camera playerCamera){
        if (!canFire){return;}

        shotLine.SetPosition(0, gameObject.transform.position);

        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, maxRange)){
            shotLine.SetPosition(1, hit.point);
            GameObject hitGameObject = hit.collider.gameObject;
            if (hitGameObject.GetComponent<Health>()){
                int damage = CalculateDamage(hit.distance);
                hitGameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }else{
            shotLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * maxRange));
        }

        StartCoroutine(ShowShot());

        canFire = false;
        StartCoroutine(WaitToFire(1 / fireRate));
    }

    private int CalculateDamage(float distance){
        int damage = 1;

        float distancePercent = distance / maxRange;
        Color damageColor = damageFalloff.Evaluate(distancePercent);
        float damagePercent = damageColor.r / 255;
        damage = (int)(maxDamage * damagePercent);

        return damage;
    }

    public IEnumerator Reload(){
        canFire = false;
        yield return new WaitForSeconds(reloadTime);
        canFire = false;
    }

    private IEnumerator WaitToFire(float timeBetweenShots){
        yield return new WaitForSeconds(timeBetweenShots);
        canFire = true;
    }

    private IEnumerator ShowShot(){
        shotLine.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        shotLine.enabled = false;
    }
}