using UnityEngine;
using System.Collections;

abstract class AbstractWeapon : MonoBehaviour, Weapon {
	public int maxAmmo = 0;
	public AmmoCount ammoCount = null;
	public Decal decal = null;
	public AudioClip[] clips = null;

	protected int ammo;
	protected Player player;
	protected PlayerController controller;
	protected AudioSource[] aSources;

	protected void Start() {
		ammo = maxAmmo;
		Instantiate(ammoCount, new Vector2(0, 0), Quaternion.identity);
		player = GameObject.FindObjectOfType<Player>();
		controller = player.GetComponent<PlayerController>();

		aSources = new AudioSource[clips.Length];
		for (int i = 0; i < clips.Length; i++) {
			GameObject child = new GameObject("WeaponAudio");
			child.transform.parent = player.transform;
			child.transform.localPosition = Vector3.zero;
			aSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			aSources[i].clip = clips[i];
		}
	}

	abstract public bool IsAuto();

	public int GetAmmo() {
		return ammo;
	}

	abstract public void Shoot();

	public void DryFire() {
		aSources[1].Play();
	}

	public void BeginReload() {
		aSources[2].Play();
	}

	public void EndReload() {
		ammo = maxAmmo;
	}
}
