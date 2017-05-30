using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
	public Transform target;
	public Vector2 rotationSpeed = new Vector2(10, 5);
	public float zoomSpeed = 10;

	[Range(6f, 40f)]
	public float maxDistance = 20;
	[Range(1f, 6f)]
	public float minDistance = 5;
	[Range(-180f, 180f)]
	public float pitchMax = 85;
	[Range(-180f, 180f)]
	public float pitchMin = -40;

	[Range(0f, 0.8f)]
	public float rotateSmoothTime = 0.12f;
	Vector3 rotateSmoothVelocity;
	Vector3 currentRotation;
	[Range(0f, 0.8f)]
	public float moveSmoothTime = 0.06f;
	Vector3 moveSmoothVelocity;
	Vector3 currentPosition;

	Vector3 vectorToCam;
	float yaw;
	float pitch;
	[SerializeField]
	float distFromTarget;

	void LateUpdate() {
		vectorToCam = transform.position - target.position;
		RaycastHit hit;
		Debug.DrawRay(target.position, vectorToCam.normalized * distFromTarget, Color.red);
		if (Physics.Raycast(target.position, vectorToCam.normalized, out hit, maxDistance, 1 << LayerMask.NameToLayer("Terrain"))) {
			distFromTarget = (hit.point - target.position).magnitude;
			distFromTarget = distFromTarget < minDistance ? minDistance : distFromTarget;
		} else {
			distFromTarget = maxDistance;
		}
		maxDistance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		maxDistance = Mathf.Clamp(maxDistance, 6, 40f);
	#if UNITY_STANDALONE_WIN
		yaw += (Input.GetAxis("Right Horizontal") + Input.GetAxis("Mouse Horizontal")) * rotationSpeed.x;
		pitch -= (Input.GetAxis("Right Vertical") + Input.GetAxis("Mouse Vertical")) * rotationSpeed.y;
	#endif
	#if UNITY_STANDALONE_OSX
		yaw += (Input.GetAxis("MacRight Horizontal") + Input.GetAxis("Mouse Horizontal")) * rotationSpeed.x;
		pitch -= (Input.GetAxis("MacRight Vertical") + Input.GetAxis("Mouse Vertical")) * rotationSpeed.y;
	#endif
		pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotateSmoothVelocity, rotateSmoothTime);
		transform.eulerAngles = currentRotation;

		currentPosition = Vector3.SmoothDamp(currentPosition, target.position - transform.forward * distFromTarget, ref moveSmoothVelocity, moveSmoothTime);
   	transform.position = currentPosition;
  }
}
