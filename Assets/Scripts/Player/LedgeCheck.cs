using UnityEngine;
using System.Collections;

public class LedgeCheck : MonoBehaviour {
	private Vector2 origin;
	private Vector2 rayWall;
	private Vector2 rayLedge;
	private bool ledge;
	private bool grab;

	void Start() {
		grab = true;
	}

	void Update() {
		if (grab) {
			origin = new Vector2(transform.position.x, transform.position.y);
			rayWall = new Vector2(transform.parent.localScale.x, 0);
			rayLedge = new Vector2(transform.parent.localScale.x, 0.25f);

			RaycastHit2D wall = Physics2D.Raycast(origin, rayWall, 0.35f);
			RaycastHit2D empty = Physics2D.Raycast(origin, rayLedge, 0.75f);

			if (wall.collider != null && wall.collider.gameObject.CompareTag("Solid") && empty.collider == null)
				ledge = true;
			else
				ledge = false;
		}
		else
			ledge = false;
	}

	public void Grab(bool stop) {
		grab = stop;
	}

	public bool IsLedge() {
		return ledge;
	}
}
