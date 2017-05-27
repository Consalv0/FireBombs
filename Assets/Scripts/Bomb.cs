﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	public float explosionRadius = 100;
	public float explosionForce = 20;
	public float lifeSpawn;

	void Start() {
    StartCoroutine(Flick());
	}

	void Update() {
		lifeSpawn -= 0.5f;
		if(lifeSpawn <= 0) {
			GetComponentInParent<Explode>().Detonate(explosionRadius, explosionForce);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Enemy>() != null) {
			GetComponentInParent<Explode>().Detonate(explosionRadius, explosionForce);
			if (other.GetComponent<Explode>() != null)
				other.GetComponent<Explode>().Detonate(0);
		}
	}

  private IEnumerator Flick() {
		yield return new WaitForSeconds(lifeSpawn/100);
		GetComponentInParent<Renderer>().material = (Material)Resources.Load("Materials/RedMetallic", typeof(Material));
    yield return new WaitForSeconds(lifeSpawn/80);
		GetComponentInParent<Renderer>().material = (Material)Resources.Load("Materials/BlackMetallic", typeof(Material));
	  StartCoroutine(Flick());
  }
}
