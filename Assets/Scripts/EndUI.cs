using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour {

	GameObject uican;
	public string firstScene;
	public Text finaltime;
	public Button replaybutton;

	// Use this for initialization
	void Start () {
		uican = GameObject.Find("Canvas");
		uican.SetActive (false);
		var seconds = (int)(Time.time - uican.GetComponent<CamUI>().prevruntime);
		if (seconds % 60 < 10) {
			finaltime.text = "" + seconds / 60 + ":0" + seconds % 60;
		} else {
			finaltime.text = "" + seconds / 60 + ":" + seconds % 60;
		}
		Destroy (uican);
		replaybutton.onClick.AddListener (Restart);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("escape"))
		{
			Debug.Log ("Quitting");
			Application.Quit();
		}
	}

	public void Restart() {
		Debug.Log ("Restart clicked");
		SceneManager.LoadScene(firstScene);
	}
}
