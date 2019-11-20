using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPreview : MonoBehaviour {

	public float movetime = 2f;
	public float staytime = .5f;
	public float changetime;
	bool moving;
	public Transform path;
	public int pathind = 0;
	Transform[] idlepath;
	Vector2 lastpos;
	Vector2 nextpos;

	// Use this for initialization
	void Start () {
		if (path != null) {
			changetime = Time.time;
			idlepath = new Transform[path.childCount];
			for (int i = 0; i < idlepath.Length; i++) {
				idlepath [i] = path.GetChild (i);
			}
			nextpos = idlepath [pathind].position;
			transform.position = nextpos;
			lastpos = nextpos;
			moving = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (path != null) {
			if (Time.time > changetime) {
				moving = !moving;
				if (moving) {
					lastpos = nextpos;
					pathind++;
					if (pathind == idlepath.Length) {
						pathind = 0;
					}
					nextpos = idlepath [pathind].position;
					changetime += movetime;
				} else {
					changetime += staytime;
				}
			}
			if (moving) {
				transform.position = Vector2.Lerp (lastpos, nextpos, (movetime - Mathf.Max ((changetime - Time.time), 0)) / movetime);
			}
		}
	}
}
