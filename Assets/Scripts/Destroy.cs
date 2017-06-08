﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Destroy : MonoBehaviour {
	[HideInInspector]
	public bool shatter; // Is Shatter?
	[HideInInspector]
	public GameObject shatterPrefab; // Prefab of shatters
	[HideInInspector]
	public Material piecesMaterial; // Material of the shatters
	[HideInInspector]
	public int numOfPieces = 2; // Number of shatters

	public float explosionForce = 10; // Force of the explosion
	public float timeBeforeExplosion = 7; // Time before explosion
	public float explosionRadius = 100; // Radius of explosion
	[Range(0, 100)]
	public float explosionDamage; // Damage of explosion

	void Start() {
		/* If not given prefab and material, take the default and take the first material of the object */

		shatterPrefab = shatterPrefab ? shatterPrefab : (GameObject)Resources.Load("Prefabs/Shatter", typeof(GameObject));
		piecesMaterial = piecesMaterial ? piecesMaterial : transform.GetComponentInChildren<Renderer>().materials[0];
	}

	public void Go(float radius = -1, float force = -1, int pieces = -1) {
		/* Because you can execute this outside the script I decided to make some custom propeties */

  	pieces = pieces < 0 ? numOfPieces : pieces;
		force = force < 0 ? explosionForce : force;
		radius = radius < 0 ? explosionRadius : radius;

		/* Make a simultaneous function that will make a count down and flickering switching materials and when the time is over, then make them explode
		 * and create a sphere, every object that is inside the spehere will take damage and will be pushed away with a axplosion force,
		 * at the end, only if shatter is active the object will instantiate shatters in a small random position range and pushed away with a explosion force */

		StartCoroutine(FlickBeforeShatter(pieces, force, radius));
	}

	IEnumerator FlickBeforeShatter(int pieces, float force, float radius) {
		var startTime = timeBeforeExplosion;
		while (timeBeforeExplosion > 0) {
			timeBeforeExplosion -= 0.1f + Mathf.Abs(timeBeforeExplosion / startTime);
			yield return new WaitForSeconds(timeBeforeExplosion / startTime);
			GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Materials/RedMetallic", typeof(Material));
			yield return new WaitForSeconds(timeBeforeExplosion / startTime * 0.5f);
			GetComponentInChildren<Renderer>().material = piecesMaterial;
		}
		if (radius > 0) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, radius/4);
			foreach (Collider coll in colliders) {
				if (coll.GetComponent<Rigidbody>() == null) continue;
				coll.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, 1, ForceMode.Impulse);
				if (explosionDamage > 0) {
					if (coll.GetComponent<Entity>() != null) {
						coll.GetComponent<Entity>().TakeDamage(explosionDamage);
					}
				}
			}
		}

		if (shatter) {
			var posX = transform.position.x;
			var posY = transform.position.y;
			var posZ = transform.position.z;
			for (int i = pieces; i > 0; i--) {
				Vector3 randomPos = new Vector3(Random.Range(posX + 1, posX - 1), Random.Range(posY + 1, posY - 1), Random.Range(posZ + 1, posZ - 1));
				var piece = Instantiate(shatterPrefab, randomPos, Quaternion.Euler(randomPos));
				piece.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
				piece.GetComponent<Rigidbody>().AddExplosionForce(force / 2, transform.position, 100, 1, ForceMode.Impulse);
				piece.GetComponent<Renderer>().material = piecesMaterial;
			}
		}
		Destroy(gameObject);
	}
}

/* This is because a custom Editor GUI Layout */
#if UNITY_EDITOR
[CustomEditor(typeof(Destroy))]
public class DestroyEditor : Editor {
	bool foldout = true;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Destroy script = (Destroy)target;
		EditorGUILayout.BeginHorizontal();
		foldout = EditorGUILayout.Foldout(foldout, new GUIContent("Shatter", "Split the object into shatters"));
		script.shatter = EditorGUILayout.ToggleLeft("", script.shatter);
		EditorGUILayout.EndHorizontal();

		if (EditorGUILayout.BeginFadeGroup(foldout ? 1 : 0)) {
			GUI.enabled = script.shatter;
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField("Optional Settings", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			script.shatterPrefab = EditorGUILayout.ObjectField("Shatter", script.shatterPrefab, typeof(GameObject), true) as GameObject;
			script.piecesMaterial = EditorGUILayout.ObjectField("Material", script.piecesMaterial, typeof(Material), true) as Material;
			EditorGUI.indentLevel--;
			script.numOfPieces = EditorGUILayout.IntField("Amount", script.numOfPieces);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();
	}
}
#endif
