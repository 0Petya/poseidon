using UnityEngine;
using System.Collections;

class Rifle : AbstractWeapon {
	override public bool IsAuto() {
		return true;
	}

	override public void FireWeapon() {
		GameObject round = Instantiate(bulletObj, transform.position, transform.rotation) as GameObject;
		round.transform.SetParent(transform, true);
		round.transform.localPosition = new Vector3(0.5f, Random.Range(-0.05f, 0.15f), 0);
		round.GetComponent<Bullet>().Fire(10f, transform.parent.localScale.x > 0 ? false : true);

		aSources[0].Play();
		ammo--;
	}
}
