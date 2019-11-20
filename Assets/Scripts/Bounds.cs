using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour {

	GameObject snaps;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == 8) { //Player
			snaps = GameObject.Find("Snapshot");
			snaps.GetComponent<Snapshot>().ViewReset();
		}
	}
}
