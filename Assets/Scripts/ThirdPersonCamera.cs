using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ThirdPersonCamera : MonoBehaviour {
	public Transform target;
	public float zoomSpeed = 10;
	public float maxDistance = 20;
	public float minDistance = 5;
	public float maxDistanceLimit = 100;
	public float minDistanceLimit = 0.1f;
	public float distance;
	float collisionDistance;
	Vector3 vectorToCam;

	[Range(0, 0.8f)]
	public float moveSmoothTime = 0.06f;
	Vector3 moveSmoothVelocity;
	Vector3 currentPosition;

	public Vector2 rotationSpeed = new Vector2(10, 5);
	[Range(0, 0.8f)]
	public float rotateSmoothTime = 0.12f;
	Vector3 rotateSmoothVelocity;
	Vector3 currentRotation;
	public float maxPitch = 85;
	public float minPitch = -40;
	public float maxPitchLimit = 180;
	public float minPitchLimit = -180;
	float yaw;
	float pitch;

	void Start() {
		distance = maxDistance;
	}

	void FixedUpdate() {
		if (target) {
			vectorToCam = transform.position - target.position;
			RaycastHit hit;
			Debug.DrawRay(target.position, vectorToCam.normalized * collisionDistance, Color.red);
			if (Physics.Raycast(target.position, vectorToCam.normalized, out hit, distance, 1 << LayerMask.NameToLayer("Terrain"))) {
				collisionDistance = (hit.point - target.position).magnitude;
				collisionDistance = collisionDistance < minDistance ? minDistance : collisionDistance;
			} else {
				collisionDistance = distance;
			}
			distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
			distance = Mathf.Clamp(distance, minDistance, maxDistance);

		#if UNITY_STANDALONE_WIN
			yaw += (Input.GetAxis("Right Horizontal") + Input.GetAxis("Mouse Horizontal")) * rotationSpeed.x;
			pitch -= (Input.GetAxis("Right Vertical") + Input.GetAxis("Mouse Vertical")) * rotationSpeed.y;
		#endif
		#if UNITY_STANDALONE_OSX
			yaw += (Input.GetAxis("MacRight Horizontal") + Input.GetAxis("Mouse Horizontal")) * rotationSpeed.x;
			pitch -= (Input.GetAxis("MacRight Vertical") + Input.GetAxis("Mouse Vertical")) * rotationSpeed.y;
		#endif
			pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

			currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotateSmoothVelocity, rotateSmoothTime);
			transform.eulerAngles = currentRotation;

			currentPosition = Vector3.SmoothDamp(currentPosition, target.position - transform.forward * collisionDistance, ref moveSmoothVelocity, moveSmoothTime);
			transform.position = currentPosition;
		}
  }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ThirdPersonCamera))]
public class ThirdPersonCameraEditor : Editor {
	public override void OnInspectorGUI() {
		ThirdPersonCamera script = (ThirdPersonCamera)target;
		script.target = EditorGUILayout.ObjectField("Target", script.target, typeof(Transform), true) as Transform;
		script.moveSmoothTime = EditorGUILayout.Slider("Move Smoothing", script.moveSmoothTime, 0, 0.8f);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Zoom", EditorStyles.boldLabel);
		script.zoomSpeed = EditorGUILayout.Slider("Zoom Speed", script.zoomSpeed, 2, 25);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Camera Dist", GUILayout.MaxWidth(100.0f), GUILayout.MinWidth(12.0f));
		GUILayout.FlexibleSpace();
		EditorGUIUtility.labelWidth = 32;
		script.minDistance = EditorGUILayout.FloatField("Min:", script.minDistance, GUILayout.MinWidth(62));
		GUILayout.FlexibleSpace();
		script.maxDistance = EditorGUILayout.FloatField("Max:", script.maxDistance, GUILayout.MinWidth(62));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		EditorGUIUtility.labelWidth = 1;
		EditorGUILayout.LabelField(script.minDistanceLimit.ToString(), GUILayout.MaxWidth(30));
		EditorGUILayout.MinMaxSlider(ref script.minDistance, ref script.maxDistance, script.minDistanceLimit, script.maxDistanceLimit);
		EditorGUIUtility.labelWidth = 1;
		EditorGUILayout.LabelField(script.maxDistanceLimit.ToString(), GUILayout.MaxWidth(30));
		EditorGUIUtility.labelWidth = 0;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
		script.rotationSpeed = EditorGUILayout.Vector2Field("Rotation Speed", script.rotationSpeed);
		script.rotateSmoothTime = EditorGUILayout.Slider("Rotation Smoothing", script.rotateSmoothTime, 0, 0.8f);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Camera Angle", GUILayout.MaxWidth(100.0f), GUILayout.MinWidth(12.0f));
		GUILayout.FlexibleSpace();
		EditorGUIUtility.labelWidth = 32;
		script.minPitch = EditorGUILayout.FloatField("Min:", script.minPitch, GUILayout.MinWidth(62));
		GUILayout.FlexibleSpace();
		script.maxPitch = EditorGUILayout.FloatField("Max:", script.maxPitch, GUILayout.MinWidth(62));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		EditorGUIUtility.labelWidth = 1;
		EditorGUILayout.LabelField(script.minPitchLimit.ToString(), GUILayout.MaxWidth(30));
		EditorGUILayout.MinMaxSlider(ref script.minPitch, ref script.maxPitch, script.minPitchLimit, script.maxPitchLimit);
		EditorGUIUtility.labelWidth = 1;
		EditorGUILayout.LabelField(script.maxPitchLimit.ToString(), GUILayout.MaxWidth(30));
		EditorGUIUtility.labelWidth = 0;
		EditorGUILayout.EndHorizontal();

	}
}
#endif
