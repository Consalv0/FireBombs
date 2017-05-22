using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBomb : MonoBehaviour {
	public float Distance;
	public float RangeX;
	public GameObject Spaceship;
	public GameObject Bomb;

	GameObject bomb = null;

	void LateUpdate() {
		if (Input.GetButtonUp("Fire1")) { //&& bomb == null) {
			var x = (Input.mousePosition.x / Screen.width) * 2;
			var z = Input.mousePosition.y / Screen.height;
			var y = z * 4;
			x = (x - 1) * RangeX;
			z = z * Distance;
			newBomb(new Vector3(x, y, z));
		}
	}

	void newBomb(Vector3 velocity) {
		bomb = Instantiate(Bomb, Spaceship.transform.position, Quaternion.identity);
		bomb.GetComponent<Rigidbody>().velocity = velocity;
		bomb.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 0));
	}
}
