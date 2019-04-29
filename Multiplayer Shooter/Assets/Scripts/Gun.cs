using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour{

    public int maxDamage;
    public int maxClipAmmo;
    public float maxRange;
    public float fireRate;
    public float reloadTime;
    public float shotDuration;
    public string gunName;
    public Gradient damageFalloff;

    private bool canFire;
    private bool reloading;
    private int clipAmmo;
    private LineRenderer shotLine;
    private PlayerController playerController;
    private PlayerUIManager playerUIManager;
    private GunInteractable gi;

    void Start(){
        shotLine = GetComponent<LineRenderer>();
        playerController = GetComponentInParent<PlayerController>();
        playerUIManager = playerController.GetUIManager();
        playerUIManager.ChangeGunAmmoUI(maxClipAmmo, maxClipAmmo);
        gi = GetComponent<GunInteractable>();

        canFire = true;
        reloading = false;
        clipAmmo = maxClipAmmo;
    }

    public void Fire(Camera playerCamera){
        if (!canFire){return;}
        shotLine.SetPosition(0, gameObject.transform.position);

        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, maxRange, 3)){
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

        clipAmmo--;
        playerUIManager.ChangeGunAmmoUI(clipAmmo, maxClipAmmo);
        if (clipAmmo <= 0){
            StartCoroutine(ReloadCo());
        }else{
            StartCoroutine(WaitToFire(1 / fireRate));
        } 
    }

    public void Reload(){
        if (clipAmmo != maxClipAmmo){
            canFire = false;
            reloading = true;
            StartCoroutine(ReloadCo());
        }
    }

    public void Equip(){
        playerController = GetComponentInParent<PlayerController>();
        playerUIManager = playerController.GetUIManager();
        playerUIManager.ChangeGunAmmoUI(clipAmmo, maxClipAmmo);
        playerUIManager.ChangeGunUI(gunName);
    }

    public void Drop(){
        gi.SwitchToInteractable();
        playerUIManager.ChangeGunAmmoUI(0,0);
        playerUIManager.ChangeGunUI("No Gun");
    }

    private int CalculateDamage(float distance){
        int damage = 1;

        float distancePercent = distance / maxRange;
        Color damageColor = damageFalloff.Evaluate(distancePercent);
        float damagePercent = damageColor.r / 1;
        damage = (int)(maxDamage * damagePercent);

        return damage + 1;
    }

    public IEnumerator ReloadCo(){
        yield return new WaitForSeconds(reloadTime);
        clipAmmo = maxClipAmmo;
        playerUIManager.ChangeGunAmmoUI(clipAmmo, maxClipAmmo);
        canFire = true;
        reloading = false;
    }

    private IEnumerator WaitToFire(float timeBetweenShots){
        yield return new WaitForSeconds(timeBetweenShots);
        if (!reloading){
            canFire = true;
        }
    }

    private IEnumerator ShowShot(){
        shotLine.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        shotLine.enabled = false;
    }
}