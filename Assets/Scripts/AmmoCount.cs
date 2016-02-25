using UnityEngine;
using System.Collections;

public class AmmoCount : MonoBehaviour {
	public Texture2D ammoTex;
	public Vector2 offset;

	private int ammo;
	private Weapon weapon;

	void Start() {
		weapon = GameObject.FindObjectOfType<AbstractWeapon>();
	}

	void OnGUI() {
		DrawAmmoCount(ammoTex);
	}

	void DrawAmmoCount(Texture2D texture) {
		for (int i = 0; i < ammo; i++) {
			float offX = offset.x + (i * (texture.width + 1)); 
			float offY = Screen.height - offset.y - texture.height;
			GUI.DrawTexture(new Rect(offX, offY, texture.width, texture.height), texture);
		}
	}

	void Update() {
		ammo = weapon.GetAmmo();
	}
}
