using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<LinearMovement>() != null) {
			Destroy(other.gameObject);
			Destroy(transform.parent.gameObject);
		}
	}
}
