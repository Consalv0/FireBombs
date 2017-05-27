using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public Transform cameraTransform = null;
	public float speed = 10f;
	public float sprintMultiplier = 1.6f;

	[Range(0.05f, 0.5f)]
	public float rotationSmoothing = 0.18f;
	float rotationSmoothVelocity;
	[Range(0.05f, 3.5f)]
	public float speedSmoothing = 1.5f;
	float speedSmoothVelocity;
	float curretSpeed;
	bool sprinting;
	float targetSpeed;
	Vector2 input;
	Vector2 inputDir;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		if (cameraTransform == null)
			cameraTransform = Camera.main.transform;
	}

	void Update () {
		input = new Vector2(Input.GetAxisRaw("Left Horizontal"), Input.GetAxisRaw("Left Vertical"));
		inputDir = input.normalized;
		if (inputDir != Vector2.zero) {
			Vector2 targetRotation = new Vector2(cameraTransform.eulerAngles.x, Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y);
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref rotationSmoothVelocity, rotationSmoothing)
			 											+ Vector3.right * targetRotation.x;
		}

		sprinting = (Input.GetButton("Sprint"));
		targetSpeed = ((sprinting) ? speed * sprintMultiplier : speed) * inputDir.magnitude;
		curretSpeed = Mathf.SmoothDamp(curretSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothing);
		transform.Translate(transform.forward * curretSpeed * Time.deltaTime, Space.World);
	}
}
