using UnityEngine;
using System.Collections;

public interface Weapon {
	bool IsAuto();
	int GetAmmo();
	void Shoot(bool trigger);
	void Fire();
	void DryFire();
	void BeginReload();
	void EndReload();
	void DisableAnim(bool disable);
}
