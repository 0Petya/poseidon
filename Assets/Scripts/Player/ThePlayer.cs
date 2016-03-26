using UnityEngine;
using System.Collections;
using System.Linq;
using Rewired;

public class ThePlayer : MonoBehaviour {
	public float speed = 2f;
	public float jumpForce = 2.5f;
	public AudioClip[] clips;
	public GameObject currentWeapon;

	private PlayerController controller;
	private Rigidbody2D rb;
	private Animator animator;
	private BoxCollider2D boxCollider;
	private float velX;
	private bool hanging;
	private bool onGround;
	private bool onWallL;
	private bool onWallR;
	private string lastCol;
	private int stance;
	private AudioSource[] aSources;
	private LedgeCheck ledgeCheck;
	private GameObject arm;
	private Weapon weapon;

	void Start() {
		controller = GetComponent<PlayerController>();
		rb = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		stance = 2;

		aSources = new AudioSource[clips.Length];
		for (int i = 0; i < clips.Length; i++) {
			GameObject child = new GameObject("PlayerAudio");
			child.transform.parent = gameObject.transform;
			child.transform.localPosition = Vector3.zero;
			aSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			aSources[i].clip = clips[i];
		}

		ledgeCheck = GetComponentInChildren<LedgeCheck>();

		arm = GameObject.Find("Arm");

		currentWeapon = Instantiate(currentWeapon, transform.position, Quaternion.identity) as GameObject;
		currentWeapon.transform.parent = gameObject.transform;
		currentWeapon.transform.localPosition = new Vector3(-0.1f, 0.02f, 0);
		weapon = currentWeapon.GetComponent<Weapon>();
	}

	void OnCollisionStay2D(Collision2D other) {
		Collider2D oCollider = other.collider;

		if (other.gameObject.CompareTag("Solid")) {
			Vector3 contact = other.contacts[0].point;
			Bounds bounds = oCollider.bounds;

			if (!onGround && contact.y > bounds.min.y && contact.y < bounds.max.y) {
				if (contact.x >= bounds.max.x)
					onWallL = true;

				if (contact.x <= bounds.min.x)
					onWallR = true;
			}
		}
	}

	IEnumerator OffWall() {
		yield return new WaitForSeconds(0.1f);
		onWallL = false;
		onWallR = false;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag("Solid"))
			StartCoroutine(OffWall());
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

	void ResetAim() {
		arm.transform.localRotation = transform.localRotation;
	}

	void Diag(bool up) {
		float x = transform.localRotation.x;
		float y = transform.localRotation.y;
		float z = up ? 45f : -45f;
		arm.transform.localRotation = Quaternion.Euler(x, y, z);
	}

	void Vert(bool up) {
		float x = transform.localRotation.x;
		float y = transform.localRotation.y;
		float z = up ? 90f : -90f;
		arm.transform.localRotation = Quaternion.Euler(x, y, z);
	}

	void AimControl() {
		if (!animator.GetBool("reloading")) {
			if (controller.walking != 0 && controller.up) {
				weapon.Diag(true);
				Diag(true);
			}
			else if (controller.walking != 0 && controller.down) {
				weapon.Diag(false);
				Diag(false);
			}
			else if (controller.down && !onGround && stance == 2) {
				weapon.Vert(false);
				Vert(false);
			}
			else if (controller.up || (controller.diagUp && controller.diagDown)) {
				weapon.Vert(true);
				Vert(true);
			}
			else if (controller.diagUp) {
				weapon.Diag(true);
				Diag(true);
			}
			else if (controller.diagDown) {
				weapon.Diag(false);
				Diag(false);
			}
			else {
				weapon.ResetAim();
				ResetAim();
			}
		}
	}

	void StandUp() {
		if (stance < 2) {
			stance = 2;
			animator.SetBool("crouching", false);
		}
	}

	IEnumerator LetGo() {
		ledgeCheck.Grab(false);
		yield return new WaitForSeconds(0.1f);
		ledgeCheck.Grab(true);
	}

	void FullMode(bool change) {
		if (change) {
			animator.SetLayerWeight(0, 100f);
			animator.SetLayerWeight(1, 0f);
			animator.SetLayerWeight(2, 0f);
			animator.SetLayerWeight(3, 0f);
			weapon.DisableAnim(true);
		}
		else {
			animator.SetLayerWeight(0, 0f);
			animator.SetLayerWeight(1, 100f);
			animator.SetLayerWeight(2, 100f);
			animator.SetLayerWeight(3, 100f);
			weapon.DisableAnim(false);
		}
	}

