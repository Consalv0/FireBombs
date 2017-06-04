using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour {
	public GameObject bombPrefab;
	public float force;
	public float minLife;
	public float maxLife;

	GameObject bomb;
	float lifeSpawn;

	void LateUpdate() {
		if (Input.GetButton("Fire1")) { //&& bomb == null) {
			lifeSpawn += 0.1f;
		}

		if (Input.GetButtonUp("Fire1")) { //&& bomb == null) {
			lifeSpawn = lifeSpawn <= minLife ? minLife : lifeSpawn >= maxLife ? maxLife : lifeSpawn;
			NewBomb(transform.position + transform.forward * -3, transform.forward * -force, lifeSpawn);
			lifeSpawn = 0;
		}
	}

	public GameObject NewBomb(Vector3 position, Vector3 velocity, float lifeSpawn) {
		bomb = Instantiate(bombPrefab, position, Quaternion.identity);
		bomb.GetComponentInChildren<Bomb>().lifeSpawn = lifeSpawn;
		bomb.GetComponent<Rigidbody>().velocity = velocity;
		bomb.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 0));
		return bomb;
	}
}
