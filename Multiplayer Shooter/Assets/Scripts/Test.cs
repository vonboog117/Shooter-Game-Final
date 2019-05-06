using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    bool test = false;

    void Update(){
        if (Input.GetKeyDown(KeyCode.N)){
            StartCoroutine("Testing");
            test = true;
            Debug.Log("Started");
        }
    }

    private IEnumerator Testing(){
        yield return new WaitForSeconds(5);
        Debug.Log("Done");
        test = false;
    }
}
