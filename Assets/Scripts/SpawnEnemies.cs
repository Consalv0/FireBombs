using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
	public int Quantity;
	public float SecondsBetween;
	public GameObject Target;
	public GameObject Enemy;
	public float SpawnPositionZ;
	public float SpawnRangeX;

	void Start() {
		StartCoroutine(createEnemies());
	}

	IEnumerator createEnemies() {
		var i = 0;
		while (i < Quantity) {
			newEnemy();
			i++;
			yield return new WaitForSeconds(SecondsBetween);
		}
	}

	void newEnemy() {
		float RangeX = Random.Range(-SpawnRangeX, SpawnRangeX);
		var enemy = Instantiate(Enemy, new Vector3(RangeX, 0, SpawnPositionZ), Quaternion.identity);
		if (enemy.GetComponent<Gravity>() != null)
			enemy.GetComponent<Gravity>().Target = Target.transform;
	}
}
