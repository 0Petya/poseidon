using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Rigidbody2D rb;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Solid")) {
			Destroy(gameObject);
		}
	}

	public void Fire(float vel, bool flip) {
		transform.parent = null;
    transform.localScale = new Vector3(1, 1, 1);

    if (flip) {
      transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180 - transform.rotation.eulerAngles.z);

      GetComponent<BoxCollider2D>().enabled = false;
      GetComponent<BoxCollider2D>().enabled = true;
    }

		GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * vel;
	}
}
