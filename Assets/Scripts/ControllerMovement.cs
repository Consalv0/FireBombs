using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public Transform camTransform;
	public float speedFactor = 10f;
	public float sprintMultiplier = 1.6f;

	[Range(0.05f, 0.5f)]
	public float rotationSmoothing = 0.18f;
	Vector2 rotationSmoothVelocity;
	[Range(0.05f, 3.5f)]
	public float speedSmoothing = 1.5f;
	float speedSmoothVelocity;
	float curretSpeed;

	bool sprinting;
	[SerializeField]
	float targetSpeed;
	Vector2 input;
	Vector2 inputDir;
	[SerializeField]
	float inputSpeed;
	[SerializeField]
	Vector2 targetRotation;

	#if UNITY_STANDALONE_OSX
	bool rightTriggerReady;
	#endif

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		if (camTransform == null)
			camTransform = Camera.main.transform;
	}

	void Update () {
		input = new Vector2(Input.GetAxisRaw("Left Horizontal"), Input.GetAxisRaw("Left Vertical"));
		inputDir = input.normalized;
		if (inputDir != Vector2.zero) {
			targetRotation = new Vector2(camTransform.eulerAngles.x, camTransform.eulerAngles.y);
			transform.eulerAngles = Vector3.right * Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.x, ref rotationSmoothVelocity.x, rotationSmoothing)
														+ Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref rotationSmoothVelocity.y, rotationSmoothing);
		}

		sprinting = Input.GetButton("Sprint");
		inputSpeed = Input.GetButton("Right Trigger") ? 1 : 0;
	#if UNITY_STANDALONE_WIN
		inputSpeed += Input.GetAxisRaw("Right Trigger");
	#endif
	#if UNITY_STANDALONE_OSX
		inputSpeed += MacTrigger("Right", ref rightTriggerReady);
	#endif
		targetSpeed = ((sprinting) ? speedFactor * sprintMultiplier : speedFactor) * inputSpeed; // inputDir.magnitude;
		curretSpeed = Mathf.SmoothDamp(curretSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothing);
		transform.Translate(transform.forward * targetSpeed * Time.deltaTime, Space.World);
	}

	public float MacTrigger(string side, ref bool triggerReady) {
		float adjustedAxis = 0f;
		float timeAxis = Input.GetAxisRaw("Mac" + side + " Trigger");

		if((timeAxis > -0.9f && timeAxis < -0.0001f) && triggerReady == false)
			triggerReady = true;
		if (triggerReady)
			adjustedAxis = (timeAxis + 1) * 0.5f;

		return adjustedAxis;
	}
}
