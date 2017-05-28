using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public Transform camTransform = null;
	public float speedFactor = 10f;
	public float sprintMultiplier = 1.6f;

	[Range(0.05f, 0.5f)]
	public float rotationSmoothing = 0.18f;
	float rotationSmoothVelocity;
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


	#if UNITY_STANDALONE_OSX
	bool rightTriggerReady = false;
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
			Vector2 targetRotation = new Vector2(camTransform.eulerAngles.x, Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camTransform.eulerAngles.y);
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref rotationSmoothVelocity, rotationSmoothing)
			 											+ Vector3.right * targetRotation.x;
		}

		sprinting = Input.GetButton("Sprint");
		#if UNITY_STANDALONE_OSX
			inputSpeed = MacTrigger("Right", ref rightTriggerReady);
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
