using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public int walking;
	public int stance = 2;
	public bool jumping;
	public bool shooting;
	public bool reloading;
	public bool sWalking;
	public bool sStance;
	public bool sShooting;

	void Update() {
		walking = 0;
		jumping = shooting = reloading = false;
		sWalking = sStance = sShooting = false;

		if (Input.GetKey(KeyCode.D))
			walking = 1;
		else if (Input.GetKey(KeyCode.A))
			walking = -1;

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
			sWalking = true;

		if (Input.GetKeyDown(KeyCode.W)) {
			if (stance < 2) {
				stance++;
				sStance = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.S)) {
			if (stance > 1) {
				stance--;
				sStance = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.K))
			jumping = true;

		if (Input.GetKey(KeyCode.J))
			shooting = true;

		if (Input.GetKeyDown(KeyCode.J))
			sShooting = true;

		if (Input.GetKeyDown(KeyCode.R))
			reloading = true;
	}
}
