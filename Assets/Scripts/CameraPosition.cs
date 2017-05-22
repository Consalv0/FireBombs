using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour {
	public GameObject Target;
	public Vector3 Offset;

	void LateUpdate() {
		if (Target != null)
			transform.position = Target.transform.position + Offset;
	}
}
