using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public int walking;
	public int stance;
	public bool down;
	public bool jumping;
	public int diag;
	public bool shooting;
	public bool reloading;
	public bool sWalking;
	public bool sShooting;

	void Update() {
		walking = stance = diag = 0;
		down = jumping = shooting = reloading = false;
		sWalking = sShooting = false;

		if (Input.GetKey(KeyCode.D))
			walking = 1;
		else if (Input.GetKey(KeyCode.A))
			walking = -1;

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
			sWalking = true;

		if (Input.GetKeyDown(KeyCode.W))
			stance = 1;

		if (Input.GetKey(KeyCode.S))
			down = true;

		if (Input.GetKeyDown(KeyCode.S))
			stance = -1;

		if (Input.GetKeyDown(KeyCode.K))
			jumping = true;

		if (Input.GetKey(KeyCode.E))
			diag = 1;
		else if (Input.GetKey(KeyCode.Q))
			diag = -1;

		if (Input.GetKey(KeyCode.J))
			shooting = true;

		if (Input.GetKeyDown(KeyCode.J))
			sShooting = true;

		if (Input.GetKeyDown(KeyCode.R))
			reloading = true;
	}
}
