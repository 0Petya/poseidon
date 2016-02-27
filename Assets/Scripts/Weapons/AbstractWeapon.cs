using UnityEngine;
using System.Collections;

abstract class AbstractWeapon : MonoBehaviour, Weapon {
	public int maxAmmo = 0;
	public AmmoCount ammoCount = null;
	public Decal decal = null;
	public AudioClip[] clips = null;

	protected int ammo;
	protected Animator animator;
	protected AudioSource[] aSources;

	protected void Start() {
		ammo = maxAmmo;
		animator = GetComponent<Animator>();
		Instantiate(ammoCount, new Vector2(0, 0), Quaternion.identity);

		aSources = new AudioSource[clips.Length];
		for (int i = 0; i < clips.Length; i++) {
			GameObject child = new GameObject("WeaponAudio");
			child.transform.parent = gameObject.transform;
			child.transform.localPosition = Vector3.zero;
			aSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			aSources[i].clip = clips[i];
		}
	}

	abstract public bool IsAuto();

	public int GetAmmo() {
		return ammo;
	}

	public void Shoot(bool trigger) {
		if (trigger)
			animator.SetBool("shooting", true);
		else
			animator.SetBool("shooting", false);
	}

	public void Fire() {
		FireWeapon();
	}

	abstract public void FireWeapon();

	public void DryFire() {
		aSources[1].Play();
	}

	public void BeginReload() {
		aSources[2].Play();
		animator.SetBool("reloading", true);
	}

	public void EndReload() {
		ammo = maxAmmo;
		animator.SetBool("reloading", false);
	}
}
