using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
	public int quantity;
	public float secondsBetween;
	public GameObject target;
	public GameObject enemyPrefab;
	public float spawnRangeX;
	public float spawnRangeY;
	public float spawnRangeZ;

	void Start() {
		/* Make a simultaneus function that it will call a function that instantiate the given prefab in a random position range,
		 * if the instantiated gameObject contains the component Gravity then add a target */

		StartCoroutine(createEnemies());
	}

	IEnumerator createEnemies() {
    for (int i = 0; i < quantity; i++) {
      if (secondsBetween > 0)
        yield return new WaitForSeconds(secondsBetween);
      NewEnemy();
    }
	}

	public void NewEnemy() {
		float RangeX = Random.Range(-spawnRangeX, spawnRangeX);
		float RangeZ = Random.Range(-spawnRangeZ, spawnRangeZ);
		float RangeY = Random.Range(-spawnRangeY, spawnRangeY);
		// TODO Optimize Intantiations
		var enemy = Instantiate(enemyPrefab, new Vector3(RangeX, RangeY, RangeZ), Quaternion.identity);
		if (enemy.GetComponent<Gravity>() != null)
			enemy.GetComponent<Gravity>().target = target == null ? null : target.transform;
	}
}
