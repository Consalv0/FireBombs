using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
	public Transform target;
	public float radius;
	public float mass;

	void Start() {
		mass = mass < 0 ? -mass : mass;
	}

	void Update() {
		transform.GetComponent<Rigidbody>().AddExplosionForce(-mass, target.position, radius);
	}
}
