﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
  [Header("Optional Settings")]
	public GameObject shatterPrefab = null;
  public Material piecesMaterial = null;
  [Space(8)]
	public int numOfPieces = 2;
	public float explosionForce = 10;

	void Start() {
		shatterPrefab = shatterPrefab == null ? (GameObject)Resources.Load("Prefabs/Shatter", typeof (GameObject)) : shatterPrefab;
		piecesMaterial = piecesMaterial == null ? transform.GetComponent<Renderer>().material : piecesMaterial;
	}

	public void Detonate(float explosionRadius, Material material = null, GameObject shatter = null, float force = -1, int pieces = -1) {
		material = material == null ? piecesMaterial : material;
		shatter = shatter == null ? shatterPrefab : shatter;
		pieces = pieces < 0 ? numOfPieces : pieces;
		force = force < 0 ? explosionForce : force;

		var posX = transform.position.x;
		var posY = transform.position.y;
		var posZ = transform.position.z;
		if (explosionRadius > 0) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach(Collider coll in colliders) {
				if (coll.GetComponent<Rigidbody>() == null) continue;
				coll.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
			}
		}

		for(int i = numOfPieces; i > 0; i--) {
	    Vector3 randomPos = new Vector3(Random.Range(posX + 1, posX - 1), Random.Range(posY + 1, posY - 1), Random.Range(posZ + 1, posZ - 1));
			var piece = Instantiate(shatter, randomPos, Quaternion.Euler(randomPos));
			piece.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
			piece.GetComponent<Rigidbody>().AddExplosionForce(explosionForce / 2, transform.position, 100, 1, ForceMode.Impulse);
    	piece.GetComponent<Renderer>().material = material;
		}
		Destroy(gameObject);
	}

	public void Detonate(float explosionRadius, float force = -1, int pieces = -1) {
  pieces = pieces < 0 ? numOfPieces : pieces;
  force = force < 0 ? explosionForce : force;

		var posX = transform.position.x;
		var posY = transform.position.y;
		var posZ = transform.position.z;
		if (explosionRadius > 0) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach(Collider coll in colliders) {
				if (coll.GetComponent<Rigidbody>() == null) continue;
				coll.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
			}
		}

		for(int i = numOfPieces; i > 0; i--) {
	    Vector3 randomPos = new Vector3(Random.Range(posX + 1, posX - 1), Random.Range(posY + 1, posY - 1), Random.Range(posZ + 1, posZ - 1));
			var piece = Instantiate(shatterPrefab, randomPos, Quaternion.Euler(randomPos));
			piece.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
			piece.GetComponent<Rigidbody>().AddExplosionForce(explosionForce / 2, transform.position, 100, 1, ForceMode.Impulse);
    	piece.GetComponent<Renderer>().material = piecesMaterial;
		}
		Destroy(gameObject);
	}
}