using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
	public Transform Target;
	public float RotationX;
	public float RotationY;
	public float RotationZ;
	public float Distance = 20;
	public float RotationSpeed = 10f;

	void Start() {
		transform.rotation *= Quaternion.AngleAxis(RotationX, Vector3.left);
    transform.rotation *= Quaternion.AngleAxis(RotationY, Vector3.up);
	  transform.rotation *= Quaternion.AngleAxis(RotationZ, Vector3.forward);
	}

	void LateUpdate() {
	}
}
