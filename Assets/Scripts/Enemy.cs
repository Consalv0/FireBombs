using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	void FixedUpdate () {
		if (GetComponent<Rigidbody>().velocity != Vector3.zero)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-GetComponent<Rigidbody>().velocity, Vector3.up), 2.5f);
	}
}
