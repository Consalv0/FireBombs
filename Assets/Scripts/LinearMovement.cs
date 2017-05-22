using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour {
	public Vector3 Target;
	public float Speed;

	void Update () {
		transform.position = transform.position + new Vector3(0, 0 , -Speed/100);
		// transform.position = Vector3.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
	}
}
