using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public Transform cameraTransform = null;
	public float speed = 10f;
	public float sprintMultiplier = 1.6f;

	public float rotationSmoothTime = 0.18f;
	float rotationSmoothVelocity;
	public float speedSmoothTime = 0.18f;
	float speedSmoothVelocity;
  [SerializeField]
	float curretSpeed;

	bool sprinting;
	float targetSpeed;
  [SerializeField]
	Vector2 input;
  [SerializeField]
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
			float targetRotationY = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
			float targetRotationX = cameraTransform.eulerAngles.x;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotationY, ref rotationSmoothVelocity, rotationSmoothTime)
			 											+ Vector3.right * targetRotationX;
		}

		sprinting = (Input.GetButton("Sprint"));
		targetSpeed = ((sprinting) ? speed * sprintMultiplier : speed) * inputDir.magnitude;
		curretSpeed = Mathf.SmoothDamp(curretSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
		transform.Translate(transform.forward * curretSpeed * Time.deltaTime, Space.World);
	}
}
