using UnityEngine;
using System.Collections;

class Rifle : AbstractWeapon {
	override public bool IsAuto() {
		return true;
	}

	override public void FireWeapon() {
		float rayX = transform.position.x;
		float rayY = transform.position.y + 0.085f;
		Vector2 direction = new Vector2(transform.parent.localScale.x, 0);
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(rayX, rayY), direction);

		float posX = hit.point.x + (transform.parent.localScale.x > 0 ? -0.5f : 0.5f);
		float posY = hit.point.y - 0.02f;

		if (hit.collider != null && hit.collider.gameObject.CompareTag("Solid")) {
			if (!GameObject.Find("Decal(Clone)")) {
				Decal iDecal = Instantiate(decal, new Vector2(posX, posY), Quaternion.identity) as Decal;
				iDecal.transform.localScale = transform.parent.localScale;
			}
		}

		aSources[0].Play();
		ammo--;
	}
}
