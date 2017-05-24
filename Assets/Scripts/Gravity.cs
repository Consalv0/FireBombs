using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
	public Transform Target;
	public float Radius;
	public float Mass;

	void Start() {
		Mass = Mass < 0 ? -Mass : Mass;
	}

	void Update() {
		transform.GetComponent<Rigidbody>().AddExplosionForce(-Mass, Target.position, Radius);
	}
}
