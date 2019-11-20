using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	float gravNormal = 8f;
	float gravJump = 3f;
	float gravDown = 10f;
	float gravWall = 5f;
	float velWalk = 3f;
	float accelWalk = 30f;
	float accelSwitch = 60f;
	float accelSlow = 20f;
	float accelAir = 10f;
	float accelAirSlow = 5f;
	float velJumpGround = 3f;
	float velJumpWallH = 5f;
	float velJumpWallV = 4f;
	float velWallSlideDown = 0f;

	public bool grounded = false;
	public bool jumping = false;
	public bool walledL = false;
	public bool wallslideL = false;
	public bool walledR = false;
	public bool wallslideR = false;
	public float walljumpgrace = .05f;
	float walljumpgracetime;
	public LayerMask layerGround;
	public Vector2 temp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		temp = transform.GetComponent<Rigidbody2D>().velocity;
		grounded = Physics2D.OverlapArea (new Vector2 (transform.position.x-.16f,transform.position.y-.36f), 
			new Vector2 (transform.position.x+.16f,transform.position.y-.38f), layerGround);
		walledL = Physics2D.OverlapArea (new Vector2 (transform.position.x-.19f,transform.position.y+.36f), 
			new Vector2 (transform.position.x-.16f,transform.position.y-.36f), layerGround);
		wallslideL = walledL && Input.GetAxisRaw ("Horizontal") < 0;
		walledR = Physics2D.OverlapArea (new Vector2 (transform.position.x+.16f,transform.position.y+.36f), 
			new Vector2 (transform.position.x+.19f,transform.position.y-.36f), layerGround);
		wallslideR = walledR && Input.GetAxisRaw ("Horizontal") > 0;
		if (walledL || walledR) {
			walljumpgracetime = Time.time + walljumpgrace;
		}

		//HORIZONTAL CONTROL
		if (Input.GetAxisRaw ("Horizontal") == 0) { //Slow down
			if (Mathf.Abs(temp.x) < accelSlow * Time.deltaTime) {
				temp.x = 0;
			} else  if (temp.x > 0){
				if (grounded) {
					temp.x -= accelSlow * Time.deltaTime;
				} else {
					temp.x -= accelAirSlow * Time.deltaTime;
				}
			} else {
				if (grounded) {
					temp.x += accelSlow * Time.deltaTime;
				} else {
					temp.x += accelAirSlow * Time.deltaTime;
				}
			}
		} else if (Input.GetAxisRaw ("Horizontal") > 0) { //Right
			if (walledR) {
				temp.x = 0;
			}
			/*
			else if (temp.x < 0 && grounded) {
				temp.x += accelSwitch * Time.deltaTime;
			} else if (temp.x < velWalk) {
				temp.x += accelWalk * Time.deltaTime;
			} else {
				temp.x = velWalk;
			}
			*/
			else if (grounded) {
				if (temp.x > velWalk) {
					temp.x = velWalk;
				} else if (temp.x < 0) {
					temp.x += accelSwitch * Time.deltaTime;
				} else {
					temp.x += accelWalk * Time.deltaTime;
				}
			} else {
				if (temp.x > velWalk) {
					temp.x = temp.x;
				} else {
					temp.x += accelAir * Time.deltaTime;
				}
			}
		} else { //Left
			if (walledL) {
				temp.x = 0;
			}
			/*
			else if (!grounded) {
				if (temp.x < -velWalk) {
					temp.x = temp.x;
				} else {
					temp.x -= accelWalk * Time.deltaTime;
				}
				//temp.x -= accelWalk * Time.deltaTime;
			} else if (temp.x > 0 && grounded) {
				temp.x -= accelSwitch * Time.deltaTime;
			} else if (temp.x > -velWalk) {
				temp.x -= accelWalk * Time.deltaTime;
			} else {
				temp.x = -velWalk;
			}
			*/
			else if (grounded) {
				if (temp.x < -velWalk) {
					temp.x = -velWalk;
				} else if (temp.x > 0) {
					temp.x -= accelSwitch * Time.deltaTime;
				} else {
					temp.x -= accelWalk * Time.deltaTime;
				}
			} else {
				if (temp.x < -velWalk) {
					temp.x = temp.x;
				} else {
					temp.x -= accelAir * Time.deltaTime;
				}
			}
		}

		//GRAVITY
		if (grounded) {
			jumping = false;
			temp.y = 0;
			if (Input.GetAxisRaw ("Vertical") > 0) {
				temp.y = velJumpGround;
				jumping = true;
			}
		} else if (walledL) {
			jumping = false;
			if (Input.GetKeyDown ("w")) {
				temp.x = velJumpWallH;
				temp.y = velJumpWallV;
				jumping = true;
			}
			if (wallslideL && temp.y < -velWallSlideDown) {
				temp.y = -velWallSlideDown;
			} else if (Input.GetAxisRaw ("Vertical") > 0 && temp.y > 0) {
				temp.y -= gravJump * Time.deltaTime;
			} else if (Input.GetAxisRaw ("Vertical") < 0) {
				jumping = false;
				temp.y -= gravDown * Time.deltaTime;
			} else {
				jumping = false;
				temp.y -= gravNormal * Time.deltaTime;
			}
		} else if (walledR) {
			jumping = false;
			if (Input.GetKeyDown ("w")) {
				temp.x = -velJumpWallH;
				temp.y = velJumpWallV;
				jumping = true;
			}
			if (wallslideR && temp.y < -velWallSlideDown) {
				temp.y = -velWallSlideDown;
			} else if (Input.GetKey ("w") && temp.y > 0) {
				temp.y -= gravJump * Time.deltaTime;
			} else if (Input.GetAxisRaw ("Vertical") < 0) {
				jumping = false;
				temp.y -= gravDown * Time.deltaTime;
			} else {
				jumping = false;
				temp.y -= gravNormal * Time.deltaTime;
			}
		} else if (Time.time < walljumpgracetime) {
			jumping = false;
			if (Input.GetKeyDown ("w")) {
				//temp.x = -velJumpWallH;
				temp.y = velJumpWallV;
				jumping = true;
			}
		} else {
			if (Input.GetKey ("w") && temp.y > 0 && jumping) {
				temp.y -= gravJump * Time.deltaTime;
			} else if (Input.GetAxisRaw ("Vertical") < 0) {
				jumping = false;
				temp.y -= gravDown * Time.deltaTime;
			} else {
				jumping = false;
				temp.y -= gravNormal * Time.deltaTime;
			}
		}
		transform.GetComponent<Rigidbody2D> ().velocity = temp;
	}
}
