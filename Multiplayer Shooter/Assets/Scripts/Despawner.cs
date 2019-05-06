using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {

    public float despawnTime;

    private Spawner spawner;
    public void SetSpawner(Spawner s) { spawner = s; }
    public Spawner GetSpawner() { return spawner; }

    public void StartDespawnTimer(){
        StartCoroutine("DespawnObject");
    }

    public void StopDespawn(){
        StopCoroutine("DespawnObject");
    }

    public IEnumerator DespawnObject(){
        yield return new WaitForSeconds(despawnTime);

        if (spawner != null){
            spawner.currentObject = null;
            spawner.StartRespawnTimer();
        }
        Destroy(gameObject);
    }
}