	void HangingControl() {
		hanging = ledgeCheck.IsLedge();

		if (hanging) {
			animator.SetBool("reloading", false);

			FullMode(true);
			animator.SetBool("hanging", true);

			if (controller.down && controller.jumping) {
				StandUp();
				StartCoroutine(LetGo());
			}
			else if (controller.jumping)
				iHangJump = true;
		}
		else {
			FullMode(false);
			animator.SetBool("hanging", false);
		}
	}

	void WallControl() {
		if (!hanging && !onGround) {
			if (onWallL) {
				if (controller.walking > 0 && controller.jumping) {
					if (lastCol == "right" || lastCol == "ground")
						resetVelY = true;

					iWallJump = 1;
					lastCol = "left";
				}
			}

			if (onWallR) {
				if (controller.walking < 0 && controller.jumping) {
					if (lastCol == "left" || lastCol == "ground")
						resetVelY = true;

					iWallJump = -1;
					lastCol = "right";
				}
			}
		}
	}

	void GroundControl() {
		if (onGround) {
			animator.SetBool("jumping", false);

			if (controller.jumping) {
				iGroundJump = true;
				StandUp();
			}

			if (controller.walking == 0) {
				if (stance == 1) {
					boxCollider.offset = new Vector2(boxCollider.offset.x, -0.02f);
					boxCollider.size = new Vector2(boxCollider.size.x, 0.72f);

					animator.SetBool("crouching", true);
				}
			}

			if (stance != 1) {
				boxCollider.offset = new Vector2(boxCollider.offset.x, -0.08f);
				boxCollider.size = new Vector2(boxCollider.size.x, 0.84f);

				animator.SetBool("crouching", false);
			}
		}
		else
			animator.SetBool("jumping", true);
	}

	void ControlAndAnimation() {
		float absVelX = Mathf.Abs(rb.velocity.x);

		GroundControl();
		WallControl();
		HangingControl();

		if (!hanging) {
			if (controller.walking != 0)
				transform.localScale = new Vector3(controller.walking, 1, 1);

			if (controller.walking != 0) {
				iWalking = 1;

				animator.SetBool("walking", true);
				PlayOnce(2, controller.sWalking);
				KeepPlaying(3);
				StandUp();
			}
			else {
				iWalking = -1;

				if (absVelX < 0.1f) {
					animator.SetBool("walking", false);
					aSources[3].Stop();
				}
			}

			AimControl();

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

			if (controller.reloading && !controller.shooting) {
				animator.SetBool("reloading", true);
				weapon.BeginReload();
				weapon.ResetAim();
				ResetAim();
			}
		}
	}

  void GroundCheck() {
    onGround = false;
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1f);
    if (hit.collider != null && hit.collider.gameObject.CompareTag("Solid")) {
      onGround = true;
      lastCol = "ground";
    }
  }

	void StanceUpdate() {
		if (onGround) {
			if (controller.stance == 1) {
				if (stance < 2) {
					stance++;

					if (onGround && controller.walking == 0)
						PlayOnce(2, controller.stance != 0);
				}
			}

			if (controller.stance == -1) {
				if (stance > 1) {
					stance--;

					if (onGround && controller.walking == 0)
						PlayOnce(2, controller.stance != 0);
				}
			}
		}
	}

	void Update() {
		StanceUpdate();
    GroundCheck();
		ControlAndAnimation();
	}

	private bool iHangJump;
	private bool resetVelY;
	private int iWallJump;
	private bool iGroundJump;
	private int iWalking;

	void FixedUpdate() {
		if (hanging) {
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.gravityScale = 0;
		}
		else
			rb.gravityScale = 1f;

		if (iHangJump) {
			rb.AddForce(new Vector2(0, jumpForce * 90f));
			iHangJump = false;
		}

		if (resetVelY) {
			rb.velocity = new Vector2(rb.velocity.x, 0);
			resetVelY = false;
		}

		if (iWallJump != 0) {
			rb.AddForce(new Vector2(100 * iWallJump, jumpForce * 75));
			iWallJump = 0;
		}

		if (iGroundJump) {
			rb.AddForce(new Vector2(0, jumpForce * 100));
			iGroundJump = false;
		}

		if (iWalking != 0) {
			if (iWalking == 1)
				velX = Mathf.MoveTowards(rb.velocity.x, speed * controller.walking, 50f * Time.deltaTime);

			if (iWalking == -1)
				velX = Mathf.MoveTowards(rb.velocity.x, 0f, 4f * Time.deltaTime);

			iWalking = 0;
		}

		rb.velocity = new Vector2(velX, rb.velocity.y);
	}
}
