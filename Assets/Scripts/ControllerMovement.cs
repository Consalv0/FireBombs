using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControllerMovement : MonoBehaviour {
	public float MovementSpeed = 10f;
	public float SprintMultiplier = 1.6f;
	public float Sensitivity = 5f;
	public float Smoothing = 2f;

	Vector2 deltaMovement;
	Vector2 smoothV;
	float sprint;
	float x = 0;
	float z = 0;

	void Start () {
		sprint = SprintMultiplier;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void LateUpdate () {
		// Translation
		if (Input.GetButtonDown("Sprint")) {
			if (sprint != 1)
				sprint = 1;
			else
				sprint = SprintMultiplier;
		}

		x = Input.GetAxis("Left Horizontal") * Time.deltaTime * MovementSpeed * SprintMultiplier / sprint;
		z = Input.GetAxis("Left Vertical") * Time.deltaTime * MovementSpeed * SprintMultiplier / sprint;

		// Rotation
		var mouseDirection = new Vector2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"));

		mouseDirection = Vector2.Scale(mouseDirection, new Vector2(Sensitivity * Smoothing, Sensitivity * Smoothing));
		smoothV.x = Mathf.Lerp(smoothV.x, mouseDirection.x, 1 / Smoothing);
		smoothV.y = Mathf.Lerp(smoothV.y, mouseDirection.y, 1 / Smoothing);
		deltaMovement += smoothV;

		// Assign Values
    transform.Translate(x, 0, z);
		transform.rotation = Quaternion.Euler(-deltaMovement.y, deltaMovement.x, 0);
	}
}
