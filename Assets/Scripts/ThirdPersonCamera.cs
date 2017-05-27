using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
	public Transform target;
	public Vector2 rotationSpeed = new Vector2(10, 5);
	public float zoomSpeed = 10;

	[Range(6f, 80f)]
  public float maxDistance = 20;
	[Range(1f, 6f)]
  public float minDistance = 5;
  public float pitchMax = 85;
  public float pitchMin = -40;

	public float rotationSmoothTime = 0.12f;
	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	Vector3 vectorToCam;
  float yaw;
  float pitch;
  [SerializeField]
  float distFromTarget;

	void LateUpdate() {
  	vectorToCam = transform.position - target.position;
  	RaycastHit hit;
		Debug.DrawRay(target.position, vectorToCam.normalized * distFromTarget, Color.red);
  	if (Physics.Raycast(target.position, vectorToCam.normalized, out hit, maxDistance)) {
    	distFromTarget = (hit.point - target.position).magnitude;
			distFromTarget = distFromTarget < minDistance ? minDistance : distFromTarget;
		} else {
			distFromTarget = maxDistance;
		}
		maxDistance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		maxDistance = Mathf.Clamp(maxDistance, 6, 80);

		yaw += Input.GetAxis("Right Horizontal") * rotationSpeed.x;
		pitch -= Input.GetAxis("Right Vertical") * rotationSpeed.y;
		pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

		currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
		transform.eulerAngles = currentRotation;

   	transform.position = target.position - transform.forward * distFromTarget;
  }
}
