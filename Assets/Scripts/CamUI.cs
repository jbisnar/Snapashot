using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CamUI : MonoBehaviour {

	public Text timertext;
	public GameObject photoUI;
	public GameObject slideshowUI;
	public Text photocounttext;
	public Text viewingindextext;
	public Text partext;
	bool timing = true;
	float flashstart;
	float flashtime = .15f;
	float shuttertime = .1f;
	bool flashing = false;
	Color flashcolor = new Color (1, 1, 1, .75f);
	Color clearwhite = new Color (1, 1, 1, 0);
	float shutterstart;
	bool shuttering = false;
	Vector2 shutterclosed = new Vector2(0,0);
	Vector2 shutteropen;
	public GameObject flash;
	public GameObject shutter;
	public AudioClip flashsound;
	public AudioClip shuttersound;
	public Scene firstScene;
	public float prevruntime = 0;

	// Use this for initialization
	void Start () {
		//timertext = transform.GetChild (0).gameObject.GetComponent<Text>();
		photoUI = transform.GetChild (1).gameObject;
		slideshowUI = transform.GetChild (2).gameObject;
		if (SceneManager.GetActiveScene ().buildIndex == 0) {
			Debug.Log ("Resetting timer");
			prevruntime = Time.time;
		}
		shutteropen = new Vector2 (0, Screen.height);
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (timing) {
			var seconds = (int)(Time.time - prevruntime);
			if (seconds % 60 < 10) {
				timertext.text = "" + seconds / 60 + ":0" + seconds % 60;
			} else {
				timertext.text = "" + seconds / 60 + ":" + seconds % 60;
			}
		}

		if (flashing) {
			flash.GetComponent<Image>().color = Color.Lerp(flashcolor,clearwhite, Mathf.Max ((Time.time - flashstart), 0)/ flashtime);
			if (Time.time > flashstart + flashtime) {
				flashing = false;
				flash.GetComponent<Image> ().color = clearwhite;
			}
		}

		if (shuttering) {
			shutter.GetComponent<RectTransform>().localPosition = Vector2.Lerp(shutterclosed,shutteropen, Mathf.Max ((Time.time - shutterstart), 0)/ shuttertime);
			if (Time.time > shutterstart + shuttertime) {
				shuttering = false;
				shutter.transform.localPosition = shutteropen;
			}
		}

		if (Input.GetKey("escape"))
		{
			Debug.Log ("Quitting");
			Application.Quit();
		}
	}

	public void LoadPhotoUI () {
		photoUI.SetActive (true);
		slideshowUI.SetActive (false);
	}

	public void LoadSlideUI () {
		photoUI.SetActive (false);
		slideshowUI.SetActive (true);
	}

	public void Flash (int photocount) {
		GetComponent<AudioSource> ().clip = flashsound;
		GetComponent<AudioSource> ().Play();
		flashing = true;
		flashstart = Time.time;
		photocounttext.text = "[R] CLEAR (" + photocount + ")";
	}

	public void Clear () {
		photocounttext.text = "[R] CLEAR (0)";
	}

	public void Shutter(int photoindex, int photocount) {
		GetComponent<AudioSource> ().clip = shuttersound;
		GetComponent<AudioSource> ().Play();
		shuttering = true;
		shutterstart = Time.time;
		viewingindextext.text = photoindex + "/" + photocount;
	}

	public void ParUpdate (int par) {
		partext.text = "PAR: "+par;
	}
}
