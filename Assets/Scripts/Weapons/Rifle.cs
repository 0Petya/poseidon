using UnityEngine;
using System.Collections;

class Rifle : AbstractWeapon {
	override public bool IsAuto() {
		return true;
	}

	override public void Shoot() {
		float rayX = player.transform.position.x;
		float rayY = player.transform.position.y + (controller.stance == 2 ? 0.085f : 0.075f);
		Vector2 direction = new Vector2(player.transform.localScale.x, 0);
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(rayX, rayY), direction);

		float posX = hit.point.x + (player.transform.localScale.x > 0 ? -0.5f : 0.5f);
		float posY = hit.point.y + (player.transform.localScale.x > 0 ? -0.02f : 0.05f);
		Quaternion rotation = Quaternion.Euler(0, 0, (player.transform.localScale.x > 0 ? 0: 180));

		if (hit.collider != null && hit.collider.gameObject.CompareTag("Solid")) {
			if (!GameObject.Find("Decal(Clone)"))
				Instantiate(decal, new Vector2(posX, posY), rotation);
		}

		aSources[0].Play();
		ammo--;
	}
}
