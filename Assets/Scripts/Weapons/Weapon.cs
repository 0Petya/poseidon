using UnityEngine;
using System.Collections;

public interface Weapon {
	bool IsAuto();
	int GetAmmo();
	void Shoot();
	void DryFire();
	void BeginReload();
	void EndReload();
}
