using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour {
	Rigidbody rigBody;

	public Transform target;
	public float radius = 1000;
	public float mass = 10;

	void Start() {
		rigBody = GetComponent<Rigidbody>();
		mass = mass < 0 ? -mass : mass;
	}

	void Update() {
		if (target != null)
			rigBody.AddExplosionForce(-mass, target.position, radius);
	}
}
