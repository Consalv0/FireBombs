﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Bomb : MonoBehaviour {
	public float lifeSpawn;

	void Start() {
		GetComponentInParent<Destroy>().timeBeforeExplosion = lifeSpawn;
		GetComponentInParent<Destroy>().Go();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Enemy>() != null) {
			GetComponentInParent<Destroy>().timeBeforeExplosion = 0;
			GetComponentInParent<Destroy>().Go();
			if (other.GetComponent<Entity>() != null)
				other.GetComponent<Entity>().TakeDamage(GetComponentInParent<Destroy>().explosionDamage * 2);
		}
	}
}
