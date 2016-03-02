using UnityEngine;
using System.Collections;

public interface Weapon {
	bool IsAuto();
	int GetAmmo();
	void ResetAim();
	void Diag(bool up);
	void Vert(bool up);
	void Shoot(bool trigger);
	void Fire();
	void DryFire();
	void BeginReload();
	void EndReload();
	void DisableAnim(bool disable);
}
