using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBomb : MonoBehaviour {
	public float Distance;
	public GameObject Spaceship;
	public GameObject Bomb;
	public float minLife;
	public float maxLife;

	GameObject bomb = null;
	float lifeSpawn = 0;
	float x, y, z;

	void LateUpdate() {
		if (Input.GetButton("Fire1")) { //&& bomb == null) {
			lifeSpawn++;
			x = (Input.mousePosition.x / Screen.width) * 2;
			z = Input.mousePosition.y / Screen.height;
			y = z * 10;
			x = (x - 1) * Distance;
			z = z * Distance;
		}

		if (Input.GetButtonUp("Fire1")) { //&& bomb == null) {
			lifeSpawn = lifeSpawn <= minLife ? minLife : lifeSpawn >= maxLife ? maxLife : lifeSpawn;
			newBomb(new Vector3(x, y, z), lifeSpawn);
			lifeSpawn = 0;
		}
	}

	void newBomb(Vector3 velocity, float lifeSpawn) {
		bomb = Instantiate(Bomb, Spaceship.transform.position, Quaternion.identity);
		bomb.GetComponentInChildren<Bomb>().LifeSpawn = lifeSpawn;
		bomb.GetComponent<Rigidbody>().velocity = velocity;
		bomb.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 0));
	}
}
