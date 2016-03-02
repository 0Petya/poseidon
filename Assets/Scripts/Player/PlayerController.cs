using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerController : MonoBehaviour {
	public int walking;
	public int stance;
	public bool up;
	public bool down;
	public bool diagUp;
	public bool diagDown;
	public bool jumping;
	public bool shooting;
	public bool reloading;
	public bool sWalking;
	public bool sShooting;

	private Player controls;

	void Start() {
		controls = ReInput.players.GetPlayer(0);
	}

	void Update() {
		walking = stance = 0;
		up = down = diagUp = diagDown = jumping = shooting = reloading = false;
		sWalking = sShooting = false;

		if (controls.GetButton("WalkRight"))
			walking = 1;
		else if (controls.GetButton("WalkLeft"))
			walking = -1;

		if (controls.GetButtonDown("WalkRight") || controls.GetButtonDown("WalkLeft"))
			sWalking = true;

		if (controls.GetButton("StanceUp"))
			up = true;

		if (controls.GetButtonDown("StanceUp"))
			stance = 1;

		if (controls.GetButton("StanceDown"))
			down = true;

		if (controls.GetButtonDown("StanceDown"))
			stance = -1;

		if (controls.GetButton("AimDiagUp"))
			diagUp = true;
		
		if (controls.GetButton("AimDiagDown"))
			diagDown = true;

		if (controls.GetButtonDown("Jump"))
			jumping = true;

		if (controls.GetButton("Shoot"))
			shooting = true;

		if (controls.GetButtonDown("Shoot"))
			sShooting = true;

		if (controls.GetButtonDown("Reload"))
			reloading = true;
	}
}
