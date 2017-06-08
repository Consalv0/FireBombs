﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

// TODO add to Entity.cs
public class Bomb : MonoBehaviour {
	public float lifeSpawn;

	void Start() {
		/* Destroy the object with a timer */
		GetComponentInParent<Destroy>().timeBeforeExplosion = lifeSpawn;
		GetComponentInParent<Destroy>().Go();
	}

	/* When a object is touched, explode and do damage to the touched object */
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Enemy>() != null) {
			GetComponentInParent<Destroy>().timeBeforeExplosion = 0;
			GetComponentInParent<Destroy>().Go();
			if (other.GetComponent<Entity>() != null)
				other.GetComponent<Entity>().TakeDamage(GetComponentInParent<Destroy>().explosionDamage * 2);
		}
	}
}
