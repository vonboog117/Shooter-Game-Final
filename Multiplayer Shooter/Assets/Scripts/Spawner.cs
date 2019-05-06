using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject objectPrefab;
    public GameObject currentObject;
    public float respawnTime;

    private Despawner despawner;

    void Start () {
        currentObject = Instantiate(objectPrefab, transform.position, transform.rotation);
        despawner = currentObject.GetComponent<Despawner>();
        despawner.SetSpawner(this);
    }

    public void StartRespawnTimer(){
        StartCoroutine(RespawnObject());
    }

    public IEnumerator RespawnObject(){
        yield return new WaitForSeconds(respawnTime);
        currentObject = Instantiate(objectPrefab, transform.position, transform.rotation);
        despawner = currentObject.GetComponent<Despawner>();
        despawner.SetSpawner(this);
    }
}
