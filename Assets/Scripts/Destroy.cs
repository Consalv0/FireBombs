﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
	public GameObject Shatter = null;
  public Material PiecesMaterial = null;
	public int NumOfPieces = 2;
	public float ExplosionForce = 10;

	void Start() {
		Shatter = Shatter == null ? (GameObject)Resources.Load("Prefabs/Shatter", typeof (GameObject)) : Shatter;
		PiecesMaterial = PiecesMaterial == null ? transform.GetComponent<Renderer>().material : PiecesMaterial;
	}

	public void Detonate(float explosionRadius, Material material = null, GameObject shatter = null, int numOfPieces = 0, float explosionForce = 0) {
		material = material == null ? PiecesMaterial : material;
		shatter = shatter == null ? Shatter : shatter;
		numOfPieces = numOfPieces == 0 ? NumOfPieces : numOfPieces;
		explosionForce = explosionForce == 0 ? ExplosionForce : explosionForce;

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

	public void Detonate(float explosionRadius, float explosionForce = 0, int numOfPieces = 0) {
		numOfPieces = numOfPieces == 0 ? NumOfPieces : numOfPieces;
		explosionForce = explosionForce == 0 ? ExplosionForce : explosionForce;

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
			var piece = Instantiate(Shatter, randomPos, Quaternion.Euler(randomPos));
			piece.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
			piece.GetComponent<Rigidbody>().AddExplosionForce(explosionForce / 2, transform.position, 100, 1, ForceMode.Impulse);
    	piece.GetComponent<Renderer>().material = PiecesMaterial;
		}
		Destroy(gameObject);
	}
}
