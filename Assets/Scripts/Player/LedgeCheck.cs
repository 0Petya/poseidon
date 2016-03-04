using UnityEngine;
using System.Collections;

public class LedgeCheck : MonoBehaviour {
	private bool grab;

	void Start() {
		grab = true;
	}

	public void Grab(bool stop) {
		grab = stop;
	}

	public bool IsLedge() {
		if (grab) {
			Vector2 origin = new Vector2(transform.position.x, transform.position.y);
			Vector2 rayWall = new Vector2(transform.parent.localScale.x, 0);
			Vector2 rayLedge = new Vector2(transform.parent.localScale.x, 0.25f);

			RaycastHit2D wall = Physics2D.Raycast(origin, rayWall, 0.35f);
			RaycastHit2D empty = Physics2D.Raycast(origin, rayLedge, 0.75f);

			if (wall.collider != null && wall.collider.gameObject.CompareTag("Solid") && empty.collider == null)
				return true;
		}

		return false;
	}
}
