using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public Transform camTransform;
	public float maxSpeed = 10f;
	public float sprintMultiplier = 1.6f;
	public Vector3 rotationSmoothing = new Vector3(0.5f, 0.3f, 0.3f);
	Vector3 rotationSmoothVelocity;
	[Range(0.05f, 3.5f)]
	public float speedSmoothing = 1.5f;

	float speedSmoothVelocity;
	[SerializeField]
	float curretSpeed;

	bool sprinting;
	float targetSpeed;
	Vector2 input;
	Vector2 inputDir;
	float inputSpeed;
	Vector3 targetRotation;

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
		// if (inputDir != Vector2.zero || Input.GetButton("Right Trigger")) {
		targetRotation = new Vector3(Mathf.Asin(input.y) * Mathf.Rad2Deg + transform.eulerAngles.x,
																 Mathf.Asin(input.x) * Mathf.Rad2Deg + transform.eulerAngles.y,
		                             input.x * -20);
		transform.eulerAngles = new Vector3(Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.x, ref rotationSmoothVelocity.x, rotationSmoothing.x),
																				Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref rotationSmoothVelocity.y, rotationSmoothing.y),
																				Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation.z, ref rotationSmoothVelocity.z, rotationSmoothing.z));
		// }

		sprinting = Input.GetButton("Sprint");
		inputSpeed = Input.GetButton("Right Trigger") ? 1 : 0;
	#if UNITY_STANDALONE_WIN
		inputSpeed += Input.GetAxisRaw("Right Trigger");
	#endif
	#if UNITY_STANDALONE_OSX
		inputSpeed += MacTrigger("Right", ref rightTriggerReady);
	#endif
		targetSpeed = ((sprinting) ? maxSpeed * sprintMultiplier : maxSpeed) * inputSpeed; // inputDir.magnitude;
		curretSpeed = Mathf.SmoothDamp(curretSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothing);
		transform.Translate(transform.forward * curretSpeed * Time.deltaTime, Space.World);
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
