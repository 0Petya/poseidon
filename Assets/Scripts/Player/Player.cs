using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {
	public float speed = 2f;
	public float jumpForce = 2f;
	public AudioClip[] clips;
	public GameObject currentWeapon;

	private PlayerController controller;
	private Rigidbody2D rb;
	private Animator animator;
	private float velX;
	private bool onGround;
	private bool onWallL;
	private bool onWallR;
	private string lastCol;
	private int stance;
	private AudioSource[] aSources;
	private bool[] played;
	private Weapon weapon;

	void Start() {
		controller = GetComponent<PlayerController>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		stance = 2;

		aSources = new AudioSource[clips.Length];
		played = new bool[clips.Length];
		for (int i = 0; i < clips.Length; i++) {
			GameObject child = new GameObject("PlayerAudio");
			child.transform.parent = gameObject.transform;
			child.transform.localPosition = Vector3.zero;
			aSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			aSources[i].clip = clips[i];
			played[i] = false;
		}

		currentWeapon = Instantiate(currentWeapon, transform.position, Quaternion.identity) as GameObject;
		currentWeapon.transform.parent = gameObject.transform;
		currentWeapon.transform.localPosition = Vector3.zero;
		weapon = currentWeapon.GetComponent<Weapon>();
	}

	void OnCollisionStay2D(Collision2D other) {
		Collider2D collider = other.collider;

		if (other.gameObject.CompareTag("Solid")) {
			Vector3 contact = other.contacts[0].point;
			Bounds bounds = collider.bounds;

			if (contact.y >= bounds.max.y && contact.x >= bounds.min.x && contact.x <= bounds.max.x) {
				onGround = true;
				lastCol = "ground";
			}

			if (!onGround && contact.y > bounds.min.y && contact.y < bounds.max.y) {
				if (contact.x >= bounds.max.x)
					onWallL = true;

				if (contact.x <= bounds.min.x)
					onWallR = true;
			}
		}
	}

	IEnumerator OffWall() {
		yield return new WaitForSeconds(0.01f);
		onWallL = false;
		onWallR = false;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag("Solid")) {
			onGround = false;
			StartCoroutine(OffWall());
		}
	}

	void FirstStep() {
		aSources[0].Play();
	}

	void SecondStep() {
		aSources[1].Play();
	}

	void EndReload() {
		weapon.EndReload();
		animator.SetBool("reloading", false);
	}

	void PlayOnce(int i, bool start) {
		if (start)
			aSources[i].Play();
	}

	void KeepPlaying(int i) {
		if (!aSources[i].isPlaying)
			aSources[i].Play();
	}

	void StandUp() {
		if (stance < 2) {
			stance = 2;
			animator.SetBool("crouching", false);
		}
	}

	void WallControl() {
		if (onWallL) {
			if (controller.walking > 0 && controller.jumping) {
				if (lastCol == "right" || lastCol == "ground")
					rb.velocity = new Vector2(rb.velocity.x, 0);

				rb.AddForce(new Vector2(100, jumpForce * 75));
				lastCol = "left";
			}
		}

		if (onWallR) {
			if (controller.walking < 0 && controller.jumping) {
				if (lastCol == "left" || lastCol == "ground")
					rb.velocity = new Vector2(rb.velocity.x, 0);

				rb.AddForce(new Vector2(-100, jumpForce * 75));
				lastCol = "right";
			}
		}
	}

	void GroundControl() {
		if (onGround) {
			animator.SetBool("jumping", false);

			if (controller.jumping) {
				rb.AddForce(new Vector2(0, jumpForce * 100));
				StandUp();
			}

			if (controller.walking == 0) {
				if (stance == 1) {
					animator.SetBool("crouching", true);
					PlayOnce(2, controller.stance != 0);
				}
				else {
					animator.SetBool("crouching", false);
					PlayOnce(2, controller.stance != 0);
				}
			}
		}
		else
			animator.SetBool("jumping", true);
	}

	void ControlAndAnimation() {
		float absVelX = Mathf.Abs(rb.velocity.x);

		GroundControl();
		WallControl();

		if (controller.walking != 0)
			transform.localScale = new Vector3(controller.walking, 1, 1);

		if (controller.walking != 0) {
			velX = Mathf.MoveTowards(rb.velocity.x, speed * controller.walking, 50f * Time.deltaTime);

			animator.SetBool("walking", true);
			PlayOnce(2, controller.sWalking);
			KeepPlaying(3);
			StandUp();
		}
		else {
			velX = Mathf.MoveTowards(rb.velocity.x, 0f, 4f * Time.deltaTime);

			if (absVelX < 0.1f) {
				animator.SetBool("walking", false);
				aSources[3].Stop();
			}
		}

		if (weapon.IsAuto()) {
			if (controller.shooting && weapon.GetAmmo() > 0) {
				animator.SetBool("shooting", true);
				weapon.Shoot(true);
			}
			else {
				if (animator.GetBool("shooting") && weapon.GetAmmo() == 0)
					weapon.DryFire();

				animator.SetBool("shooting", false);
				weapon.Shoot(false);
			}

			if (controller.sShooting && weapon.GetAmmo() == 0)
				weapon.DryFire();
		}

		if (controller.reloading) {
			animator.SetBool("reloading", true);
			weapon.BeginReload();
		}
		
		rb.velocity = new Vector2(velX, rb.velocity.y);
	}

	void StanceUpdate() {
		if (controller.stance == 1) {
			if (stance < 2)
				stance++;
		}

		if (controller.stance == -1) {
			if (stance > 1)
				stance--;
		}
	}

	void Update() {
		StanceUpdate();
		ControlAndAnimation();
	}
}
