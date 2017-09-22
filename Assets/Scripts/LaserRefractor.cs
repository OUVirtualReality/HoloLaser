using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRefractor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // These methods will be called by RaycastExample
    void OnRaycastEnter(GameObject sender)
    {
        this.GetComponent<LaserEmitter>().enabled = true;
    }
}
