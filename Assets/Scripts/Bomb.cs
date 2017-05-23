using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	public float ExplosionRadius;
	public float ExplosionForce;
	public float LifeSpawn;

	void Update() {
		LifeSpawn -= 0.5f;
		if(LifeSpawn <= 0) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
			foreach(Collider coll in colliders) {
				if (coll.GetComponent<Rigidbody>() == null) continue;
				coll.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 1, ForceMode.Impulse);
			}
			Destroy(transform.parent.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<LinearMovement>() != null) {
			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
			foreach(Collider coll in colliders) {
				if (coll.GetComponent<Rigidbody>() == null) continue;
				coll.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 1, ForceMode.Impulse);
			}
			Destroy(other.gameObject);
			Destroy(transform.parent.gameObject);
		}
	}
}
