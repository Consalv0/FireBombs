using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour {
	public Transform startPosition;
	public GameObject bombPrefab;
	public float distance;
	public float minLife;
	public float maxLife;

	GameObject bomb = null;
	float lifeSpawn = 0;
	float x, z;

	void LateUpdate() {
		if (Input.GetButton("Fire1")) { //&& bomb == null) {
			lifeSpawn++;
			x = (Input.mousePosition.x / Screen.width) * 2;
			z = Input.mousePosition.y / Screen.height;
			x = (x - 1) * distance;
			z = z * distance;
		}

		if (Input.GetButtonUp("Fire1")) { //&& bomb == null) {
			lifeSpawn = lifeSpawn <= minLife ? minLife : lifeSpawn >= maxLife ? maxLife : lifeSpawn;
			NewBomb(new Vector3(x, 0, z), lifeSpawn);
			lifeSpawn = 0;
		}
	}

	public void NewBomb(Vector3 velocity, float lifeSpawn) {
		bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
		bomb.GetComponentInChildren<Bomb>().lifeSpawn = lifeSpawn;
		bomb.GetComponent<Rigidbody>().velocity = velocity;
		bomb.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 0));
	}
}
