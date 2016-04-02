using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	private GameObject target;
	private Transform t;

	void Awake() {
		GetComponent<Camera>().orthographicSize = ((Screen.height / 2.0f) / 100f);
	}

  void Update() {
    if (!target) {
      target = GameObject.FindGameObjectWithTag("Player");
      if (target) t = target.transform;
    }

		transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);
	}
}
