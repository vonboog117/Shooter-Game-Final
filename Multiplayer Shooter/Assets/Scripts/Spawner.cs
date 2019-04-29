using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject objectPrefab;
    public float respawnTime;

	void Start () {
        Instantiate(objectPrefab, gameObject.transform);
	}

    public void StartRespawnTimer(){
        StartCoroutine(RespawnObject());
    }

    public IEnumerator RespawnObject(){
        yield return new WaitForSeconds(respawnTime);
        Instantiate(objectPrefab, gameObject.transform);
    }
}
