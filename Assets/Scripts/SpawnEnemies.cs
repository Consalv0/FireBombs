using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
	public int quantity;
	public float secondsBetween;
	public GameObject target;
	public GameObject enemyPrefab;
	public float spawnPositionZ;
	public float spawnRangeX;

	void Start() {
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
		var enemy = Instantiate(enemyPrefab, new Vector3(RangeX, 0, spawnPositionZ), Quaternion.identity);
		if (enemy.GetComponent<Gravity>() != null)
			enemy.GetComponent<Gravity>().target = target == null ? null : target.transform;
	}
}
