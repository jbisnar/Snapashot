using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot : MonoBehaviour {

	struct platdata {
		public Vector2 pos;
		public float xscale;
		public float yscale;
	};
	List<List<platdata>> Snapshots = new List<List<platdata>>();
	List<platdata> currentprev;
	bool photoMode = true;
	public GameObject platform;
	GameObject spawner;
	GameObject playerinst;
	public GameObject playerobj;
	int photoindex = 0;
	GameObject uican;
	public int levelpar = 0;

	// Use this for initialization
	void Start () {
		spawner = GameObject.Find("Spawner");
		uican = GameObject.Find("Canvas");
		uican.GetComponent<CamUI> ().LoadPhotoUI();
		uican.GetComponent<CamUI> ().ParUpdate(levelpar);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (photoMode) {
				Snap ();
			} else {
				photoindex++;
				if (Snapshots.Count > photoindex) {
					ViewPhoto (photoindex);
				}
			}
		}
		if (Input.GetKeyDown ("space")) {
			photoMode = !photoMode;
			if (photoMode) {
				var Prevs = GameObject.FindGameObjectsWithTag ("Preview");
				foreach (GameObject pre in Prevs) {
					pre.GetComponent<SpriteRenderer> ().enabled = true;
				}
				var Plats = GameObject.FindGameObjectsWithTag ("Platform");
				foreach (GameObject plat in Plats) {
					Destroy (plat);
				}
				Snapshots.Clear();
				if (playerinst != null) {
					Destroy (playerinst);
				}
				uican.GetComponent<CamUI> ().LoadPhotoUI ();
				uican.GetComponent<CamUI> ().Clear ();
			} else {
				if (Snapshots.Count > 0) {
					var Prevs = GameObject.FindGameObjectsWithTag ("Preview");
					foreach (GameObject pre in Prevs) {
						pre.GetComponent<SpriteRenderer> ().enabled = false;
					}
					ViewReset ();
				} else {
					Debug.Log ("No photos taken");
				}
			}
		}
		if (Input.GetKeyDown ("r")) {
			if (photoMode) {
				Snapshots.Clear ();
				uican.GetComponent<CamUI> ().Clear();
			} else {
				ViewReset ();
			}
		}
	}

	void Snap() {
		var Prevs = GameObject.FindGameObjectsWithTag ("Preview");
		currentprev = new List<platdata>();
		var previnstance = new platdata();
		for (int i = 0; i < Prevs.Length; i++) {
			previnstance.pos = Prevs [i].transform.position;
			previnstance.xscale = Prevs [i].transform.localScale.x;
			previnstance.yscale = Prevs [i].transform.localScale.y;
			currentprev.Add (previnstance);
		}
		Snapshots.Add (currentprev);
		uican.GetComponent<CamUI> ().Flash (Snapshots.Count);
	}

	void ViewPhoto(int index) {
		var Plats = GameObject.FindGameObjectsWithTag ("Platform");
		foreach (GameObject plat in Plats) {
			Destroy (plat);
		}
		var NewPlats = Snapshots [index];
		for (int i = 0; i < NewPlats.Count; i++) {
			var newplatinst = GameObject.Instantiate (platform, NewPlats [i].pos, transform.rotation);
			newplatinst.transform.parent = null;
			var platscale = new Vector2 (NewPlats [i].xscale, NewPlats [i].yscale);
			newplatinst.transform.localScale = platscale;
		}
		uican.GetComponent<CamUI> ().Shutter (index+1, Snapshots.Count);
	}

	public void ViewReset () {
		photoindex = 0;
		ViewPhoto (0);
		var Guys = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject guy in Guys) {
			Destroy (guy);
		}
		playerinst = GameObject.Instantiate (playerobj, spawner.transform.position, transform.rotation);
		playerinst.transform.parent = null;
		uican.GetComponent<CamUI> ().LoadSlideUI ();
	}
}
