using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huo : MonoBehaviour {
    public GameObject explo;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Cube") {
          
            Instantiate(explo, other.transform.position, Quaternion.identity);
        }
    }
}
