﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	public float ExplosionRadius;
	public float ExplosionForce;
	public float LifeSpawn;

	void Start() {
    StartCoroutine(Flick());
	}

	void Update() {
		LifeSpawn -= 0.5f;
		if(LifeSpawn <= 0) {
			GetComponentInParent<Destroy>().Detonate(ExplosionRadius, ExplosionForce, 4);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Enemy>() != null) {
			GetComponentInParent<Destroy>().Detonate(ExplosionRadius, ExplosionForce, 4);
			if (other.GetComponent<Destroy>() != null)
				other.GetComponent<Destroy>().Detonate(0);
		}
	}

  private IEnumerator Flick() {
		yield return new WaitForSeconds(LifeSpawn/100);
		GetComponentInParent<Renderer>().material = (Material)Resources.Load("Materials/RedMetallic", typeof(Material));
    yield return new WaitForSeconds(LifeSpawn/80);
		GetComponentInParent<Renderer>().material = (Material)Resources.Load("Materials/BlackMetallic", typeof(Material));
	  StartCoroutine(Flick());
  }
}
